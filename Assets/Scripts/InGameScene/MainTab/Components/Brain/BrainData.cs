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
        /// 거리
        /// </summary>
        public int distance = -1;
        /// <summary>
        /// 현재 브레인 타입
        /// </summary>
        public EBrainType brainType = EBrainType.MAINBRAIN;
        /// <summary>
        /// 브레인 좌표
        /// </summary>
        public Vector2 coordinates;
        /// <summary>
        /// 브레인 스킨 코드
        /// </summary>
        public int skinCode;
        /// <summary>
        /// 업그레이드 상태
        /// </summary>
        public int UpgradeCondition;

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
            _receiverIdList = new HashSet<int>();
            _senderIdList = new HashSet<int>();
        }
        public BrainData(int id, UpArrowNotation intellect, int distance, EBrainType brainType)
        {
            this.id = id;
            this.intellect = intellect;
            this.brainType = brainType;
            this.distance = distance;
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