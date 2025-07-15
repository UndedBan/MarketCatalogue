using MarketCatalogue.Shared.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Shared.Application.Services;

public class SMTPCommunicatorService : ISMTPCommunicatorService
{
    private readonly ISMTPClientService _smtpClientService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public SMTPCommunicatorService(ISMTPClientService smtpClientService, IWebHostEnvironment webHostEnvironment)
    {
        _smtpClientService = smtpClientService;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<bool> SendEmailConfirmationEmail(string subject, string email, string message, string confirmationUrl)
    {
        var body = await RenderTemplate("ConfirmEmail.html");
        if (body.IsNullOrEmpty())
            return false;
        body = body.Replace("!URL!", confirmationUrl);
        body = body.Replace("!MESSAGE!", message);
        return await _smtpClientService.SendAsync(subject, body, [email]);
    }

    public async Task<bool> SendPurchaseConfirmationEmail(string subject, string email, string message)
    {
        var body = await RenderTemplate("PurchaseConfirmationEmail.html");
        if (body.IsNullOrEmpty())
            return false;
        body = body.Replace("!MESSAGE!", message);
        return await _smtpClientService.SendAsync(subject, body, [email]);
    }

    public async Task<bool> SendPurchaseCancellationEmail(string subject, string email, string message)
    {
        var body = await RenderTemplate("PurchaseCancellationEmail.html");
        if (body.IsNullOrEmpty())
            return false;
        body = body.Replace("!MESSAGE!", message);
        return await _smtpClientService.SendAsync(subject, body, [email]);
    }

    private async Task<string> RenderTemplate(string filename)
    {
        try
        {
            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, "emails", filename);

            if (File.Exists(templatePath))
                return await File.ReadAllTextAsync(templatePath);
            else
                throw new FileNotFoundException($"Template file not found: {templatePath}");
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}
