using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PowerTowerNotation a = new PowerTowerNotation(2237194820940239f);
        PowerTowerNotation b = new PowerTowerNotation(40f);
        Debug.Log(a ^ b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
