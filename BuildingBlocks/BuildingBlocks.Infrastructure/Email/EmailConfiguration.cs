namespace BuildingBlocks.Infrastructure.Email;

public class EmailConfiguration(string fromEmail)
{
    public string FromEmail { get;  } = fromEmail;
}