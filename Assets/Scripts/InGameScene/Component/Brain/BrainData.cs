using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BrainData
{
    public double number = 0f;
    public ECellType cellType = ECellType.MainCell;
    public int id;
}

public enum ECellType
{
    MainCell,
    NormalCell,
}