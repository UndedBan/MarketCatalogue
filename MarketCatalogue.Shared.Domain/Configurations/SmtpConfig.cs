using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.Shared.Domain.Configurations;

public class SmtpConfig
{
    public string DefaultFromAddress { get; set; } = null!;
    public string DefaultFromAddressName { get; set; } = null!;
    public string DefaultMessageEncoding { get; set; } = null!;
    public string DefaultSubjectEncoding { get; set; } = null!;
    public string Hostname { get; set; } = null!;
    public int Port { get; set; }
    public string Username { get; set; } = null!;
    public bool UseSsl { get; set; }
    public bool UseCredentials { get; set; }
}
