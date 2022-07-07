using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public void SetData(string name)
    {
        _name = name;
    }

    public void EnableObject(Transform parent)
    {
        _objLife = 3;
        _isEnable = true;

        this.gameObject.SetActive(true);
        this.transform.SetParent(parent);
    }

    public void DisableObject(Transform pool)
    {
        _isEnable = false;
        this.gameObject.SetActive(false);
        this.transform.SetParent(pool);
    }

    public bool IsDie()
    {
        if (--_objLife > 0)
        {
            return false;
        }
        return true;
    }
}
