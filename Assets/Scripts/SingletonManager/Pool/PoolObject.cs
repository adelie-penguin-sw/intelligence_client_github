using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Pooling으로 생성된 오브젝트에 강제로 부여되는 Class
/// </summary>
public class PoolObject : MonoBehaviour
{
    private int _objLife;
    public int ObjLife
    {
        get
        {
            return _objLife;
        }
    }

    private string _name;
    public string Name
    {
        get
        {
            return _name;
        }
    }
    private bool _isEnable;
    private Transform _poolLayer;

    /// <summary>
    /// 해당 프리팹의 이름을 변수 데이터에 저장한다. 
    /// </summary>
    /// <param name="name">이름</param>
    public void SetData(string name)
    {
        _name = name;
    }

    /// <summary>
    /// 오브젝트 활성화 <br />
    /// 풀 레이어에서 활성화 시킬 레이어로 옮겨주고 오브젝트를 활성화 시킨다.
    /// </summary>
    /// <param name="parent">오브젝트가 활성화 될 부모</param>
    public void EnableObject(Transform parent)
    {
        _objLife = 3;
        _isEnable = true;

        this.gameObject.SetActive(true);
        this.transform.SetParent(parent);
    }

    /// <summary>
    /// 오브젝트 비활성화<br />
    /// 오브젝트의 부모를 pool 레이어로 돌려주고 오브젝트를 비활성화 시킨다.
    /// </summary>
    /// <param name="pool">기존 pool의 레이어</param>
    public void DisableObject(Transform pool)
    {
        _isEnable = false;
        this.gameObject.SetActive(false);
        this.transform.SetParent(pool);
    }

    /// <summary>
    /// 오브젝트의 Destroy 여부 계산<br />
    /// </summary>
    /// <returns>object의 Life가 0이 되면 true를 반환 아니면 false</returns>
    public bool IsDie()
    {
        if (--_objLife > 0)
        {
            return false;
        }
        return true;
    }
}
