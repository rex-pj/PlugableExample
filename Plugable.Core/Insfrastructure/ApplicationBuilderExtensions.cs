using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Plugable.Core.Contracts;
using Plugable.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plugable.Core.Insfrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static void UsePlugins(this IApplicationBuilder app, IWebHostEnvironment env, IList<PluginInfo> plugins)
        {
            var pluginStartupInterfaceType = typeof(IPluginStartup);
            foreach (var module in plugins)
            {
                var pluginStartupType = module.Assembly.GetTypes().FirstOrDefault(x => pluginStartupInterfaceType.IsAssignableFrom(x));
                if (pluginStartupType != null && pluginStartupType != pluginStartupInterfaceType)
                {
                    var moduleInitializer = Activator.CreateInstance(pluginStartupType) as IPluginStartup;
                    moduleInitializer.Configure(app, env);
                }

                var wwwrootDir = new DirectoryInfo(Path.Combine(module.Path, "wwwroot"));
                if (!wwwrootDir.Exists)
                {
                    continue;
                }

                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(wwwrootDir.FullName),
                    RequestPath = new PathString("/" + module.ShortName)
                });
            }
        }
    }
}
