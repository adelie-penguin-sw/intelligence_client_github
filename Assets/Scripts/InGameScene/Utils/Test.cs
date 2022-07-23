using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PowerTowerNotation a = new PowerTowerNotation(50000000000);

        Debug.Log(a - 800000000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
