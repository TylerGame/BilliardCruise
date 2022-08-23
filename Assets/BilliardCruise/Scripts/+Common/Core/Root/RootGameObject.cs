using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RootGameObject : MonoBehaviour
{
    static RootGameObject instance;
    public static RootGameObject Instance => instance;
    
    public IEnumerable<IModule> modules => Core.GetModules();

    public void DestroyMyself()
    {
        Destroy(gameObject);
    }

    public GameObject SpawnGameObject(GameObject prefab, Transform transform)
    {
        return Instantiate(prefab, transform);
    }

    public void OnApplicationQuit()
    {
        foreach (IModule gm in modules)
        {
            gm.OnApplicationQuit();
        }
    }

    public void OnApplicationFocus(bool focused)
    {
        foreach (IModule gm in modules)
        {
            gm.OnApplicationFocus(focused);
        }
    }

    public void OnApplicationPause(bool paused)
    {
        foreach (IModule gm in modules)
        {
            gm.OnApplicationPause(paused);
        }
    }

    public void OnEnable()
    {
        if (modules != null)
        {
            foreach (IModule gm in modules)
            {
                gm.OnEnable();
            }
        }
    }

    public void OnDisable()
    {
        foreach (IModule gm in modules)
        {
            gm.OnDisable();
        }
    }


    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 300;
    }


}