using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plugable.Core
{
    public class PluginViewFinder : IViewLocationExpander
    {
        private readonly Assembly _assembly;
        public PluginViewFinder(Assembly assembly)
        {
            _assembly = assembly;
        }

        private const string _moduleKey = "module";

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.ContainsKey(_moduleKey))
            {
                var module = context.Values[_moduleKey];
                if (!string.IsNullOrWhiteSpace(module))
                {
                    var moduleViewLocations = new string[]
                    {
                        "../../Plugins/Plugin1/Views/{1}/{0}.cshtml",
                        "../../Plugins/Plugin1/Views/Shared/{0}.cshtml"
                    };

                    viewLocations = moduleViewLocations.Concat(viewLocations);
                }
            }
            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var controller = context.ActionContext.ActionDescriptor.DisplayName;
            var moduleName = controller.Split('.')[2];
            if (moduleName != "Plugable.Example")
            {
                context.Values[_moduleKey] = moduleName;
            }
        }
    }
}
