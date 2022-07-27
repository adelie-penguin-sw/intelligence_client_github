using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BaseTabApplication이 가지고 있는 Controller의 규격을 정의하는 클래스
/// </summary>
/// <typeparam name="T">해당 Controller를 가지고 있는 Application Class</typeparam>
public class BaseTabController<T> : MonoBehaviour
{
    protected T _app;

    /// <summary>
    /// Application Init시 같이 실행
    /// </summary>
    /// <param name="app">해당 Controller를 가지고있는 Application Class</param>
    public virtual void Init(T app)
    {
        _app = app;
    }

    /// <summary>
    /// Controller가 재실행 될 때 호출되는 초기화 메서드
    /// </summary>
    public virtual void Set()
    { 
    }

    /// <summary>
    /// 유니티 생명주기의 Update역할 수행
    /// </summary>
    /// <param name="dt_sec">deltaTime</param>
    public virtual void AdvanceTime(float dt_sec)
    { 
    }

    /// <summary>
    /// 해당 controller 제거시 실행. 메모리 해제용.
    /// </summary>
    public virtual void Dispose()
    { 
    }
}
