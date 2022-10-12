using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GetFixedNetworkResponse
{
    public int statusCode;
    public List<BrainAttributes> FNBrainAttributes;
    public List<Structure> FNStructures;
}
