using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    [Serializable]
    public class BrainData
    {
        public double intellect = 1f;
        public EBrainType brainType = EBrainType.MAINBRAIN;
        public int id;
    }

    public struct BrainSendData
    {
        public int id;
        public Transform tr;
    }

    public enum EBrainType
    {
        MAINBRAIN,
        NORMALBRAIN,
        GUIDEBRAIN,
    }
}