namespace BuildingBlocks.Application.Email;

public interface IEmailSender
{
   Task SendMessage(EmailMessage message);
}