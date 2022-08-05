using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreateSingleNetworkBrainResponse
{
	public int statusCode;
	public List<AnsEquations> ansEquations;
	public List<Distances> distances; 
	public int newBrain;
	public AnsEquation NP;
	public AnsEquation TP;
	public int calcTime;
}


