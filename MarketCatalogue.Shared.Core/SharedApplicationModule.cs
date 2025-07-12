using MarketCatalogue.DependencyInjection.Interfaces;
using MarketCatalogue.Shared.Application.Services;
using MarketCatalogue.Shared.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
namespace MarketCatalogue.Shared.Application;

public class SharedApplicationModule : IModule
{
    public void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddScoped<ISMTPCommunicatorService, SMTPCommunicatorService>();
        services.AddScoped<ISMTPClientService,  SMTPClientService>();
    }
}
