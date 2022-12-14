using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace MainTab
{
    /// <summary>
    /// 메인탭의 데이터를 가지고 있는 Model 클래스
    /// </summary>
    [Serializable]
    public class MainTabModel
    {
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
                Camera.main.orthographicSize = _curCameraSize;
            }
        }

        public Vector3 CameraMoveDelta
        {
            get { return _prevMousePos - _curMousePos; }
        }

        #endregion

        [BoxGroup("Brain Network")]
        [SerializeField] private BrainNetwork _brainNetwork = new BrainNetwork();
        public BrainNetwork BrainNetwork {
            get
            {
                return _brainNetwork;
            }
            set
            {
                _brainNetwork = value;
            }
        }

        #region UserData
        //[SerializeField] private SingleNetworkWrapper _singleNetworkWrapper;
        //public SingleNetworkWrapper SingleNetworkWrapper
        //{
        //    get
        //    {
        //        return _singleNetworkWrapper;
        //    }
        //    set
        //    {
        //        _singleNetworkWrapper = value;
        //    }
        //}

        #endregion
    }
}