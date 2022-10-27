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
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Material _material;

        private EChannelType _type;
        private Transform _trSender;
        private Transform _trReceiver;

        /// <summary>
        /// 해당 채널 오브젝트 생성시 최초 1회 실행되어야 한다.<br />
        /// sender와 receiver가 반드시 Init을 하며 초기화 될 필요는 없음.<br />
        /// 허나 추후 Set()을 통해 설정해주어야 한다.<br />
        /// </summary>
        /// <param name="type">채널 타입</param>
        /// <param name="sender">senderBrain Transform</param>
        public void Init(EChannelType type, Transform sender)
        {
            _type = type;
            Set(sender, null);
        }

        /// <summary>
        /// 해당 채널 오브젝트 생성시 최초 1회 실행되어야 한다.<br />
        /// </summary>
        /// <param name="type">채널 타입</param>
        /// <param name="sender">senderBrain Transform</param>
        /// <param name="receiver">receiverBrain Transform</param>
        public void Init(EChannelType type, Transform sender, Transform receiver)
        {
            _type = type;
            Set(sender, receiver);
        }

        /// <summary>
        /// sender와 receiver Transform 모두 초기화시 사용한다.
        /// </summary>
        /// <param name="sender">sender Brain</param>
        /// <param name="receiver">receiver Brain</param>
        public void Set(Transform sender, Transform receiver)
        {
            if (sender != null)
            {
                _trSender = sender;
                _lineRenderer.SetPosition(0, (Vector2)_trSender.position);
            }

            if (receiver != null)
            {
                _trReceiver = receiver;
                _lineRenderer.SetPosition(1, (Vector2)_trReceiver.position);
            }
        }

        /// <summary>
        /// 이미 sender를 Init에서 설정해준 경우 다음 Set을 사용한다.
        /// </summary>
        /// <param name="receiver">receiver Transform</param>
        public void Set(Transform receiver)
        {
            Set(null, receiver);
        }

        /// <summary>
        /// Unity 기본 생명주기 Update를 대체해주는 함수<br />
        /// 지속 실행 시켜주어야한다.<br />
        /// </summary>
        /// <param name="dt_sec">deltaTime</param>
        public void AdvanceTime(float dt_sec)
        {
            if (_type != EChannelType.UNKNOWN)
            {
                if (_trSender != null)
                {
                    _lineRenderer.SetPosition(0, (Vector2)_trSender.position);
                }

                if (_trReceiver != null)
                {
                    _lineRenderer.SetPosition(1, (Vector2)_trReceiver.position);
                }
            }
        }

        /// <summary>
        /// 해당 오브젝트 Despawn시 실행시켜주어야 한다.
        /// </summary>
        public void Dispose()
        {
            _trSender = null;
            _trReceiver = null;
            _type = EChannelType.UNKNOWN;
            Managers.Pool.DespawnObject(EPrefabsType.CHANNEL, this.gameObject);
        }

        /// <summary>
        /// 그려지는 Line의 도착부분의 좌표를 설정해주는 메서드<br />
        /// 채널 타입이 TEMP일 때 작동한다.<br />
        /// </summary>
        /// <param name="toPos">도착 좌표</param>
        public void SetLineRenderToPos(Vector2 toPos)
        {
            if (_type == EChannelType.TEMP)
            {
                _lineRenderer.SetPosition(1, toPos);
            }
        }
    }

    public enum EChannelType
    {
        UNKNOWN,
        TEMP,
        NORMAL,
    }
}