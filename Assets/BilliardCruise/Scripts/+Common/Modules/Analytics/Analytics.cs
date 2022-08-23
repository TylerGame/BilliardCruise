using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class AnalyticsGO : MonoBehaviour
{
    public void FixedUpdate()
    {
        Analytics.UpdateSessionID();
    }
}

public class AnalyticsConfig : ModuleConfig
{

}

public class AnalyticsData : ModuleData
{

}

public class Analytics : GameModuleConfigured<AnalyticsData, AnalyticsConfig>
{
    static HashSet<AnalyticsProvider> providers = new HashSet<AnalyticsProvider>();

    static string _sessionID;

    public static string SessionId { 
        get { 
            if(_sessionID == null)
            {
                UpdateSessionID();
            }
            return _sessionID;
        }
    }

    static double lastTimeActive = 0;

    // List of modules need to be initialized before
    public override IEnumerable<Type> GetDependencies()
    {
        return new List<Type>() { typeof(LocalConfig), typeof(LocalProfile) };
    }

    internal static void UpdateSessionID()
    {
        double now = TimeUtils.GetCurrentTimestampFull();
        if (_sessionID == null || now - lastTimeActive > 5 * 60)
        {
            _sessionID = UUID.GenerateUUID();
        }
        lastTimeActive = now;
    }

    public override void Reset()
    {
        base.Reset();
        providers.Clear();
    }

    // Initialize method
    public override IObservable<bool> Initialize()
    {
        Core.CreateGameObject().AddComponent<AnalyticsGO>();
        UpdateSessionID();
        return Observable.Return(true);
    }

    public static void RegisterProvider(AnalyticsProvider provider)
    {
        providers.Add(provider);
    }

    public static string DictionaryToString(Dictionary<string, string> parameters)
    {
        List<string> results = new List<string>();
        if(parameters != null)
        {
            foreach(string key in parameters.Keys)
            {
                results.Add("\"" + key + "\": \"" + parameters[key] + "\"");
            }
        }
        return "{ " + string.Join(", ", results) + " }";
    }
    
    public static void SendEvent(string eventName, Dictionary<string, string> parameters = null)
    {
        Log.Message("ANALYTICS <<< " + eventName + " <<< " + DictionaryToString(parameters));
        foreach(AnalyticsProvider provider in providers)
        {
            try
            {
                provider.SendEvent(eventName, parameters);
            } catch(Exception err)
            {
                Log.Error(err);
            }
        }
    }
}
