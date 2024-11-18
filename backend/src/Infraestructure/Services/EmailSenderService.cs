using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using Playground.Infraestructure.Configurations;
using Microsoft.Extensions.Options;
using OneOf;
using Playground.Application.Commands.Dtos;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Playground.Infraestructure.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailConfiguration _options;
    private readonly SmtpClient _smtpClient;

    public EmailSenderService(IOptions<EmailConfiguration> _options)
    {
        this._options = _options.Value;

        _smtpClient = new SmtpClient();
    }
    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        await QueueSendEmailAsync(email, "Activate user", confirmationLink);
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode) => throw new NotImplementedException();
    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink) => throw new NotImplementedException();

    public async Task<OneOf<ErrorResponse, string>> QueueSendEmailAsync(string to, string subject, string message)
    {
        if (_options.DebugMode)
        {
            Console.WriteLine($"EMAIL SENDED\nFrom: ${_options.EmailFrom}\nTo: ${to}\nSubject: ${subject}\n${message}");
            return "DEBUG";
        }
        try
        {
            if (!_smtpClient.IsConnected || !_smtpClient.IsAuthenticated)
            {
                _smtpClient.Connect(_options.SmtpHost, _options.SmtpPort, SecureSocketOptions.SslOnConnect);
                _smtpClient.Authenticate(_options.SmtpUser, _options.SmtpPass);
            }

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.EmailFrom)
            };
            email.From.Add(new MailboxAddress("Gateway tropic", _options.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            var builder = new BodyBuilder
            {
                HtmlBody = message
            };
            email.Body = builder.ToMessageBody();

            await _smtpClient.SendAsync(email);
        }
        catch (System.Exception ex)
        {
            return (OneOf<ErrorResponse, string>)new ErrorResponse { Detail = ex.Message };
        }
        return (OneOf<ErrorResponse, string>)string.Empty;
    }
}