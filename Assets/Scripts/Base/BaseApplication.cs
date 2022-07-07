using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BaseApplication : MonoBehaviour, IGameBasicModule, IDisposable
{
    public virtual void Init()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
    }

    public virtual void AdvanceTime(float dt_sec)
    {

    }

    public virtual void OnExit()
    {
        gameObject.SetActive(false);
    }

    public virtual void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private bool disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed) return;
        if (disposing)
        {
            // IDisposable 인터페이스를 구현하는 멤버들을 여기서 정리합니다.
        }
        // .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
        this.disposed = true;
    }
}
