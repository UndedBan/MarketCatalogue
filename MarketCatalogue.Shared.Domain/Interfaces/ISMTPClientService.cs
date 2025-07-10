using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Shared.Domain.Interfaces;

public interface ISMTPClientService
{
    Task<bool> SendAsync(string subject, string body, params string[] recipients);
    Task<bool> SendWithAttachmentsAsync(string subject, string body, Attachment[] attachments, params string[] recipients);
}
