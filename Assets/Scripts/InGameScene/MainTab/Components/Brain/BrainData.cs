using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    [Serializable]
    public class BrainData
    {
        public double intellect = 0f;
        public ECellType cellType = ECellType.MainCell;
        public int id;
    }

    public struct BrainSendData
    {
        public int id;
        public Transform tr;
    }

    public enum ECellType
    {
        MainCell,
        NormalCell,
    }
}