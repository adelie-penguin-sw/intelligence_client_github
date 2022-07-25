using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpArrowNotation a = new UpArrowNotation(1f, 1f, 1f, 123456789);
        for (int i = 0; i < 9; i++)
        {
            Debug.Log(a._operatorLayerCount[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
