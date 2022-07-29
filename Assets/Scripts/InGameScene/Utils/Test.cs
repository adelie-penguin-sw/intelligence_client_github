using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PowerTowerNotation a = new PowerTowerNotation(2);
        PowerTowerNotation b = new PowerTowerNotation(1000);
        Debug.Log(a >= b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
