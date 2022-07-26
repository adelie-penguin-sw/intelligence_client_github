using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpArrowNotation a = new UpArrowNotation(3.14f, 2f, 2f, 531123112);
        Debug.Log(a);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
