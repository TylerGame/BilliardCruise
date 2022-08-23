using System;
using System.Collections.Generic;
using UniRx;

public class LocalConfig : IModule
{
    static bool isTest = false;

    public static void TestInitialize()
    {
        isTest = true;
        data = new ConfigData();
    }

    public IObservable<bool> Initialize()
    {
        LoadLocalConfig();
        return Observable.Return(true);
    }

    protected static ConfigData data;
    public static ConfigData Data {
        get
        {
            return data;
        }
    }

    public static void SetData(ConfigData _data)
    {
        data = _data;
    }

    static string CONFIG_FILENAME = "config.json";
                
    public static IObservable<bool> LoadLocalConfig()
    {
        ConfigData appDefaultConfig = ConfigData.CreateDefault(); // config came with app
        return Observable.Return(true);
    }

    public IEnumerable<Type> GetDependencies()
    {
        return new List<Type>();
    }
}
