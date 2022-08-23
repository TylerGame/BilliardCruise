using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UniRx;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerResources : GameModuleConfigured<PlayerResourcesData, PlayerResourcesConfig>
{
    public override IEnumerable<Type> GetDependencies()
    {
        return new List<Type>() { typeof(LocalConfig), typeof(LocalProfile) };
    }
    public override IObservable<bool> Initialize()
    {

        return Observable.Return(true);
    }
    public static bool IsEnough(ResourcePack pack)
    {
        return Data.resources.IsEnough(pack);
    }

    public static double Get(IResource resource)
    {
        return Data.resources.Get(resource);
    }

    public static void AddResources(ResourcePack another, bool publish = true)
    {
        Data.resources.Add(another);
        if(publish) Core.Rx.Publish(new ResourceUpdateEvent());
    }

    public static void AddResources(IResource res, double count, bool publish = true)
    {
        Data.resources.Add(res, count);
        if (publish) Core.Rx.Publish(new ResourceUpdateEvent());
    }

    public static GameObject GetResourceThumb(IResource res)
    {
        return AssetDB.LoadPrefab("PlayerResources/" + res.GetType().Name+"Thumb");
    }

    public static void SpendResources(ResourcePack another, bool publish = true)
    {
        Data.resources.Subtract(another);
        if (publish) Core.Rx.Publish(new ResourceUpdateEvent());
    }
}
