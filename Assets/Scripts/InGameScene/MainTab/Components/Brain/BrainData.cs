using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    [Serializable]
    public class BrainData
    {
        public int id;
        public double intellect = 1f;
        public double standByIntellect = 0f;
        public EBrainType brainType = EBrainType.MAINBRAIN;

    }

    public struct BrainSendData
    {
        public int id;
        public Brain brain;
    }

    public enum EBrainType
    {
        MAINBRAIN,
        NORMALBRAIN,
        GUIDEBRAIN,
    }
}