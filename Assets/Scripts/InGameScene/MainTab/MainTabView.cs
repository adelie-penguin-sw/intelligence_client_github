using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    /// <summary>
    /// Controller에서 받은 데이터들을 화면에서 출력해주는 역할을 함
    /// </summary>
    public class MainTabView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private MainTabUI _ui;

        public MainTabUI UI
        {
            get
            {
                return _ui;
            }
        }
    }
}