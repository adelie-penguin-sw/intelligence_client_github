using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct ChannelData
{
    public int id;
    public Transform trFrom;
    public Transform trTo;
    public int fromId;
    public int toId;
}

public enum EChannelBrainType
{
    FROM,
    TO,
}