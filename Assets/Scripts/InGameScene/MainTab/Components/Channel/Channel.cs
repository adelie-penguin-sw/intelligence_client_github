using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    public class Channel : MonoBehaviour
    {
        [SerializeField]
        private ChannelData _data;
        [SerializeField]
        private LineRenderer _lineRenderer;
        [SerializeField]
        private Material _material;

        public Brain FromBrain
        {
            get
            {
                return _data.fromBrain;
            }
        }
        public Brain ToBrain
        {
            get
            {
                return _data.toBrain;
            }
        }
        public void Init(BrainSendData fromBrain, BrainSendData toBrain)
        {
            //_material.SetFloat("width", 0.5f);
            // _material.SetFloat("heigth", 0.5f);
            Set(fromBrain, toBrain);
        }

        public void Set(BrainSendData fromBrain, BrainSendData toBrain)
        {
            SetBrainInfo(fromBrain, EChannelBrainType.FROM);
            SetBrainInfo(toBrain, EChannelBrainType.TO);
        }

        public void AdvanceTime(float dt_sec)
        {
            if (_data.fromBrain != null)
            {
                _lineRenderer.SetPosition(0, _data.fromBrain.transform.position);
            }
            if (_data.toBrain != null)
            {
                _lineRenderer.SetPosition(1, _data.toBrain.transform.position);
            }
        }

        public void Dispose()
        {
        }

        public void SetLineRenderToPos(Vector2 toPos)
        {
            _lineRenderer.SetPosition(1, toPos);
        }

        private void SetBrainInfo(BrainSendData data, EChannelBrainType type)
        {
            switch (type)
            {
                case EChannelBrainType.FROM:
                    _data.fromBrain = data.brain;
                    _data.fromId = data.id;
                    break;
                case EChannelBrainType.TO:
                    _data.toBrain = data.brain;
                    _data.toId = data.id;
                    break;
            }
        }
    }
}