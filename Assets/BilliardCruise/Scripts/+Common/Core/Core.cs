using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using System.Reflection;

public static class Core
{
    public static RootGameObject Root { get; private set; }
    public static bool IsDebug { get; private set; }

    static List<IModule> modules;

    static Dictionary<IModule, IEnumerable<Type>> dependencies = new Dictionary<IModule, IEnumerable<Type>>();

    static HashSet<IModule> pending = new HashSet<IModule>();

    static Dictionary<Type, BehaviorSubject<bool>> initialized = new Dictionary<Type, BehaviorSubject<bool>>();

    static MessageBroker rx;
    public static MessageBroker Rx
    {
        get
        {
            if(rx == null)
            {
                rx = new MessageBroker();
            }
            return rx;
        }
    }

    public static IEnumerable<IModule> GetModules()
    {
        return modules;
    }

    static void OnModuleInitialized(IModule module)
    {
        if (module != null)
        {
            Debug.Log("[Core] \"" + module.GetType() + "\" module have been initialized");
            MarkInitialized(module.GetType());
        }
        
        foreach (IModule m in pending.ToList())
        {
            if (dependencies[m].FirstOrDefault(mt => pending.FirstOrDefault(p => p.GetType() == mt) != null) == null)
            {
                pending.Remove(m);
                m.Initialize().SubscribeOnMainThread().Subscribe(state => {
                    if (state)
                    {
                        OnModuleInitialized(m);
                    }
                    else
                    {
                        Debug.LogError("[Core] " + m.GetType() + " failed");
                    }
                });
            }
        }
    }

    public static BehaviorSubject<bool> OnInitialized(Type type)
    {
        if (!initialized.ContainsKey(type))
        {
            initialized[type] = new BehaviorSubject<bool>(false);
        }
        return initialized[type];
    }

    private static void MarkInitialized(Type type)
    {
        if (!initialized.ContainsKey(type))
        {
            initialized[type] = new BehaviorSubject<bool>(true);
        } else {
            initialized[type].Publish(true);
        }
    }



    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        
#if DEBUG
        IsDebug = true;
#endif
        Debug.Log("[Core] Starting initialization...");

        Root = new GameObject().AddComponent<RootGameObject>();
        Root.transform.parent = null;
        Type rootInterface = typeof(IModule);
        modules =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => rootInterface != p && p.IsAbstract == false && rootInterface.IsAssignableFrom(p))
                .Select(type => Activator.CreateInstance(type) as IModule).ToList();



        Debug.Log($"[Core] Found {modules.Count} modules");
        modules.ForEach(m => pending.Add(m));
        modules.ForEach(m => dependencies.Add(m, m.GetDependencies()));

        OnModuleInitialized(null);
    }

    public static GameObject CreateGameObject(IModule module)
    {
        return CreateGameObject(module.GetType().Name);
    }

    public static GameObject CreateGameObject(string name = "Untitled")
    {
        GameObject go = new GameObject();
        go.transform.parent = Root.transform;
        go.transform.name = name;
        return go;
    }
}
