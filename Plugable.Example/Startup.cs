using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Plugable.Core;
using Plugable.Core.Insfrastructure;
using Plugable.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plugable.Example
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IList<PluginInfo> _plugins;

        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var rootPath = Path.Combine(Directory.GetParent(_webHostEnvironment.ContentRootPath).FullName, "Plugins");
            _plugins = new PluginManager().LoadPlugins(rootPath);

            var mvcBuilder = services.AddPlugins(_plugins);

            //var serviceProvider = services.BuildServiceProvider();
            //_webHostEnvironment.WebRootFileProvider = CreateCompositeFileProvider(serviceProvider);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UsePlugins(env, _plugins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
