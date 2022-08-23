using System;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;

public interface IModule 
{
    public IObservable<bool> Initialize();

    public IEnumerable<Type> GetDependencies();

    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
    public virtual void OnBeforeQuit() { }
    public virtual void OnApplicationPause() { }
    public virtual void OnApplicationQuit() { }
    public virtual void OnApplicationFocus(bool focused) { }
    public virtual void OnApplicationPause(bool paused) { }
    public virtual void Reset() { }
}
