using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace ExpenseTracker.WebApi.Application.Services;

public class EmailService(IConfiguration config) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var host = config["Email:Host"] 
                   ?? throw new InvalidOperationException("Email:Host missing");

        var portString = config["Email:Port"]
                         ?? throw new InvalidOperationException("Email:Port missing");

        var port = int.Parse(portString);

        var username = config["Email:Username"]
                       ?? throw new InvalidOperationException("Email:Username missing");

        var password = config["Email:Password"]
                       ?? throw new InvalidOperationException("Email:Password missing");

        var from = config["Email:From"]
                   ?? throw new InvalidOperationException("Email:From missing");

        
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress("Expense Tracker", from));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;
        
        var bodyBuilder = new BodyBuilder { HtmlBody = body };
        message.Body = bodyBuilder.ToMessageBody();
        
        using var client = new SmtpClient();

        try
        {

            await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls); 
            
            await client.AuthenticateAsync(username, password);
            
            await client.SendAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}