using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.DependencyInjection.Interfaces;

public interface IModule
{
    void ConfigureDependencyInjection(IServiceCollection services);
}
