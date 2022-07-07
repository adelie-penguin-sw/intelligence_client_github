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
            // IDisposable �������̽��� �����ϴ� ������� ���⼭ �����մϴ�.
        }
        // .NET Framework�� ���Ͽ� �������� �ʴ� �ܺ� ���ҽ����� ���⼭ �����մϴ�.
        this.disposed = true;
    }
}
