using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreateSingleNetworkBrainResponse
{
	public int statusCode;
	public List<AnsEquations> ansEquations;
	public List<Distances> distances; 
	public long newBrain;
	public TopCoeffs NP;
	public TopCoeffs TP;
	public long calcTime;
}


