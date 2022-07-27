using UnityEngine;

namespace Michsky.LSS
{
    [AddComponentMenu("Loading Screen Studio/Additive Loading Trigger")]
    public class AdditiveLoadTrigger : MonoBehaviour
    {
        [Header("Resources")]
        public LoadingScreenManager loadingManager;

        [Header("Settings")]
        public string sceneName;
        public string objectTag = "Player";
        public EventType eventType;
        public enum EventType { Load, Unload }

        private void OnTriggerEnter(Collider other)
        {
            if (loadingManager == null) { Debug.Log("<b>[LSS Additive Loading]</b> Loading Manager is missing.", this); return; }

            if (eventType == EventType.Load && other.gameObject.tag == objectTag) { loadingManager.LoadAdditiveScene(sceneName); this.enabled = false; }
            else if (eventType == EventType.Unload && other.gameObject.tag == objectTag) { loadingManager.UnloadAdditiveScene(sceneName); this.enabled = false; }
        }
    }
}