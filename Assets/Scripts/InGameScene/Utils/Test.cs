using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    List<UpArrowNotation> brainEqCoeffList = new List<UpArrowNotation>();
    int elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        brainEqCoeffList.Add(new UpArrowNotation(16288));
        brainEqCoeffList.Add(new UpArrowNotation(416));
        brainEqCoeffList.Add(new UpArrowNotation(7432));
        brainEqCoeffList.Add(new UpArrowNotation(96809));
        brainEqCoeffList.Add(new UpArrowNotation(14));
        brainEqCoeffList.Add(new UpArrowNotation(6));
        brainEqCoeffList.Add(new UpArrowNotation(3));
        brainEqCoeffList.Add(new UpArrowNotation(1));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Equation.GetCurrentIntellect(brainEqCoeffList, elapsedTime));
        elapsedTime += 1;
    }
}
