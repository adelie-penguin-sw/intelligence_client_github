using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    /// <summary>
    /// 채널의 데이터를 담는 저장용 클래스
    /// </summary>
    [Serializable]
    struct ChannelData
    {
        /// <summary>
        /// 아이디
        /// </summary>
        public int id;
        /// <summary>
        /// 보내는 Brain Class
        /// </summary>
        public Brain fromBrain;
        /// <summary>
        /// 받는 Brain Class
        /// </summary>
        public Brain toBrain;
        /// <summary>
        /// 보내는 Brain의 ID
        /// </summary>
        public int fromId;
        /// <summary>
        /// 받는 Brain의 ID
        /// </summary>
        public int toId;
    }

    /// <summary>
    /// 채널이 가지고 있는 시작 브레인과 도착 브레인을 구분하기 위한 Enum
    /// </summary>
    public enum EChannelBrainType
    {
        FROM,
        TO,
    }
}