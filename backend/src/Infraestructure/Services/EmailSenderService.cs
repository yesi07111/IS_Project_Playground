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

/// <summary>
/// Servicio para enviar correos electrónicos, incluyendo enlaces de confirmación y restablecimiento de contraseña.
/// Utiliza MailKit para enviar correos a través de un servidor SMTP.
/// </summary>
public class EmailSenderService : IEmailSenderService
{
    private readonly EmailConfiguration _options;
    private readonly SmtpClient _smtpClient;
    private readonly ICodeGenerator _codeGenerator;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EmailSenderService"/>.
    /// </summary>
    /// <param name="options">Configuración de correo electrónico.</param>
    /// <param name="codeGenerator">Generador de códigos para crear códigos de verificación.</param>
    public EmailSenderService(IOptions<EmailConfiguration> options, ICodeGenerator codeGenerator)
    {
        _options = options.Value;
        _smtpClient = new SmtpClient();
        _codeGenerator = codeGenerator;
    }

    /// <summary>
    /// Envía un enlace de confirmación al correo electrónico del usuario.
    /// </summary>
    /// <param name="user">El usuario al que se envía el correo.</param>
    /// <param name="email">La dirección de correo electrónico del destinatario.</param>
    /// <param name="code">El código de confirmación.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public async Task SendConfirmationLinkAsync(User user, string email, string code)
    {
        string reducedCode = _codeGenerator.GenerateReducedCode(code);

        string message = $"Username: {user.UserName} \nCode: {reducedCode}";

        await QueueSendEmailAsync(email, "Activate user", message);
    }

    /// <summary>
    /// Envía un código de restablecimiento de contraseña al correo electrónico del usuario.
    /// </summary>
    /// <param name="user">El usuario al que se envía el correo.</param>
    /// <param name="email">La dirección de correo electrónico del destinatario.</param>
    /// <param name="resetCode">El código de restablecimiento de contraseña.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode) => throw new NotImplementedException();

    /// <summary>
    /// Envía un enlace de restablecimiento de contraseña al correo electrónico del usuario.
    /// </summary>
    /// <param name="user">El usuario al que se envía el correo.</param>
    /// <param name="email">La dirección de correo electrónico del destinatario.</param>
    /// <param name="resetLink">El enlace de restablecimiento de contraseña.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink) => throw new NotImplementedException();

    /// <summary>
    /// Cola el envío de un correo electrónico.
    /// </summary>
    /// <param name="to">La dirección de correo electrónico del destinatario.</param>
    /// <param name="subject">El asunto del correo electrónico.</param>
    /// <param name="message">El cuerpo del mensaje del correo electrónico.</param>
    /// <returns>Un resultado que indica el éxito o el error del envío.</returns>
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
        catch (Exception ex)
        {
            return (OneOf<ErrorResponse, string>)new ErrorResponse { Detail = ex.Message };
        }
        return (OneOf<ErrorResponse, string>)string.Empty;
    }
}