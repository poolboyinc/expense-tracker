using System.Net;
using System.Net.Mail;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;

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


        using var client = new SmtpClient(host, port);
        client.Credentials = new NetworkCredential(username, password);
        client.EnableSsl = true;

        var message = new MailMessage(from!, to, subject, body)
        {
            IsBodyHtml = true 
        };

        await client.SendMailAsync(message);
    }
}