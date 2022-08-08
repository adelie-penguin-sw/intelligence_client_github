#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Michsky.LSS
{
    [CustomEditor(typeof(LoadingScreenManager))]
    [System.Serializable]
    public class LoadingScreenManagerEditor : Editor
    {
        private LoadingScreenManager lsmTarget;
        private GUISkin customSkin;
        private int currentTab;
        private string mainScene;
        List<string> lsList = new List<string>();

        private void OnEnable()
        {
            lsmTarget = (LoadingScreenManager)target;
            lsmTarget.loadingScreens = Resources.LoadAll("Loading Screens", typeof(GameObject));

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\LSS Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\LSS Skin Light"); }

            foreach (var t in lsmTarget.loadingScreens) { lsList.Add(t.name); }
        }

        public override void OnInspectorGUI()
        {
            LSSEditorHandler.DrawComponentHeader(customSkin, "LSM Top Header");

            GUIContent[] toolbarTabs = new GUIContent[1];
            toolbarTabs[0] = new GUIContent("Settings");

            currentTab = LSSEditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 0;

            GUILayout.EndHorizontal();

            var managerTag = serializedObject.FindProperty("managerTag");
            var loadingMode = serializedObject.FindProperty("loadingMode");
            var enableTrigger = serializedObject.FindProperty("enableTrigger");
            var onTriggerExit = serializedObject.FindProperty("onTriggerExit");
            var onlyLoadWithTag = serializedObject.FindProperty("onlyLoadWithTag");
            var dontDestroyOnLoad = serializedObject.FindProperty("dontDestroyOnLoad");
            var startLoadingAtStart = serializedObject.FindProperty("startLoadingAtStart");
            var sceneName = serializedObject.FindProperty("sceneName");
            var objectTag = serializedObject.FindProperty("objectTag");
            var prefabHelper = serializedObject.FindProperty("prefabHelper");
            var selectedLoadingIndex = serializedObject.FindProperty("selectedLoadingIndex");
            var selectedTagIndex = serializedObject.FindProperty("selectedTagIndex");
            var onLoadStart = serializedObject.FindProperty("onLoadStart");
            var loadedScenes = serializedObject.FindProperty("loadedScenes");

            LSSEditorHandler.DrawHeader(customSkin, "Resources Header", 6);
            LSSEditorHandler.DrawProperty(loadingMode, customSkin, "Loading Mode");

            if (lsmTarget.loadingMode == LoadingScreenManager.LoadingMode.Additive)
            {
                GUI.enabled = false;
                mainScene = SceneManager.GetActiveScene().name;
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUILayout.LabelField(new GUIContent("Main Scene"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.TextField(mainScene);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(loadedScenes, new GUIContent("Loaded Scenes"), true);
                loadedScenes.isExpanded = true;
                GUILayout.EndHorizontal();
                GUI.enabled = true;
            }

            else
            {
                if (lsList.Count == 1 || lsList.Count >= 1)
                {
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUILayout.LabelField(new GUIContent("Selected Screen"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    selectedLoadingIndex.intValue = EditorGUILayout.Popup(selectedLoadingIndex.intValue, lsList.ToArray());
                    prefabHelper.stringValue = lsmTarget.loadingScreens.GetValue(selectedLoadingIndex.intValue).ToString().Replace(" (UnityEngine.GameObject)", "").Trim();
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Show Selected Screen", customSkin.button))
                        Selection.activeObject = Resources.Load("Loading Screens/" + lsList[lsmTarget.selectedLoadingIndex]);

                    LSSEditorHandler.DrawHeader(customSkin, "Settings Header", 10);
                    startLoadingAtStart.boolValue = LSSEditorHandler.DrawToggle(startLoadingAtStart.boolValue, customSkin, "Start Loading At Start");

                    if (startLoadingAtStart.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        EditorGUILayout.LabelField(new GUIContent("Load Scene"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        sceneName.stringValue = EditorGUILayout.TextField(sceneName.stringValue);
                        GUILayout.EndHorizontal();
                    }

                    enableTrigger.boolValue = LSSEditorHandler.DrawToggle(enableTrigger.boolValue, customSkin, "Load With Trigger");

                    if (enableTrigger.boolValue == true)
                    {
                        onTriggerExit.boolValue = LSSEditorHandler.DrawToggle(onTriggerExit.boolValue, customSkin, "On Trigger Exit");
                        onlyLoadWithTag.boolValue = LSSEditorHandler.DrawToggle(onlyLoadWithTag.boolValue, customSkin, "Only Load With Tag");

                        if (lsmTarget.onlyLoadWithTag == true)
                        {
                            GUILayout.BeginHorizontal(EditorStyles.helpBox);

                            EditorGUILayout.LabelField(new GUIContent("Object Tag"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                            selectedTagIndex.intValue = EditorGUILayout.Popup(selectedTagIndex.intValue, UnityEditorInternal.InternalEditorUtility.tags);
                            objectTag.stringValue = UnityEditorInternal.InternalEditorUtility.tags[selectedTagIndex.intValue].ToString();

                            GUILayout.EndHorizontal();
                        }

                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Load Scene"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        sceneName.stringValue = EditorGUILayout.TextField(sceneName.stringValue);

                        GUILayout.EndHorizontal();
                    }

                    LSSEditorHandler.DrawHeader(customSkin, "Events Header", 10);
                    EditorGUILayout.PropertyField(onLoadStart, new GUIContent("On Load Start"), true);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.PropertyField(dontDestroyOnLoad, new GUIContent("Don't Destroy On Load"), true);
                    EditorGUI.indentLevel = 0;
                    GUILayout.EndHorizontal();
                }

                else
                    EditorGUILayout.HelpBox("There isn't any loading screen prefab in Resoures > Loading Screens folder." +
                        "You have to create a prefab to use the loading screen system.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif