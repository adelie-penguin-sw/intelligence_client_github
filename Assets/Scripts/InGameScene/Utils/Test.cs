using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PowerTowerNotation a = new PowerTowerNotation(300000000000000000f);
        PowerTowerNotation b = a.Copy();
        Debug.Log(a * b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
