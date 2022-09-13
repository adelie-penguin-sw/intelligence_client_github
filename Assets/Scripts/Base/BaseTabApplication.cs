using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


/// <summary>
/// Model, View, Controller로 역할에 따라 나누어 가지고 있다. <br />
/// 인게임의 탭은 BaseTabApplication을 상속받아야 한다.<br />
/// </summary>
public class BaseTabApplication : MonoBehaviour, IGameStateBasicModule, IDisposable
{
    /// <summary>
    /// InGameManager에서 handler 최초 한번 실행
    /// </summary>
    public virtual void Init()
    {
        gameObject.SetActive(false);
    }


    /// <summary>
    /// InGameManager에서 해당 핸들러로 ChangeState하는 경우 실행
    /// </summary>
    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 유니티 생명주기에서 Update와 같은 역할,<br />
    /// 해당 핸들러가 주 핸들러일 경우 계속 실행<br />
    /// </summary>
    /// <param name="dt_sec">deltaTime</param>
    public virtual void AdvanceTime(float dt_sec)
    {

    }

    /// <summary>
    /// 유니티 생명주기에서 LateUpdate와 같은 역할
    /// </summary>
    /// <param name="dt_sec"></param>
    public virtual void LateAdvanceTime(float dt_sec)
    {

    }

    /// <summary>
    /// InGameManager에서 해당 핸들러가 다른 핸들러로 변경되는 경우 실행 
    /// </summary>
    public virtual void OnExit()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Scene이 변경되는 경우 실행<br />
    /// Dispose에서 메모리를 해제 시켜야 함<br />
    /// </summary>
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
            // IDisposable ???????????? ???????? ???????? ?????? ??????????.
        }
        // .NET Framework?? ?????? ???????? ???? ???? ?????????? ?????? ??????????.
        this.disposed = true;
    }
}
