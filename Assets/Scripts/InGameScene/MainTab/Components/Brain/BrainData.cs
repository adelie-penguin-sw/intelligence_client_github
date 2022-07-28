using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
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
        public UpArrowNotation intellect = new UpArrowNotation(1f);
        /// <summary>
        /// 다음 tick에 증가 될 지능
        /// </summary>
        public UpArrowNotation standByIntellect = new UpArrowNotation();
        /// <summary>
        /// 거리
        /// </summary>
        public int distance = -1;
        /// <summary>
        /// 현재 브레인 타입
        /// </summary>
        public EBrainType brainType = EBrainType.MAINBRAIN;

        [ShowInInspector] public HashSet<int> _receiverIdList;
        [ShowInInspector] public HashSet<int> _senderIdList;
        public BrainData()
        {
            _receiverIdList = new HashSet<int>();
            _senderIdList = new HashSet<int>();
        }

        public BrainData(int id, EBrainType brainType)
        {
            this.id = id;
            this.intellect = new UpArrowNotation(1);
            this.brainType = brainType;
            this.standByIntellect = new UpArrowNotation();
            _receiverIdList = new HashSet<int>();
            _senderIdList = new HashSet<int>();
        }
        public BrainData(int id, UpArrowNotation intellect, EBrainType brainType)
        {
            this.id = id;
            this.intellect = intellect;
            this.brainType = brainType;
            this.standByIntellect = new UpArrowNotation();
            _receiverIdList = new HashSet<int>();
            _senderIdList = new HashSet<int>();
        }
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