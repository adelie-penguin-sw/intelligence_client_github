using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    UpArrowNotation i = new UpArrowNotation(1f);
    UpArrowNotation j = new UpArrowNotation(2f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(i);
        Debug.Log(Exchange.Slog10(i));
        i.Mul(j);
        j.Mul(new UpArrowNotation(2f));
    }
}
