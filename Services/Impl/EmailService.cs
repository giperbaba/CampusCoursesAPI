using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using repassAPI.Services.Interfaces;
using repassAPI.Utils;

namespace repassAPI.Services.Impl;

public class EmailService: IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> options)
    {
        _smtpSettings = options.Value;
    }

    public async Task SendEmail(string toEmail, string subject, string body)
    {
        using var smtpClient = new SmtpClient(_smtpSettings.Host)
        {
            Port = _smtpSettings.Port,
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = true
        };

        var email = new MailMessage
        {
            From = new MailAddress(_smtpSettings.FromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        email.To.Add(toEmail);

        await smtpClient.SendMailAsync(email);
    }
}