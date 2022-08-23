using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LocalProfile : IModule
{
    static BaseProfileData data;
    public static bool isTest;

    
    public static void TestInitialize()
    {
        data = new BaseProfileData();
        isTest = true;
    }

    public IEnumerable<Type> GetDependencies()
    {
        return new List<Type>() { typeof(LocalConfig)};
    }

    public static BaseProfileData Data
    {
        get
        {
            if(data == null)
            {
                data = new BaseProfileData();
            }
            return data;
        }
    }

    static bool loadedOnce;

    const string PROFILE_FILENAME = "profile.json";

    static string LoadRawLocalProfile()
    {
        return FileUtils.LoadTextFile(PROFILE_FILENAME);
    }

    public static void OverrideData(BaseProfileData newData)
    {
        data = newData;
    }

    public static IObservable<bool> LoadLocalProfile()
    {
        string text = "";
        try
        {
            Log.Message("Loading Local profile...");
            text = LoadRawLocalProfile();
            data = JsonUtils.Deserialize<BaseProfileData>(text);
            Log.Message("Profile loaded.");
            loadedOnce = true;
        }
        catch (Exception e)
        {
            Log.Message("Can't load local profile, creating a blank one..."+e);
            data = new BaseProfileData();
            loadedOnce = true;
        }
        data.Migrate();
        return Observable.Return(true);
    }

    public static void OverwriteLocalProfile(BaseProfileData pd )
    {
        FileUtils.SaveTextFile(PROFILE_FILENAME,
            JsonUtils.Serialize(pd)
        );
    }

    public static IObservable<bool> SaveLocalProfile()
    {
        Data?.BeforeSave();
        if (Data == null) return Observable.Return(false);
        if (!loadedOnce) return Observable.Return(false);
        Data.updatedAt = TimeUtils.GetCurrentTimestamp();
        try
        {
            FileUtils.SaveTextFile(PROFILE_FILENAME,
                JsonUtils.Serialize(Data)
            );
            Log.Message("Saved local profile... ");
        }
        catch (Exception err)
        {
            Log.Error(err.ToString());
        }
        return Observable.Return(true);
    }
    public IObservable<bool> Initialize()
    {
        return LoadLocalProfile();
    }

    public static void SaveProfile()
    {
        SaveLocalProfile();
    }

    public void OnApplicationQuit()
    {
        foreach(IModule gm in Core.GetModules())
        {
            gm.OnBeforeQuit();
        }
        SaveLocalProfile();
    }
    
    public void Reset() {
        data = new BaseProfileData();
        data.updatedAt = TimeUtils.GetCurrentTimestamp();
    }
    internal void OnApplicationFocus(bool focused)
    {
        if(!focused)
        {
            SaveLocalProfile();
        }
    }
        
}
