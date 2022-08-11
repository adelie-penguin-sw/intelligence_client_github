using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreateSingleNetworkBrainNumberResponse
{
    public int statusCode;
    public List<AnsEquations> ansEquations;
    public List<Distances> distances;
    public TopCoeffs NP;
    public long calcTime;
}
