using System;
using System.Collections.Generic;
using System.Reflection;

namespace Arrowgene.Ez2Off.Server.Plugin
{
    public class PluginLoader<T> where T : IPlugin
    {
        private ICollection<T> _plugins;
        private Type _type;

        public PluginLoader()
        {
            _type = typeof(T);
            _plugins = new List<T>();
        }

        public ICollection<T> Plugins => new List<T>(_plugins);

        public ICollection<T> Load(ICollection<Assembly> assemblies)
        {
            _plugins.Clear();
            ICollection<Type> pluginTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsInterface || type.IsAbstract || type == typeof(PluginDispatcher))
                    {
                        continue;
                    }
                    else if (type.GetInterface(_type.FullName) != null)
                    {
                        T plugin = (T) Activator.CreateInstance(type);
                        _plugins.Add(plugin);
                    }
                }
            }

            return _plugins;
        }
    }
}