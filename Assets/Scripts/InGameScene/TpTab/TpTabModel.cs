using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace TpTab
{
    /// <summary>
    /// TP 업그레이드 탭의 데이터를 가지고 있는 Model 클래스
    /// </summary>
    [Serializable]
    public class TpTabModel
    {
        public Dictionary<int,TpUpgradeDefinition> TpUpgradeDefinition
        {
            get
            {
                return Managers.Definition.GetDatas<Dictionary<int, TpUpgradeDefinition>>(EDefType.TP_UPGRADE);
            }
        }

        #region Camera
        [BoxGroup("Camera")]
        [SerializeField] private Vector3 _curMousePos;
        public Vector3 CurMousePos { get { return _curMousePos; } set { _curMousePos = value; } }

        [BoxGroup("Camera")]
        [SerializeField] private Vector3 _prevMousePos;
        public Vector3 PrevMousePos { get { return _prevMousePos; } set { _prevMousePos = value; } }

        [BoxGroup("Camera")]
        [SerializeField] private float _dragSpeed = 0.003f;
        public float DragSpeed { get { return _dragSpeed; } }

        [BoxGroup("Camera")]
        [SerializeField] private Camera _mainCamera;
        public Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                    _mainCamera = Camera.main;
                return _mainCamera;
            }
        }

        [BoxGroup("Camera")]
        [SerializeField] private float _minSize = 4;

        [BoxGroup("Camera")]
        [SerializeField] private float _maxSize = 20;

        [BoxGroup("Camera")]
        [SerializeField] private float _resizeSpeedScale = 2;

        [BoxGroup("Camera")]
        [SerializeField] private float _curCameraSize = 10;
        public float CurCameraSize
        {
            get { return _curCameraSize; }
            set
            {
                _curCameraSize *= Mathf.Pow(_resizeSpeedScale, -value);
                _curCameraSize = Math.Min(Math.Max(_curCameraSize, _minSize), _maxSize);
                MainCamera.orthographicSize = _curCameraSize;
            }
        }

        public Vector3 CameraMoveDelta
        {
            get { return _prevMousePos - _curMousePos; }
        }

        #endregion
    }
}
