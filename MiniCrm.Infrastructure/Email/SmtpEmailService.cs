using System.Net;
using System.Net.Mail;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Infrastructure.Email;

public sealed class SmtpEmailService
    : IEmailService
{
    private readonly SmtpSettings _settings;

    public SmtpEmailService(
        SmtpSettings settings)
    {
        _settings = settings;
    }

    public async Task SendAsync(
        string toEmail,
        string subject,
        string body,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(
                _settings.Host))
        {
            throw new InvalidOperationException(
                "SMTP host is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _settings.Username))
        {
            throw new InvalidOperationException(
                "SMTP username is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _settings.Password))
        {
            throw new InvalidOperationException(
                "SMTP password is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
                _settings.FromEmail))
        {
            throw new InvalidOperationException(
                "SMTP from email is not configured.");
        }

        cancellationToken
            .ThrowIfCancellationRequested();

        using var message =
            new MailMessage
            {
                From =
                    new MailAddress(
                        _settings.FromEmail,
                        _settings.FromName),

                Subject = subject,

                Body = body,

                IsBodyHtml = false
            };

        message.To.Add(toEmail);

        using var client =
            new SmtpClient(
                _settings.Host,
                _settings.Port)
            {
                EnableSsl =
                    _settings.EnableSsl,

                UseDefaultCredentials =
                    false,

                Credentials =
                    new NetworkCredential(
                        _settings.Username,
                        _settings.Password)
            };

        await client.SendMailAsync(
            message);
    }
}