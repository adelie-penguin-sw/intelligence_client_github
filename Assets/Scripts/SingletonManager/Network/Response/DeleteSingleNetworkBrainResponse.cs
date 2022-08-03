using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeleteSingleNetworkBrainResponse
{
	public int statusCode;
	public List<AnsEquations> ansEquations;
	public List<Distances> distance;
	public List<int> deletedBrains;
	public double NP;
	public int calcTime;
}
