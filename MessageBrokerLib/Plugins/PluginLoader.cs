using System.Reflection;


namespace MessageBrokerLib.Plugins
{
    public static class PluginLoader
    {
        public static List<IPlugin> LoadPlugins(string path)
        {
            var plugins = new List<IPlugin>();

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Plugin directory not found: {path}");
            }

            var dllFiles = Directory.GetFiles(path, "*.dll");

            foreach (var dll in dllFiles)
            {
                var assembly = Assembly.LoadFrom(dll);
                var types = assembly.GetTypes()
                    .Where(t => t.GetCustomAttributes<PluginAttribute>().Any() && typeof(IPlugin).IsAssignableFrom(t));

                foreach (var type in types)
                {
                    if (Activator.CreateInstance(type) is IPlugin plugin)
                    {
                        plugins.Add(plugin);
                    }
                }
            }

            return plugins;
        }
    }
}
