using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreateSingleNetworkChannelResponse
{
	public int statusCode;
	public List<AnsEquations> ansEquations;
	public List<Distances> distance;
	public double NP;
	public int calcTime;
}
