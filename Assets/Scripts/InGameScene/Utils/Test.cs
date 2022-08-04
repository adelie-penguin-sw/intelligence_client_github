using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    UpArrowNotation a = new UpArrowNotation(1f);
    UpArrowNotation b = new UpArrowNotation(1823748924352273f);
    UpArrowNotation c = new UpArrowNotation(1128925342354912f);
    UpArrowNotation d = new UpArrowNotation(112423523534789127f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        c.Mul(d);
        b.Mul(c);
        a.Mul(b);
        Debug.Log(a._operatorLayerCount);
        Debug.Log(a);
    }
}
