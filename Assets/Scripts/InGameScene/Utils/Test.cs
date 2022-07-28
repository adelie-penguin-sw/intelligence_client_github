using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    UpArrowNotation a = new UpArrowNotation(1);
    UpArrowNotation b = new UpArrowNotation(516883629);
    UpArrowNotation c = new UpArrowNotation(4618929);
    UpArrowNotation d = new UpArrowNotation(4982648723);

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

        Debug.Log(a);
    }
}
