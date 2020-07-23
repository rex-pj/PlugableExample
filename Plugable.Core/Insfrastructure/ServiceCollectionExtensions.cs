using Microsoft.Extensions.DependencyInjection;
using Plugable.Core.Contracts;
using Plugable.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugable.Core.Insfrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IMvcBuilder AddPlugins(this IServiceCollection services, IList<PluginInfo> plugins)
        {
            var mvcBuilder = services.AddControllersWithViews();
            var pluginStartupInterfaceType = typeof(IPluginStartup);
            foreach (var module in plugins)
            {
                mvcBuilder.AddApplicationPart(module.Assembly);

                var pluginStartupType = module.Assembly.GetTypes().FirstOrDefault(x => pluginStartupInterfaceType.IsAssignableFrom(x));
                if (pluginStartupType != null && pluginStartupType != pluginStartupInterfaceType)
                {
                    var moduleInitializer = Activator.CreateInstance(pluginStartupType) as IPluginStartup;
                    moduleInitializer.ConfigureServices(services);
                }
            }

            return mvcBuilder;
        }
    }
}
