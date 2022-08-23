using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UniRx;

// Log module

public class Log : IModule
{
    static bool isEditor;
    static bool isDebug;

    public static void Message(object obj)
    {
        if (Core.IsDebug)
        {
            Debug.Log(obj.ToString());
        }
    }

    public IObservable<bool> Initialize()
    {
        isDebug = Debug.isDebugBuild;
        isEditor = Application.isEditor;

        return Observable.Return(true);
    }

    public static void Error(object obj)
    {
        if (Core.IsDebug)
        {
            Debug.LogError(obj.ToString());
        }
    }

    public IEnumerable<Type> GetDependencies()
    {
        return new List<Type>();
    }
}