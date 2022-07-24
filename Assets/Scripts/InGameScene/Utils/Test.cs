using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PowerTowerNotation a = new PowerTowerNotation(5300000000f);
        Debug.Log(a ^ 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
