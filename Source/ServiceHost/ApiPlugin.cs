using System.IO;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ServiceHost
{
    public class ApiPlugin
    {
        public ApiPlugin(DirectoryInfo directory, ILogger<ApiPlugin> logger)
        {
            ApiAssembly = GetAssembly("*.Api.dll", directory);
            CompositionRootAssembly = GetAssembly("*.CompositionRoot.dll", directory);
        }

        public Assembly ApiAssembly { get; }
        public Assembly CompositionRootAssembly { get; }

        private Assembly GetAssembly(string filePattern, DirectoryInfo directory)
        {
            var file = directory.GetFiles("*.Api.dll", SearchOption.TopDirectoryOnly).Single();

            return Assembly.Load(file.FullName);
        }
    }
}
