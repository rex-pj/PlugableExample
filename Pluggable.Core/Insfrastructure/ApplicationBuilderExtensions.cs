using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Pluggable.Core.Contracts;
using Pluggable.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pluggable.Core.Insfrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static void UsePlugins(this IApplicationBuilder app, IWebHostEnvironment env, IList<PluginInfo> plugins)
        {
            foreach (var module in plugins)
            {
                var pluginStartupType = module.Assembly.GetTypes().Where(x => typeof(IPluginStartup).IsAssignableFrom(x))
                    .FirstOrDefault();

                if (pluginStartupType != null && pluginStartupType != typeof(IPluginStartup))
                {
                    var moduleInitializer = (IPluginStartup)Activator.CreateInstance(pluginStartupType);
                    moduleInitializer.Configure(app, env);
                }
            }
        }
    }
}
