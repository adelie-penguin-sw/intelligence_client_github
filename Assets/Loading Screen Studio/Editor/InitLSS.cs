#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.LSS
{
    public class InitLSS
    {
        [InitializeOnLoad]
        public class InitOnLoad
        {
            static InitOnLoad()
            {
                if (!EditorPrefs.HasKey("LSS.Installed"))
                {
                    EditorPrefs.SetInt("LSS.Installed", 1);
                    EditorUtility.DisplayDialog("Hello there!", "Thank you for purchasing Loading Screen Studio.\r\r" +
                                                "If you need help, feel free to contact us through our support channels or Discord.", "Got it!");
                }

                PlayerPrefs.SetString("LSS_SelectedLS", "Standard");
            }
        }
    }
}
#endif