using MarketCatalogue.Shared.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Shared.Application.Services;

public class SMTPCommunicatorService : ISMTPCommunicatorService
{
    private readonly ISMTPClientService _smtpClientService;

    public SMTPCommunicatorService(ISMTPClientService smtpClientService)
    {
        _smtpClientService = smtpClientService;
    }

    public async Task<bool> SendEmailConfirmationEmail(string subject, string email, string message, string confirmationUrl)
    {
        var body = await RenderTemplate("ConfirmEmail.html");

        body = body.Replace("!URL!", confirmationUrl);
        body = body.Replace("!MESSAGE!", message);
        return await _smtpClientService.SendAsync(subject, body, [email]);
    }

    public async Task<bool> SendPurchaseConfirmationEmail(string subject, string email, string message)
    {
        var body = await RenderTemplate("PurchaseConfirmationEmail.html");
        body = body.Replace("!MESSAGE!", message);
        return await _smtpClientService.SendAsync(subject, body, [email]);
    }

    public async Task<bool> SendPurchaseCancellationEmail(string subject, string email, string message)
    {
        var body = await RenderTemplate("PurchaseCancellationEmail.html");
        body = body.Replace("!MESSAGE!", message);
        return await _smtpClientService.SendAsync(subject, body, [email]);
    }

    private static async Task<string> RenderTemplate(string filename)
    {
        try
        {
            var templatePath = Path.Combine(@"D:\banlo\VisualStudioProjects\source\repos\MarketCatalogue\MarketCatalogue\wwwroot\emails\", filename);

            if (File.Exists(templatePath))
                return await File.ReadAllTextAsync(templatePath);
            else
                throw new FileNotFoundException($"Template file not found: {templatePath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Cannot read the email template: {ex.Message}");
        }
    }
}
