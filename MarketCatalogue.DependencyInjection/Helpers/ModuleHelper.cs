using MarketCatalogue.DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarketCatalogue.DependencyInjection.Helpers;

public class ModuleHelper
{
    public static List<IModule> LoadAll()
    {
        //Find all solution assemblies.
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;

        // Get all DLL files starting with MarketCatalogue in base directory
        var assemblyPaths = Directory.GetFiles(baseDir, "MarketCatalogue*.dll");

        //Filter out all third party assemblies.
        var assemblyNames = assemblyPaths
            .Select(Path.GetFileNameWithoutExtension)
            .Where(n => n!.StartsWith("MarketCatalogue"))
            .ToList();

        //Find all classes that implement IModule.
        return assemblyNames
            .Select(Assembly.Load!)
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IModule).IsAssignableFrom(t) && t.IsClass)
            .Select(t => (IModule)Activator.CreateInstance(t)!)
            .ToList();
    }
}
