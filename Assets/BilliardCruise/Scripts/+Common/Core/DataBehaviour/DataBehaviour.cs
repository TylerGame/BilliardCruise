using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

public class DataBehaviour<T> : DataBehaviour
{
    public T Model => (T)model;
}

public class DataBehaviour : MonoBehaviour
{
    // Don't change this without need to avoid problems
    protected bool disposeOnDisable = true;

    protected object model;

    CompositeDisposable disposables = new CompositeDisposable();
    public void Subscribe<T>(Action<T> action) where T : IEvent
    {
        Core.Rx.Receive<T>().Subscribe(data => action.Invoke(data)).AddTo(disposables);
    }

    public virtual void OnDisable()
    {
        if (disposeOnDisable)
        {
            disposables.Dispose();
        }
    }

    public virtual void Populate(object obj)
    {
        model = obj;
        Populate();
    }

    public virtual void Populate()
    {
        // populate component with data
    }
}
