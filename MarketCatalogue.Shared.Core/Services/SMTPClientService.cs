using MarketCatalogue.Shared.Domain.Configurations;
using MarketCatalogue.Shared.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Shared.Application.Services;

public class SMTPClientService : ISMTPClientService
{
    private readonly SmtpConfig _smtpConfig;
    private readonly ILogger<SMTPClientService> _logger;

    public SMTPClientService(IOptions<SmtpConfig> smtpConfig, ILogger<SMTPClientService> logger)
    {
        _smtpConfig = smtpConfig.Value;
        _logger = logger;
    }

    public async Task<bool> SendAsync(string subject, string body, params string[] recipients)
    {
        try
        {
            using var client = CreateClient();

            var mail = CreateMessage(subject, body, recipients);

            await client.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            _logger.LogError("SMTP Error, message: {message}", ex.Message);
            return false;
        }


        return true;
    }

    public async Task<bool> SendWithAttachmentsAsync(string subject, string body, Attachment[] attachments, params string[] recipients)
    {
        try
        {
            using var client = CreateClient();

            var mail = CreateMessage(subject, body, recipients);
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    mail.Attachments.Add(attachment);
                }
            }
            await client.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            _logger.LogError("SMTP Error, message: {message}", ex.Message);
            return false;
        }
        return true;
    }

    private SmtpClient CreateClient()
    {
        var client = new SmtpClient();

        client.DeliveryMethod = SmtpDeliveryMethod.Network;

        client.UseDefaultCredentials = true;

        client.Host = _smtpConfig.Hostname;

        client.Port = _smtpConfig.Port;

        client.EnableSsl = _smtpConfig.UseSsl;

        return client;
    }

    private MailMessage CreateMessage(string subject, string body, params string[] recipients)
    {
        var mail = new MailMessage
        {
            From = new MailAddress(_smtpConfig.DefaultFromAddress, _smtpConfig.DefaultFromAddressName,
                                    Encoding.GetEncoding(_smtpConfig.DefaultMessageEncoding))
        };

        foreach (var recipient in recipients)
            mail.To.Add(new MailAddress(recipient));

        mail.Subject = subject;
        mail.SubjectEncoding = Encoding.GetEncoding(_smtpConfig.DefaultSubjectEncoding);
        mail.Body = body;
        mail.IsBodyHtml = true;

        return mail;
    }
}
