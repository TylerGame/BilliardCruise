using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConfigData : BaseConfigData
{
    // Here will be overrided configs for modules
    public HashTable<string, ModuleConfig> modules = new HashTable<string, ModuleConfig>()
    {
        
    };

    public T GetConfigFor<T>() where T : ModuleConfig, new()
    {
        if (modules.ContainsKey(typeof(T).Name))
        {
            return (T)modules[typeof(T).Name];
        }
        modules[typeof(T).Name] = new T();
        return (T)modules[typeof(T).Name];
    }

}
