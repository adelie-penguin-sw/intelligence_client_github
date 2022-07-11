using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTabController<T> : MonoBehaviour
{
    protected T _app;

    public virtual void Init(T app)
    {
        _app = app;
    }

    public virtual void Set()
    { 
    }

    public virtual void AdvanceTime(float dt_sec)
    { 
    }

    public virtual void Dispose()
    { 
    }
}
