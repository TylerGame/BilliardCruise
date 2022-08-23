using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;



[Serializable]
public class BaseProfileData
{
    public HashTable<string, ModuleData> modules = new HashTable<string, ModuleData>();

    public T GetDataFor<T>() where T : ModuleData, new()
    {
        if(modules.ContainsKey(typeof(T).Name))
        {
            return (T)modules[typeof(T).Name];
        }
        modules[typeof(T).Name] = new T();
        return (T)modules[typeof(T).Name];
    }

    
    public virtual void BeforeSave()
    {

    }
    [JsonProperty]
    internal double updatedAt;

    [JsonProperty]
    internal double lastOnline;

    public virtual void Migrate()
    {
        //Game.Root.Publish(CommonMessage.SettingsUpdate, null);
    }

}
