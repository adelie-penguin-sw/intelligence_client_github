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
        [ShowInInspector] public List<UpArrowNotation> intellectEquation;
        /// <summary>
        /// 브레인 업그레이드로 얻은 지능 증폭계수
        /// </summary>        
        [ShowInInspector] public UpArrowNotation upgradedMultiplier;
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

        private long _lastCalcTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        public long LastCalcTime
        {
            get
            {
                return _lastCalcTime;
            }
            set
            {
                UserData.LastCalcTime = value;
                _lastCalcTime = value;
            }
        }
        /// <summary>
        /// 지능 수치 계산하여 반환
        /// </summary>
        public UpArrowNotation Intellect
        {
            get
            {
                double elapsedTime = (double)(DateTimeOffset.Now.ToUnixTimeMilliseconds() - LastCalcTime) / 1000f;
                UpArrowNotation intellect = Equation.GetCurrentIntellect(intellectEquation, elapsedTime);

                //if (id == 0)
                //{
                //    UserData.CoreIntellect = intellect;
                //}
                return intellect;
            }
        }

        [ShowInInspector] public HashSet<long> receiverIds;
        [ShowInInspector] public HashSet<long> senderIds;
        [ShowInInspector] public HashSet<long> deletableSenderIds;
        public BrainData()
        {
            receiverIds = new HashSet<long>();
            senderIds = new HashSet<long>();
            deletableSenderIds = new HashSet<long>();
            this.intellectEquation = new List<UpArrowNotation>();
            this.upgradedMultiplier = new UpArrowNotation(1);
        }

        public BrainData(int id, EBrainType brainType)
        {
            this.id = id;
            this.intellectEquation = new List<UpArrowNotation> { new UpArrowNotation(1)};
            this.upgradedMultiplier = new UpArrowNotation(1);
            this.brainType = brainType;
            receiverIds = new HashSet<long>();
            senderIds = new HashSet<long>();
            deletableSenderIds = new HashSet<long>();
        }
        public BrainData(int id, List<UpArrowNotation> intellect, UpArrowNotation multiplier, int distance, EBrainType brainType)
        {
            this.id = id;
            this.intellectEquation = intellect;
            this.upgradedMultiplier = multiplier;
            this.brainType = brainType;
            this.distance = distance;
            receiverIds = new HashSet<long>();
            senderIds = new HashSet<long>();
            deletableSenderIds = new HashSet<long>();
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
