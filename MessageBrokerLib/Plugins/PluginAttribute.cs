using System;

namespace MessageBrokerLib.Plugins
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PluginAttribute : Attribute
    {
        public string Name { get; }

        public PluginAttribute(string name)
        {
            Name = name;
        }
    }
}
