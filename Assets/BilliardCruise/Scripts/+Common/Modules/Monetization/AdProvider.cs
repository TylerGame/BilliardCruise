using System;
using System.Collections.Generic;

public enum AdType
{
    None,
    Rewarded,
    Interstitial,
    Banner,
}
public class AdProviderConfig : ModuleConfig
{
    public float interstitialTimeInterval = 3 * 60; // 3 minutes

    public Dictionary<AdType, string> adIds = new Dictionary<AdType, string>();
    public Dictionary<AdType, string> debugIds = new Dictionary<AdType, string>();
    public string GetAdId(AdType adType)
    {
        string result = null;

        if (!Core.IsDebug)
        {
            adIds.TryGetValue(adType, out result);
        }

        if(string.IsNullOrEmpty(result))
        {
            debugIds.TryGetValue(adType, out result);
        }

        return result;
    }
}

public class AdProviderData : ModuleData
{
    
}

public interface AdProvider
{
    public bool CanRequestBanner();

    public bool HaveBanner();

    public IObservable<bool> RequestBanner();

    public void DestroyBanner();

    public bool HaveRewarded();


    public IObservable<bool> ShowRewarded();

    public bool HaveInterstitial();
    public IObservable<bool> ShowInterstitial();
}
 