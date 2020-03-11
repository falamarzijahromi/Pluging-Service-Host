using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceHost
{
    public static class PluginExtension
    {
        public static IServiceCollection RegisterPlugins(
            this IServiceCollection serviceCollection, 
            string pluginsDirectory,
            ILogger<ApiPlugin> logger)
        {
            var allPluginDirectories = GetAllPluginsDirectories(pluginsDirectory);

            var pluginList = GetAllPlugins(allPluginDirectories, logger);

            RegisterPluginsApis(pluginList, logger, serviceCollection);

            RegisterPluginsDependencies(pluginList, logger, serviceCollection);

            LoadModuleDependencies(allPluginDirectories, logger);

            return serviceCollection;
        }

        private static void LoadModuleDependencies(DirectoryInfo[] allPluginDirectories, ILogger<ApiPlugin> logger)
        {
            var loadedAssembelies = new List<string>();

            foreach (var pluginDirectory in allPluginDirectories)
            {

            }
        }

        private static DirectoryInfo[] GetAllPluginsDirectories(string pluginsDirectory)
        {
            var pluginsDirectoryInfo = new DirectoryInfo(pluginsDirectory);

            return pluginsDirectoryInfo.GetDirectories("*.plugin", SearchOption.TopDirectoryOnly);
        }

        private static void LoadModuleDependencies(List<ApiPlugin> pluginList, ILogger<ApiPlugin> logger)
        {
            
        }

        private static void RegisterPluginsDependencies(List<ApiPlugin> pluginList, ILogger<ApiPlugin> logger, IServiceCollection serviceCollection)
        {
            foreach (var plugin in pluginList)
            {
                serviceCollection
                    .AddControllers()
                    .AddApplicationPart(plugin.ApiAssembly);
            }
        }

        private static void RegisterPluginsApis(List<ApiPlugin> pluginList, ILogger<ApiPlugin> logger, IServiceCollection serviceCollection)
        {
            foreach (var plugin in pluginList)
            {
                var compositionType = plugin.CompositionRootAssembly.GetType("Composition");

                var registerDependenciesMethod = compositionType.GetMethod("RegisterDependencies");

                registerDependenciesMethod.Invoke(null, new[] { serviceCollection });
            }
        }

        private static List<ApiPlugin> GetAllPlugins(DirectoryInfo[] allPluginDirectories, ILogger<ApiPlugin> logger)
        {
            var pluginList = new List<ApiPlugin>();

            foreach (var pluginDir in allPluginDirectories)
            {
                var plugin = new ApiPlugin(pluginDir, logger);

                pluginList.Add(plugin);
            }

            return pluginList;
        }
    }
}
