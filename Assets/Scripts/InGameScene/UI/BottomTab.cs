using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{ 
    public class BottomTab : MonoBehaviour
    {
        [SerializeField]
        private EInGameTab _tab;

        public delegate void OnClickTabEvent(EInGameTab tab);
        public OnClickTabEvent OnClickTab;

        public void Init()
        {

        }
    }
}
