using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; // Belangrijk: Zorg dat deze using er is
using System.Text;
using System.Threading.Tasks;

namespace Blazor.News
{
    public static class DependencyNews
    {
        public static void AddSiteNews(this IServiceCollection services, IConfiguration configuration)
        {
            // Laad de assembly van de Blazor.News RCL.
            // Je kunt dit doen door een type uit die assembly op te vragen.
            var rclAssembly = typeof(DependencyNews).Assembly; // Gebruik een type uit de huidige RCL

            var types = rclAssembly.GetTypes()
                         .Where(x => typeof(IDependency).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false });

            foreach (var type in types)
            {
                var dependency = Activator.CreateInstance(type) as IDependency;
                dependency?.AddDependencies(services, configuration);
            }
        }
    }

    public interface IDependency
    {
        void AddDependencies(IServiceCollection services, IConfiguration configuration);
    }
}