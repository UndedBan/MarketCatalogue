using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Shared.Domain.Interfaces;

public interface ISMTPCommunicatorService
{
    Task<bool> SendEmailConfirmationEmail(string subject, string email, string message, string confirmationUrl);
}
