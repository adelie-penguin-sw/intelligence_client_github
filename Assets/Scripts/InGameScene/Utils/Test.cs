using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PowerTowerNotation a = new PowerTowerNotation(3141592653589793238f);
        Debug.Log(a);
        Debug.Log(a.Layer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
