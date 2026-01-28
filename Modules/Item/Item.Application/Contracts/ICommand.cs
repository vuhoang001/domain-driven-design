using MediatR;

namespace Item.Application.Contracts;

/// <summary>
/// Interface cho command có trả về kết quả
/// out TResult: Kiểu kết quả là covariant -> ICommand<Dog> dùng được ở chỗ cần ICommand<Animal> (Dog kế thừa Animal).
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface ICommand<out TResult> : IRequest<TResult>
{
    Guid Id { get;  }
}

public interface ICommand : IRequest
{
    Guid Id { get;  }
}