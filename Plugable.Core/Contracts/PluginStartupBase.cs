using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Plugable.Core.Contracts
{
    public class PluginStartupBase : IPluginStartup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
