using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    private DateTimeOffset _dateTimeOffset;
    private long _lastCalcTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(DateTimeOffset.Now.ToUnixTimeMilliseconds());
    }
}
