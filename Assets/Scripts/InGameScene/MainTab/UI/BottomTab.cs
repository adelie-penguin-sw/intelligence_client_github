using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// 하단 탭 버튼에 속하는 UI class
    /// </summary>
    public class BottomTab : MonoBehaviour
    {
        [SerializeField]
        private EGameState _tab;

        public delegate void OnClickTabEvent(EGameState tab);
        public OnClickTabEvent OnClickTab;

        public void Init()
        {

        }
    }
}
