using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    /// <summary>
    /// Channel Component Class / 모든 채널 오브젝트는 이 클래스를 보유
    /// </summary>
    public class Channel : MonoBehaviour
    {
        [SerializeField] private ChannelData _data;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Material _material;

        /// <summary>
        /// 지능을 보내는 brain class
        /// </summary>
        public Brain SenderBrain
        {
            get
            {
                return _data.senderBrain;
            }
        }

        /// <summary>
        /// 지능을 받는 brain class
        /// </summary>
        public Brain ReceiverBrain
        {
            get
            {
                return _data.receiverBrain;
            }
        }

        /// <summary>
        /// 해당 채널 오브젝트 생성시 최초 1회 실행되어야 한다.<br />
        /// fromBrain, toBrain이 반드시 Init을 하며 초기화 될 필요는 없음.<br />
        /// 허나 추후 Set()을 통해 설정해주어야 한다.<br />
        /// </summary>
        /// <param name="senderBrain">지능을 보내는 BrainData</param>
        /// <param name="receiverBrain">지능을 받는 BrainData</param>
        public void Init(BrainSendData senderBrain, BrainSendData receiverBrain)
        {
            //_material.SetFloat("width", 0.5f);
            // _material.SetFloat("heigth", 0.5f);
            Set(senderBrain, receiverBrain);
        }

        /// <summary>
        /// 채널 오브젝트 설정시 사용.<br />
        /// 초기화/재설정용 으로 사용가능.<br />
        /// </summary>
        /// <param name="senderBrain">지능을 보내는 BrainData</param>
        /// <param name="receiverBrain">지능을 받는 BrainData</param>
        public void Set(BrainSendData senderBrain, BrainSendData receiverBrain)
        {
            SetBrainInfo(senderBrain, EChannelBrainType.SENDER);
            SetBrainInfo(receiverBrain, EChannelBrainType.RECEIVER);
        }


        /// <summary>
        /// Unity 기본 생명주기 Update를 대체해주는 함수<br />
        /// 지속 실행 시켜주어야한다.<br />
        /// </summary>
        /// <param name="dt_sec">deltaTime</param>
        public void AdvanceTime(float dt_sec)
        {
            if (_data.senderBrain != null)
            {
                _lineRenderer.SetPosition(0, _data.senderBrain.transform.position);
            }
            if (_data.receiverBrain != null)
            {
                _lineRenderer.SetPosition(1, _data.receiverBrain.transform.position);
            }
        }


        /// <summary>
        /// 해당 오브젝트 Despawn시 실행시켜주어야 한다.
        /// </summary>
        public void Dispose()
        {
            PoolManager.Instance.DespawnObject(EPrefabsType.CHANNEL, this.gameObject);
        }

        /// <summary>
        /// 그려지는 Line의 도착부분의 좌표를 설정해주는 메서드
        /// </summary>
        /// <param name="toPos">도착 좌표</param>
        public void SetLineRenderToPos(Vector2 toPos)
        {
            _lineRenderer.SetPosition(1, toPos);
        }

        private void SetBrainInfo(BrainSendData data, EChannelBrainType type)
        {
            switch (type)
            {
                case EChannelBrainType.SENDER:
                    _data.senderBrain = data.brain;
                    _data.senderId = data.id;
                    break;
                case EChannelBrainType.RECEIVER:
                    _data.receiverBrain = data.brain;
                    _data.receiverId = data.id;
                    break;
            }
        }
    }
}