using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MainTab
{
    [Serializable]
    public class MainTabModel
    {
        #region Camera
        [Header("*Camera*")]
        [SerializeField]
        private Vector3 _curMousePos;
        public Vector3 CurMousePos
        {
            get
            {
                return _curMousePos;
            }
            set
            {
                _curMousePos = value;
            }
        }

        [SerializeField]
        private Vector3 _prevMousePos;
        public Vector3 PrevMousePos
        {
            get
            {
                return _prevMousePos;
            }
            set
            {
                _prevMousePos = value;
            }
        }

        public Vector3 CameraMoveDelta
        {
            get
            {
                return _prevMousePos - _curMousePos;
            }
        }

        [SerializeField]
        private float _dragSpeed = 0.003f;
        public float DragSpeed
        {
            get
            {
                return _dragSpeed;
            }
        }

        [SerializeField]
        private Camera _mainCamera;
        public Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                    _mainCamera = Camera.main;
                return _mainCamera;
            }
        }

        [SerializeField]
        private float _minSize = 1;
        [SerializeField]
        private float _maxSize = 20;
        [SerializeField]
        private float _resizeSpeedScale = 2;
        [SerializeField]
        private float _curCameraSize = 5;
        public float CurCameraSize
        {
            get
            {
                return _curCameraSize;
            }
            set
            {
                _curCameraSize *= Mathf.Pow(_resizeSpeedScale, -value);
                _curCameraSize = Math.Min(Math.Max(_curCameraSize, _minSize), _maxSize);
                Camera.main.orthographicSize = _curCameraSize;
            }
        }
        #endregion

        #region Behavior
        [Header("*Behavior*")]
        [SerializeField]
        private float _waitBrainClickTime = 1f;
        public float WaitBrainClickTime
        {
            get
            {
                return _waitBrainClickTime;
            }
        }
        #endregion
    }
}