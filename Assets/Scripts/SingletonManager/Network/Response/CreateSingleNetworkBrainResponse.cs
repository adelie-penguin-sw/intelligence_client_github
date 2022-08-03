using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreateSingleNetworkBrainResponse
{
	public int statusCode;
	public List<AnsEquations> ansEquations;
	public List<Distances> distance;
	public int newBrain;
	public double NP;
	public int calcTime;
}


