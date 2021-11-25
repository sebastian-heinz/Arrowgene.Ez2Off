using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Arrowgene.Ez2Off.Server.Plugin
{
    public class PluginRegistry
    {
        private Dictionary<Type, ICollection<IPlugin>> _plugins;

        public PluginRegistry()
        {
            _plugins = new Dictionary<Type, ICollection<IPlugin>>();
        }

        public void Load(string directory)
        {
            _plugins.Clear();

            DirectoryInfo directoryInfo;
            try
            {
                directoryInfo = new DirectoryInfo(directory);
            }
            catch (Exception)
            {
                return;
            }

            if (!directoryInfo.Exists)
            {
                return;
            }

            ICollection<Assembly> assemblies = new List<Assembly>();
            assemblies.Add(Assembly.GetExecutingAssembly());
            foreach (FileInfo dllFile in directoryInfo.GetFiles("Arrowgene.StepFile.Plugin.*.dll",
                SearchOption.TopDirectoryOnly))
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile.FullName);
                Assembly assembly = Assembly.Load(an);
                if (assembly != null)
                {
                    assemblies.Add(assembly);
                }
            }

            AddPlugins<IPlugin>(assemblies);
        }

        public ICollection<T> GetPlugins<T>() where T : IPlugin
        {
            ICollection<T> plugins = new List<T>();
            Type key = typeof(T);
            if (_plugins.ContainsKey(key))
            {
                foreach (IPlugin plugin in _plugins[key])
                {
                    plugins.Add((T) plugin);
                }
            }

            return plugins;
        }

        private void AddPlugins<T>(ICollection<Assembly> assemblies) where T : IPlugin
        {
            ICollection<T> plugins = new PluginLoader<T>().Load(assemblies);
            Type key = typeof(T);
            if (!_plugins.ContainsKey(key))
            {
                _plugins.Add(key, new List<IPlugin>());
            }

            foreach (IPlugin plugin in plugins)
            {
                _plugins[key].Add(plugin);
            }
        }
    }
}