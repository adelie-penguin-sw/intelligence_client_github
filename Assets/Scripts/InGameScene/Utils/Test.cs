using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    UpArrowNotation a = new UpArrowNotation(1);
    UpArrowNotation b = new UpArrowNotation(18237489273);
    UpArrowNotation c = new UpArrowNotation(112894912);
    UpArrowNotation d = new UpArrowNotation(1124789127);

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
