using BuildingBlocks.Application.Data;
using BuildingBlocks.Application.Email;
using Dapper;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.Email;

/// <summary>
/// Đây chỉ là sample đơn giản để minh họa cách gửi email bằng cách lưu trữ vao cơ sở dữ liệu.
/// </summary>
/// <param name="logger"></param>
/// <param name="configuration"></param>
/// <param name="sqlConnectionFactory"></param>
public class EmailSender(
    ILogger<EmailSender> logger,
    EmailConfiguration configuration,
    ISqlConnectionFactory sqlConnectionFactory) : IEmailSender
{
    public async Task SendMessage(EmailMessage message)
    {
        var sqlConnection = sqlConnectionFactory.GetOpenConnection();

        await sqlConnection.ExecuteScalarAsync(
            "INSERT INTO [app].[Emails] ([Id], [From], [To], [Subject], [Content], [Date]) " +
            "VALUES (@Id, @From, @To, @Subject, @Content, @Date) ",
            new
            {
                Id   = Guid.NewGuid(),
                From = configuration.FromEmail,
                message.To,
                message.Subject,
                message.Content,
                Date = DateTime.UtcNow
            });

        logger.LogInformation(
            "Email sent. From: {From}, To: {To}, Subject: {Subject}, Content: {Content}.",
            configuration.FromEmail,
            message.To,
            message.Subject,
            message.Content);
    }
}