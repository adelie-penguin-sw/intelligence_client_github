using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Michsky.LSS
{
    public class LoadingScreenManager : MonoBehaviour
    {
        // Settings
        public LoadingMode loadingMode;
        public string prefabHelper = "Standard";
        public bool enableTrigger;
        public bool onTriggerExit;
        public bool onlyLoadWithTag;
        public bool startLoadingAtStart;
        public string objectTag;
        public string sceneName;

        // Temp Variables
        public Object[] loadingScreens;
        public int selectedLoadingIndex = 0;
        public int selectedTagIndex = 0;

        // Events
        public UnityEvent onLoadStart;
        public List<GameObject> dontDestroyOnLoad = new List<GameObject>();

        // Additive-only
        [SerializeField] private List<string> loadedScenes = new List<string>();

        public enum LoadingMode { Single, Additive }

        void Start()
        {
            if (startLoadingAtStart == true && loadingMode == LoadingMode.Single) { LoadScene(sceneName); }
        }

        public void SetStyle(string styleName)
        {
            prefabHelper = styleName;
        }

        public void LoadScene(string sceneName)
        {
            LoadingScreen.prefabName = prefabHelper;
            LoadingScreen.LoadScene(sceneName);

            for (int i = 0; i < dontDestroyOnLoad.Count; i++)
                DontDestroyOnLoad(dontDestroyOnLoad[i]);

            onLoadStart.Invoke();
        }

        public void LoadAdditiveScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
#if UNITY_EDITOR
            loadedScenes.Add(SceneManager.GetSceneByName(sceneName).name);
#endif
        }

        public void UnloadAdditiveScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
#if UNITY_EDITOR
            loadedScenes.Remove(sceneName);
#endif
        }

        private void OnTriggerEnter(Collider other)
        {
            if (loadingMode == LoadingMode.Additive || enableTrigger == false)
                return;

            LoadingScreen.prefabName = prefabHelper;

            if (onlyLoadWithTag == true && onTriggerExit == false && other.gameObject.tag == objectTag)
            {
                onLoadStart.Invoke();
                LoadingScreen.LoadScene(sceneName);
            }

            else if (onTriggerExit == false) { onLoadStart.Invoke(); LoadingScreen.LoadScene(sceneName); }
        }

        private void OnTriggerExit(Collider other)
        {
            if (loadingMode == LoadingMode.Additive || enableTrigger == false)
                return;

            LoadingScreen.prefabName = prefabHelper;

            if (onlyLoadWithTag == true && onTriggerExit == true && other.gameObject.tag == objectTag)
            {
                LoadingScreen.LoadScene(sceneName);
                onLoadStart.Invoke();
            }

            else if (onlyLoadWithTag == false && onTriggerExit == true) { LoadingScreen.LoadScene(sceneName); onLoadStart.Invoke(); }
        }
    }
}