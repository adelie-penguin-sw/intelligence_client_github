using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
[Serializable]
public class CreateFixedNetworkChannelResponse
{
    public int statusCode;
    public AnsEquation TP;
    public List<BrainAttributes> FNBrainAttributes;
    public List<Structure> FNStructures;
}
