using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreateSingleNetworkBrainResponse
{
	public int statusCode;
    public List<AnsEquations> ansEquations;
    public List<Multiplier> multipliers;
    public List<Distances> distances; 
	public long newBrain;
	public AnsEquation NP;
	public long calcTime;
}


