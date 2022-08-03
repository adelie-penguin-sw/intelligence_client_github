using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreateSingleNetworkChannelResponse
{
	public int statusCode;
	public List<AnsEquations> ansEquations;
	public List<Distances> distances;
	public double NP;
	public int calcTime;
}
