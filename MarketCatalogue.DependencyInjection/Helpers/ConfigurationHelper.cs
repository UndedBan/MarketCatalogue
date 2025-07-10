using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.DependencyInjection.Helpers;

public static class ConfigurationHelper
{
    private static IConfiguration _config = null!;

    public static void Initialize(IConfiguration config)
    {
        _config = config;
    }

    public static T? GetValue<T>(string key)
    {
        return _config.GetValue<T>(key);
    }
}
