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
        public long id;
        /// <summary>
        /// 지능
        /// </summary>
        public List<UpArrowNotation> intellect;
        /// <summary>
        /// 거리
        /// </summary>
        public long distance = -1;
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
        public long skinCode;
        /// <summary>
        /// 업그레이드 상태
        /// </summary>
        public long UpgradeCondition;

        [ShowInInspector] public HashSet<long> _receiverIdList;
        [ShowInInspector] public HashSet<long> _senderIdList;
        public BrainData()
        {
            _receiverIdList = new HashSet<long>();
            _senderIdList = new HashSet<long>();
        }

        public BrainData(int id, EBrainType brainType)
        {
            this.id = id;
            this.intellect = new List<UpArrowNotation> { new UpArrowNotation(1)};
            this.brainType = brainType;
            _receiverIdList = new HashSet<long>();
            _senderIdList = new HashSet<long>();
        }
        public BrainData(int id, List<UpArrowNotation> intellect, int distance, EBrainType brainType)
        {
            this.id = id;
            this.intellect = intellect;
            this.brainType = brainType;
            this.distance = distance;
            _receiverIdList = new HashSet<long>();
            _senderIdList = new HashSet<long>();
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