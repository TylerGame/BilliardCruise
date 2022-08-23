using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UniRx;

public class MonetizationConfig : ModuleConfig {

}

public class MonetizationData : ModuleData {

}
public class Monetization : GameModuleConfigured<MonetizationData, MonetizationConfig>
{
    static HashSet<AdProvider> providers = new HashSet<AdProvider>();

    public override void Reset()
    {
        base.Reset();
        providers.Clear();
    }

    // List of modules need to be initialized before
    public override IEnumerable<Type> GetDependencies()
    {
        return new List<Type>() { typeof(LocalConfig), typeof(LocalProfile) };
    }

    // Initialize method
    public override IObservable<bool> Initialize()
    {
        return Observable.Return(true);
    }

    public static void RegisterProvider(AdProvider provider)
    {
        providers.Add(provider);
    }
    
    public static bool HaveRewarded()
    {
        foreach(AdProvider provider in providers)
        {
            if (provider.HaveRewarded()) return true;
            
        }

        return Core.IsDebug;
    }

    public static IObservable<bool> ShowRewarded()
    {
        foreach (AdProvider provider in providers)
        {
            if (provider.HaveRewarded()) return provider.ShowRewarded();
        }

        return Observable.Return(providers.Count == 0);
    }


    public static bool HaveInterstitial()
    {
        foreach (AdProvider provider in providers)
        {
            if (provider.HaveInterstitial()) return true;
        }

        return false;
    }


    public static IObservable<bool> RequestBanner()
    {
        foreach (AdProvider provider in providers)
        {
            if (provider.CanRequestBanner())
            {
                return provider.RequestBanner();
            }
        }
        return Observable.Return(providers.Count == 0);
    }

    public static IObservable<bool> DestroyBanner()
    {
        foreach (AdProvider provider in providers)
        {
            if (provider.HaveBanner())
            {
                provider.DestroyBanner();
                return Observable.Return(true);

            }
        }
        return Observable.Return(providers.Count == 0);
    }
    public static IObservable<bool> ShowInterstital()
    {
        foreach (AdProvider provider in providers)
        {
            if (provider.HaveInterstitial())
            {
                return provider.ShowInterstitial();
            }
        }
        return Observable.Return(providers.Count == 0);
    }


}
