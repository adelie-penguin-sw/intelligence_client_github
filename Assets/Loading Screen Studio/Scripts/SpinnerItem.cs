using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.LSS
{
    [ExecuteInEditMode]
    public class SpinnerItem : MonoBehaviour
    {
        [Header("Settings")]
        public LoadingScreen loadingScreen;

        [Header("Resources")]
        public List<Image> foreground = new List<Image>();
        public List<Image> background = new List<Image>();

        void OnEnable()
        {
            if (loadingScreen == null)
            {
                try { loadingScreen = gameObject.GetComponentInParent<LoadingScreen>(); }
                catch { Debug.Log("No Loading Screen found. Assign it manually, otherwise you'll get errors about it.", this); }
            }
        }

        public void UpdateValues()
        {
            for (int i = 0; i < foreground.Count; ++i)
            {
                Image currentImage = foreground[i];
                currentImage.color = loadingScreen.spinnerColor;
            }

            for (int i = 0; i < background.Count; ++i)
            {
                Image currentImage = background[i];
                currentImage.color = new Color(loadingScreen.spinnerColor.r, loadingScreen.spinnerColor.g, loadingScreen.spinnerColor.b, 0.08f);
            }
        }
    }
}