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

        public void Init(BrainSendData fromBrain, BrainSendData toBrain)
        {
            _material.SetFloat("width", 0.5f);
            _material.SetFloat("heigth", 0.5f);
            SetBrainInfo(fromBrain, EChannelBrainType.FROM);
            SetBrainInfo(toBrain, EChannelBrainType.TO);
        }

        //public void Set(BrainSendData fromBrain, BrainSendData toBrain)
        //{
        //    SetBrainInfo(fromBrain, EChannelBrainType.FROM);
        //    SetBrainInfo(toBrain, EChannelBrainType.TO);
        //}

        public void AdvanceTime(float dt_sec)
        {
            _lineRenderer.SetPosition(0, _data.trFrom.position);
            _lineRenderer.SetPosition(1, _data.trTo.position);
        }

        public void Dispose()
        {
        }

        private void SetBrainInfo(BrainSendData data, EChannelBrainType type)
        {
            switch (type)
            {
                case EChannelBrainType.FROM:
                    _data.trFrom = data.tr;
                    _data.fromId = data.id;
                    break;
                case EChannelBrainType.TO:
                    _data.trTo = data.tr;
                    _data.toId = data.id;
                    break;
            }
        }
    }
}