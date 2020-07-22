using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plugable.Core;
using Plugable.Core.Insfrastructure;
using Plugable.Core.Models;
using System.Collections.Generic;
using System.IO;

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

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new PluginViewFinder());
            });

            services.AddPlugins(_plugins);
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
