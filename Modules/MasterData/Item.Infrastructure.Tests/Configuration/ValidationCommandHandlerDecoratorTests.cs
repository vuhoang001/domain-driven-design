using FluentValidation;
using FluentValidation.Results;
using Item.Application.Configuration.Commands;
using Item.Application.Contracts;
using Moq;
using Xunit;
using BuildingBlocks.Application;

namespace Item.Infrastructure.Tests.Configuration;

public class ValidationCommandHandlerDecoratorTests
{
    #region Helper Classes and Interfaces

    /// <summary>
    /// Test command without return value
    /// </summary>
    private class TestCommand : CommandBase
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// Validator cho TestCommand
    /// </summary>
    private class TestCommandValidator : AbstractValidator<TestCommand>
    {
        public TestCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email must be valid");
        }
    }

    #endregion

    #region Tests - ValidationCommandHandlerDecorator<T>

    [Fact]
    public async Task Handle_WithValidCommand_CallsDecoratedHandler()
    {
        // Arrange
        var command = new TestCommand { Name = "John", Email = "john@example.com" };
        var mockValidator = new Mock<IValidator<TestCommand>>();
        var mockDecoratedHandler = new Mock<ICommandHandler<TestCommand>>();

        mockValidator.Setup(v => v.Validate(It.IsAny<TestCommand>()))
            .Returns(new ValidationResult());

        var validators = new List<IValidator<TestCommand>> { mockValidator.Object };
        var decorator = new ValidationCommandHandlerDecorator<TestCommand>(validators, mockDecoratedHandler.Object);

        // Act
        await decorator.Handle(command, CancellationToken.None);

        // Assert
        mockDecoratedHandler.Verify(h => h.Handle(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidCommand_ThrowsInvalidCommandException()
    {
        // Arrange
        var command = new TestCommand { Name = "", Email = "invalid-email" };
        
        var validator = new TestCommandValidator();
        var mockDecoratedHandler = new Mock<ICommandHandler<TestCommand>>();

        var validators = new List<IValidator<TestCommand>> { validator };
        var decorator = new ValidationCommandHandlerDecorator<TestCommand>(validators, mockDecoratedHandler.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidCommandException>(
            () => decorator.Handle(command, CancellationToken.None));

        Assert.NotEmpty(exception.Errors);
        Assert.Contains("Name is required", exception.Errors);
        Assert.Contains("Email must be valid", exception.Errors);

        // Decorated handler should not be called when validation fails
        mockDecoratedHandler.Verify(h => h.Handle(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithMultipleValidators_AppliesAllValidations()
    {
        // Arrange
        var command = new TestCommand { Name = "", Email = "" };

        var validator1 = new TestCommandValidator();
        var mockValidator2 = new Mock<IValidator<TestCommand>>();
        
        // Setup second validator to also return errors
        var validationFailure = new ValidationFailure("Name", "Name cannot be empty from validator2");
        mockValidator2.Setup(v => v.Validate(It.IsAny<TestCommand>()))
            .Returns(new ValidationResult(new[] { validationFailure }));

        var mockDecoratedHandler = new Mock<ICommandHandler<TestCommand>>();
        var validators = new List<IValidator<TestCommand>> { validator1, mockValidator2.Object };
        var decorator = new ValidationCommandHandlerDecorator<TestCommand>(validators, mockDecoratedHandler.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidCommandException>(
            () => decorator.Handle(command, CancellationToken.None));

        // Should contain errors from both validators
        Assert.True(exception.Errors.Count >= 2, "Should have at least 2 validation errors");
    }

    [Fact]
    public async Task Handle_WithEmptyValidatorsList_CallsDecoratedHandler()
    {
        // Arrange
        var command = new TestCommand { Name = "John", Email = "john@example.com" };
        var mockDecoratedHandler = new Mock<ICommandHandler<TestCommand>>();

        var validators = new List<IValidator<TestCommand>>();
        var decorator = new ValidationCommandHandlerDecorator<TestCommand>(validators, mockDecoratedHandler.Object);

        // Act
        await decorator.Handle(command, CancellationToken.None);

        // Assert
        mockDecoratedHandler.Verify(h => h.Handle(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDecoratedHandlerThrows_ExceptionPropagates()
    {
        // Arrange
        var command = new TestCommand { Name = "John", Email = "john@example.com" };
        var mockValidator = new Mock<IValidator<TestCommand>>();
        var mockDecoratedHandler = new Mock<ICommandHandler<TestCommand>>();

        mockValidator.Setup(v => v.Validate(It.IsAny<TestCommand>()))
            .Returns(new ValidationResult());

        var expectedException = new InvalidOperationException("Handler error");
        mockDecoratedHandler.Setup(h => h.Handle(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var validators = new List<IValidator<TestCommand>> { mockValidator.Object };
        var decorator = new ValidationCommandHandlerDecorator<TestCommand>(validators, mockDecoratedHandler.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => decorator.Handle(command, CancellationToken.None));

        Assert.Equal("Handler error", exception.Message);
    }

    [Fact]
    public async Task Handle_ValidatesOnlyReturnedErrors_NotNullErrors()
    {
        // Arrange
        var command = new TestCommand { Name = "", Email = "" };
        
        var mockValidator = new Mock<IValidator<TestCommand>>();
        // Return validation result with null error (edge case)
        var errors = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required"),
            null,
            new ValidationFailure("Email", "Email is required")
        };
        
        mockValidator.Setup(v => v.Validate(It.IsAny<TestCommand>()))
            .Returns(new ValidationResult(errors.Where(e => e != null).ToList()));

        var mockDecoratedHandler = new Mock<ICommandHandler<TestCommand>>();
        var validators = new List<IValidator<TestCommand>> { mockValidator.Object };
        var decorator = new ValidationCommandHandlerDecorator<TestCommand>(validators, mockDecoratedHandler.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidCommandException>(
            () => decorator.Handle(command, CancellationToken.None));

        Assert.NotEmpty(exception.Errors);
        mockDecoratedHandler.Verify(h => h.Handle(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }

    #endregion
}
