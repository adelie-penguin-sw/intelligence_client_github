using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    /// <summary>
    /// 브레인의 데이터를 담는 저장용 클래스
    /// </summary>
    [Serializable]
    public class BrainData
    {
        /// <summary>
        /// 아이디
        /// </summary>
        public int id;
        /// <summary>
        /// 지능
        /// </summary>
        public double intellect = 1f;
        /// <summary>
        /// 다음 tick에 증가 될 지능
        /// </summary>
        public double standByIntellect = 0f;
        /// <summary>
        /// 현재 브레인 타입
        /// </summary>
        public EBrainType brainType = EBrainType.MAINBRAIN;

    }

    /// <summary>
    /// 브레인의 정보를 전달할때 사용하는 저장용 구조체 
    /// </summary>
    public struct BrainSendData
    {
        /// <summary>
        /// 아이디 
        /// </summary>
        public int id;
        /// <summary>
        /// 브레인 클래스
        /// </summary>
        public Brain brain;
    }

    /// <summary>
    /// 브레인 타입 Enum
    /// </summary>
    public enum EBrainType
    {
        MAINBRAIN,
        NORMALBRAIN,
        GUIDEBRAIN,
    }
}