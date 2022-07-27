#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;

namespace Michsky.LSS
{
    [CustomEditor(typeof(LoadingScreen))]
    [System.Serializable]
    public class LoadingScreenEditor : Editor
    {
        private LoadingScreen lsTarget;
        private int currentTab;
        List<Transform> spinnerList = new List<Transform>();
        List<string> spinnerTitles = new List<string>();
        int selectedSpinnerIndex;
        Image pakCountdownFilled;
        Image pakCountdownBG;
        SpinnerItem selectedSpinnerItem;

        private void OnEnable()
        {
            lsTarget = (LoadingScreen)target;

            foreach (Transform child in lsTarget.spinnerParent)
                spinnerList.Add(child);

            foreach (var t in spinnerList)
                spinnerTitles.Add(t.name);

            selectedSpinnerIndex = lsTarget.spinnerHelper;
        }

        public override void OnInspectorGUI()
        {
            GUISkin customSkin;
            Color defaultColor = GUI.color;

            if (EditorGUIUtility.isProSkin == true)
                customSkin = (GUISkin)Resources.Load("Editor\\LSS Skin Dark");
            else
                customSkin = (GUISkin)Resources.Load("Editor\\LSS Skin Light");

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = defaultColor;

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("LS Top Header"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-42);

            GUIContent[] toolbarTabs = new GUIContent[5];
            toolbarTabs[0] = new GUIContent("Layout");
            toolbarTabs[1] = new GUIContent("Hints");
            toolbarTabs[2] = new GUIContent("Images");
            toolbarTabs[3] = new GUIContent("Resources");
            toolbarTabs[4] = new GUIContent("Settings");

            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            currentTab = GUILayout.Toolbar(currentTab, toolbarTabs, customSkin.FindStyle("Tab Indicator"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-40);
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Hints", "Hints"), customSkin.FindStyle("Tab Hints")))
                currentTab = 1;
            if (GUILayout.Button(new GUIContent("Background", "Background"), customSkin.FindStyle("Tab Background")))
                currentTab = 2;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 3;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 4;

            GUILayout.EndHorizontal();

            // Property variables
            var titleObjText = serializedObject.FindProperty("titleObjText");
            var titleObjDescText = serializedObject.FindProperty("titleObjDescText");
            var backgroundImage = serializedObject.FindProperty("backgroundImage");
            var updateHelper = serializedObject.FindProperty("updateHelper");

            // Layout variables
            var titleColor = serializedObject.FindProperty("titleColor");
            var titleSize = serializedObject.FindProperty("titleSize");
            var titleFont = serializedObject.FindProperty("titleFont");
            var descriptionColor = serializedObject.FindProperty("descriptionColor");
            var descriptionSize = serializedObject.FindProperty("descriptionSize");
            var descriptionFont = serializedObject.FindProperty("descriptionFont");
            var hintColor = serializedObject.FindProperty("hintColor");
            var hintSize = serializedObject.FindProperty("hintSize");
            var hintFont = serializedObject.FindProperty("hintFont");
            var statusColor = serializedObject.FindProperty("statusColor");
            var statusSize = serializedObject.FindProperty("statusSize");
            var statusFont = serializedObject.FindProperty("statusFont");
            var pakColor = serializedObject.FindProperty("pakColor");
            var pakSize = serializedObject.FindProperty("pakSize");
            var pakFont = serializedObject.FindProperty("pakFont");
            var spinnerColor = serializedObject.FindProperty("spinnerColor");

            // Hint variables
            var enableRandomHints = serializedObject.FindProperty("enableRandomHints");
            var hintList = serializedObject.FindProperty("hintList");
            var changeHintWithTimer = serializedObject.FindProperty("changeHintWithTimer");
            var hintTimerValue = serializedObject.FindProperty("hintTimerValue");

            // Image variables
            var enableRandomImages = serializedObject.FindProperty("enableRandomImages");
            var ImageList = serializedObject.FindProperty("ImageList");
            var changeImageWithTimer = serializedObject.FindProperty("changeImageWithTimer");
            var imageTimerValue = serializedObject.FindProperty("imageTimerValue");
            var imageFadingSpeed = serializedObject.FindProperty("imageFadingSpeed");

            // Resources
            var canvasGroup = serializedObject.FindProperty("canvasGroup");
            var statusObj = serializedObject.FindProperty("statusObj");
            var titleObj = serializedObject.FindProperty("titleObj");
            var descriptionObj = serializedObject.FindProperty("descriptionObj");
            var progressBar = serializedObject.FindProperty("progressBar");
            var hintsText = serializedObject.FindProperty("hintsText");
            var imageObject = serializedObject.FindProperty("imageObject");
            var fadingAnimator = serializedObject.FindProperty("fadingAnimator");
            var objectAnimator = serializedObject.FindProperty("objectAnimator");
            var pakTextObj = serializedObject.FindProperty("pakTextObj");
            var pakCountdownSlider = serializedObject.FindProperty("pakCountdownSlider");
            var pakCountdownLabel = serializedObject.FindProperty("pakCountdownLabel");
            var spinnerParent = serializedObject.FindProperty("spinnerParent");

            // Settings
            var enableTitle = serializedObject.FindProperty("enableTitle");
            var enableDescription = serializedObject.FindProperty("enableDescription");
            var enableStatusLabel = serializedObject.FindProperty("enableStatusLabel");
            var enablePressAnyKey = serializedObject.FindProperty("enablePressAnyKey");
            var useSpecificKey = serializedObject.FindProperty("useSpecificKey");
            var keyCode = serializedObject.FindProperty("keyCode");
            var pakText = serializedObject.FindProperty("pakText");
            var pakCountdownTimer = serializedObject.FindProperty("pakCountdownTimer");
            var enableVirtualLoading = serializedObject.FindProperty("enableVirtualLoading");
            var virtualLoadingTimer = serializedObject.FindProperty("virtualLoadingTimer");
            var fadingAnimationSpeed = serializedObject.FindProperty("fadingAnimationSpeed");
            var onFinishEvents = serializedObject.FindProperty("onFinishEvents");
            var onBeginEvents = serializedObject.FindProperty("onBeginEvents");
            var forceCanvasGroup = serializedObject.FindProperty("forceCanvasGroup");

            switch (currentTab)
            {
                case 0:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Layout Header"));

                    if (enableTitle.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField(new GUIContent("Title Text"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        GUILayout.Space(-14);
                        EditorGUILayout.PropertyField(titleObjText, new GUIContent(""), GUILayout.Height(50));
                        GUILayout.EndVertical();

                        if (titleObj != null)
                            lsTarget.titleObj.text = lsTarget.titleObjText;
                    }

                    if (enableDescription.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField(new GUIContent("Description Text"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        GUILayout.Space(-14);
                        EditorGUILayout.PropertyField(titleObjDescText, new GUIContent(""), GUILayout.Height(50));
                        GUILayout.EndVertical();

                        if (titleObj != null)
                            lsTarget.descriptionObj.text = lsTarget.titleObjDescText;
                    }

                    if (enableTitle.boolValue == false && enableDescription.boolValue == false)
                        EditorGUILayout.HelpBox("Both Title and Description is disabled.", MessageType.Info);

                    if (enablePressAnyKey.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField(new GUIContent("PAK Text"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        GUILayout.Space(-14);
                        EditorGUILayout.PropertyField(pakText, new GUIContent(""), GUILayout.Height(50));
                        GUILayout.EndVertical();

                        if (pakTextObj != null)
                            lsTarget.pakTextObj.text = lsTarget.pakText;
                    }

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUILayout.LabelField(new GUIContent("Selected Spinner"), customSkin.FindStyle("Text"), GUILayout.Width(120));

                    selectedSpinnerIndex = EditorGUILayout.Popup(selectedSpinnerIndex, spinnerTitles.ToArray());
                    lsTarget.spinnerHelper = selectedSpinnerIndex;

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();

                    if (lsTarget.canvasGroup.alpha == 0)
                    {
                        if (GUILayout.Button("Make It Visible", customSkin.button))
                            lsTarget.canvasGroup.alpha = 1;
                    }

                    else
                    {
                        if (GUILayout.Button("Make It Invisible", customSkin.button))
                            lsTarget.canvasGroup.alpha = 0;
                    }

                    if (GUILayout.Button("Update Values", customSkin.button))
                    {
                        if (enableTitle.boolValue == true)
                        {
                            lsTarget.titleObj.fontSize = lsTarget.titleSize;
                            lsTarget.titleObj.font = lsTarget.titleFont;
                            lsTarget.titleObj.color = lsTarget.titleColor;
                        }

                        if (enableDescription.boolValue == true)
                        {
                            lsTarget.descriptionObj.fontSize = lsTarget.descriptionSize;
                            lsTarget.descriptionObj.font = lsTarget.descriptionFont;
                            lsTarget.descriptionObj.color = lsTarget.descriptionColor;
                        }

                        if (enableRandomHints.boolValue == true)
                        {
                            lsTarget.hintsText.fontSize = lsTarget.hintSize;
                            lsTarget.hintsText.font = lsTarget.hintFont;
                            lsTarget.hintsText.color = lsTarget.hintColor;
                        }

                        if (enableStatusLabel.boolValue == true)
                        {
                            lsTarget.statusObj.fontSize = lsTarget.statusSize;
                            lsTarget.statusObj.font = lsTarget.statusFont;
                            lsTarget.statusObj.color = lsTarget.statusColor;
                        }

                        if (enablePressAnyKey.boolValue == true)
                        {
                            lsTarget.pakTextObj.fontSize = lsTarget.pakSize;
                            lsTarget.pakTextObj.font = lsTarget.pakFont;
                            lsTarget.pakTextObj.color = lsTarget.pakColor;
                            lsTarget.pakCountdownLabel.color = lsTarget.pakColor;

                            try
                            {
                                pakCountdownFilled = lsTarget.pakCountdownSlider.transform.Find("Filled").GetComponent<Image>();
                                pakCountdownBG = lsTarget.pakCountdownSlider.transform.Find("Background").GetComponent<Image>();
                            }

                            catch { }

                            if (pakCountdownFilled != null && pakCountdownBG != null)
                            {
                                pakCountdownFilled.color = lsTarget.spinnerColor;
                                pakCountdownBG.color = new Color(lsTarget.spinnerColor.r, lsTarget.spinnerColor.g, lsTarget.spinnerColor.b, 0.08f);
                            }
                        }

                        try
                        {
                            selectedSpinnerItem = spinnerList[selectedSpinnerIndex].GetComponent<SpinnerItem>();
                            selectedSpinnerItem.UpdateValues();
                        }

                        catch { Debug.Log("Loading Screen - Cannot initialize selected Spinner Item.", this); }

                        updateHelper.boolValue = true;
                        updateHelper.boolValue = false;
                    }

                    GUILayout.EndHorizontal();

                    foreach (Transform child in lsTarget.spinnerParent)
                    {
                        if (child.name != spinnerList[selectedSpinnerIndex].ToString().Replace(" (UnityEngine.RectTransform)", "").Trim())
                            child.gameObject.SetActive(false);
                        else
                            child.gameObject.SetActive(true);
                    }

                    GUILayout.Space(10);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Customization Header"));

                    if (enableTitle.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField(new GUIContent("Title"), customSkin.FindStyle("Text"));
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(titleSize, new GUIContent(""), GUILayout.Width(40));
                        EditorGUILayout.PropertyField(titleFont, new GUIContent(""));
                        EditorGUILayout.PropertyField(titleColor, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }

                    if (enableDescription.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField(new GUIContent("Description"), customSkin.FindStyle("Text"), GUILayout.Width(100));
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(descriptionSize, new GUIContent(""), GUILayout.Width(40));
                        EditorGUILayout.PropertyField(descriptionFont, new GUIContent(""));
                        EditorGUILayout.PropertyField(descriptionColor, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }

                    if (enableRandomHints.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField(new GUIContent("Hint"), customSkin.FindStyle("Text"), GUILayout.Width(100));
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(hintSize, new GUIContent(""), GUILayout.Width(40));
                        EditorGUILayout.PropertyField(hintFont, new GUIContent(""));
                        EditorGUILayout.PropertyField(hintColor, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }

                    if (enableStatusLabel.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField(new GUIContent("Status"), customSkin.FindStyle("Text"), GUILayout.Width(100));
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(statusSize, new GUIContent(""), GUILayout.Width(40));
                        EditorGUILayout.PropertyField(statusFont, new GUIContent(""));
                        EditorGUILayout.PropertyField(statusColor, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }

                    if (enablePressAnyKey.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField(new GUIContent("Press Any Key"), customSkin.FindStyle("Text"), GUILayout.Width(100));
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(pakSize, new GUIContent(""), GUILayout.Width(40));
                        EditorGUILayout.PropertyField(pakFont, new GUIContent(""));
                        EditorGUILayout.PropertyField(pakColor, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }
                  
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField(new GUIContent("Spinner"), customSkin.FindStyle("Text"), GUILayout.Width(100));
                    GUILayout.BeginHorizontal();

                    EditorGUILayout.PropertyField(spinnerColor, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("Restore Values", customSkin.button))
                    {
                        if (enableTitle.boolValue == true)
                        {
                            lsTarget.titleSize = lsTarget.titleObj.fontSize;
                            lsTarget.titleFont = lsTarget.titleObj.font;
                            lsTarget.titleColor = lsTarget.titleObj.color;
                        }

                        if (enableDescription.boolValue == true)
                        {
                            lsTarget.descriptionSize = lsTarget.descriptionObj.fontSize;
                            lsTarget.descriptionFont = lsTarget.descriptionObj.font;
                            lsTarget.descriptionColor = lsTarget.descriptionObj.color;
                        }

                        if (enableRandomHints.boolValue == true)
                        {
                            lsTarget.hintSize = lsTarget.hintsText.fontSize;
                            lsTarget.hintFont = lsTarget.hintsText.font;
                            lsTarget.hintColor = lsTarget.hintsText.color;
                        }

                        if (enableStatusLabel.boolValue == true)
                        {
                            lsTarget.statusSize = lsTarget.statusObj.fontSize;
                            lsTarget.statusFont = lsTarget.statusObj.font;
                            lsTarget.statusColor = lsTarget.statusObj.color;
                        }

                        if (enablePressAnyKey.boolValue == true)
                        {
                            lsTarget.pakSize = lsTarget.pakTextObj.fontSize;
                            lsTarget.pakFont = lsTarget.pakTextObj.font;
                            lsTarget.pakColor = lsTarget.pakTextObj.color;
                        }
                    }

                    if (GUILayout.Button("Update Values", customSkin.button))
                    {
                        if (enableTitle.boolValue == true)
                        {
                            lsTarget.titleObj.fontSize = lsTarget.titleSize;
                            lsTarget.titleObj.font = lsTarget.titleFont;
                            lsTarget.titleObj.color = lsTarget.titleColor;
                        }
                          
                        if (enableDescription.boolValue == true)
                        {
                            lsTarget.descriptionObj.fontSize = lsTarget.descriptionSize;
                            lsTarget.descriptionObj.font = lsTarget.descriptionFont;
                            lsTarget.descriptionObj.color = lsTarget.descriptionColor;
                        }

                        if (enableRandomHints.boolValue == true)
                        {
                            lsTarget.hintsText.fontSize = lsTarget.hintSize;
                            lsTarget.hintsText.font = lsTarget.hintFont;
                            lsTarget.hintsText.color = lsTarget.hintColor;
                        }

                        if (enableStatusLabel.boolValue == true)
                        {
                            lsTarget.statusObj.fontSize = lsTarget.statusSize;
                            lsTarget.statusObj.font = lsTarget.statusFont;
                            lsTarget.statusObj.color = lsTarget.statusColor;
                        }

                        if (enablePressAnyKey.boolValue == true)
                        {
                            lsTarget.pakTextObj.fontSize = lsTarget.pakSize;
                            lsTarget.pakTextObj.font = lsTarget.pakFont;
                            lsTarget.pakTextObj.color = lsTarget.pakColor;
                            lsTarget.pakCountdownLabel.color = lsTarget.pakColor;

                            try
                            {
                                pakCountdownFilled = lsTarget.pakCountdownSlider.transform.Find("Filled").GetComponent<Image>();
                                pakCountdownBG = lsTarget.pakCountdownSlider.transform.Find("Background").GetComponent<Image>();
                            }

                            catch { }

                            if (pakCountdownFilled != null && pakCountdownBG != null)
                            {
                                pakCountdownFilled.color = lsTarget.spinnerColor;
                                pakCountdownBG.color = new Color(lsTarget.spinnerColor.r, lsTarget.spinnerColor.g, lsTarget.spinnerColor.b, 0.08f);
                            }
                        }

                        try
                        {
                            selectedSpinnerItem = spinnerList[selectedSpinnerIndex].GetComponent<SpinnerItem>();
                            selectedSpinnerItem.UpdateValues();
                        }

                        catch { Debug.Log("Loading Screen - Cannot initialize selected Spinner Item.", this); }

                        updateHelper.boolValue = true;
                        updateHelper.boolValue = false;
                    }

                    GUILayout.EndHorizontal();
                    break;

                case 1:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Hints Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableRandomHints.boolValue = GUILayout.Toggle(enableRandomHints.boolValue, new GUIContent("Enable Random Hints"), customSkin.FindStyle("Toggle"));
                    enableRandomHints.boolValue = GUILayout.Toggle(enableRandomHints.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (enableRandomHints.boolValue == true)
                    {
                        if (hintsText != null)
                            lsTarget.hintsText.gameObject.SetActive(true);

                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        changeHintWithTimer.boolValue = GUILayout.Toggle(changeHintWithTimer.boolValue, new GUIContent("Change With Timer"), customSkin.FindStyle("Toggle"));
                        changeHintWithTimer.boolValue = GUILayout.Toggle(changeHintWithTimer.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                        GUILayout.EndHorizontal();

                        if (changeHintWithTimer.boolValue == true)
                        {
                            GUILayout.BeginHorizontal(EditorStyles.helpBox);

                            EditorGUILayout.LabelField(new GUIContent("Timer Value"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                            EditorGUILayout.PropertyField(hintTimerValue, new GUIContent(""));

                            GUILayout.EndHorizontal();
                        }

                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        EditorGUI.indentLevel = 1;

                        EditorGUILayout.PropertyField(hintList, new GUIContent("Hint List"), true);

                        EditorGUI.indentLevel = 0;
                        GUILayout.Space(6);
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("+ Add a new hint", customSkin.button))
                            lsTarget.hintList.Add("Type your hint here.");

                        if (lsTarget.hintsText == null)
                        {
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.HelpBox("'Hint Text Object' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                            GUILayout.EndHorizontal();
                        }
                    }

                    else if (enableRandomHints.boolValue == false && hintsText != null)
                        lsTarget.hintsText.gameObject.SetActive(false);

                    GUILayout.Space(6);
                    break;

                case 2:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Background Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableRandomImages.boolValue = GUILayout.Toggle(enableRandomImages.boolValue, new GUIContent("Enable Random Background Images"), customSkin.FindStyle("Toggle"));
                    enableRandomImages.boolValue = GUILayout.Toggle(enableRandomImages.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (enableRandomImages.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        changeImageWithTimer.boolValue = GUILayout.Toggle(changeImageWithTimer.boolValue, new GUIContent("Change With Timer"), customSkin.FindStyle("Toggle"));
                        changeImageWithTimer.boolValue = GUILayout.Toggle(changeImageWithTimer.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                        GUILayout.EndHorizontal();

                        if (changeImageWithTimer.boolValue == true)
                        {
                            GUILayout.BeginHorizontal(EditorStyles.helpBox);

                            EditorGUILayout.LabelField(new GUIContent("Timer Value"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                            EditorGUILayout.PropertyField(imageTimerValue, new GUIContent(""));

                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal(EditorStyles.helpBox);

                            EditorGUILayout.LabelField(new GUIContent("Fading Speed"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                            EditorGUILayout.PropertyField(imageFadingSpeed, new GUIContent(""));

                            GUILayout.EndHorizontal();
                        }

                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        EditorGUI.indentLevel = 1;

                        EditorGUILayout.PropertyField(ImageList, new GUIContent("Image List"), true);
                        ImageList.isExpanded = true;

                        EditorGUI.indentLevel = 0;
                        GUILayout.Space(6);
                        GUILayout.EndHorizontal();

                        if (lsTarget.imageObject == null)
                        {
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.HelpBox("'Image Object' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                            GUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button("+ Add a new image", customSkin.button))
                            lsTarget.ImageList.Add(null);
                    }

                    else
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Background Image"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(backgroundImage, new GUIContent(""));

                        GUILayout.EndHorizontal();

                        if (lsTarget.imageObject.sprite != lsTarget.backgroundImage)
                        {
                            lsTarget.imageObject.sprite = lsTarget.backgroundImage;
                            updateHelper.boolValue = true;
                            updateHelper.boolValue = false;
                        }
                    }

                    GUILayout.Space(6);
                    break;

                case 3:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Resources Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Canvas Group"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(canvasGroup, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Status Label"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(statusObj, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    if (enableTitle.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Title Object"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(titleObj, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    if (enableDescription.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Description Object"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(descriptionObj, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    if (enableRandomHints.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Hint Text Object"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(hintsText, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Progress Slider"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(progressBar, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Main Animator"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(objectAnimator, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Spinner Parent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(spinnerParent, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    if (enableRandomImages.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Image Object"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(imageObject, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Image Animator"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(fadingAnimator, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    if (enablePressAnyKey.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("PAK Text Object"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(pakTextObj, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("PAK Countdown Slider"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(pakCountdownSlider, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("PAK Countdown Label"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(pakCountdownLabel, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.Space(6);
                    break;

                case 4:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Settings Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableTitle.boolValue = GUILayout.Toggle(enableTitle.boolValue, new GUIContent("Enable Title"), customSkin.FindStyle("Toggle"));
                    enableTitle.boolValue = GUILayout.Toggle(enableTitle.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (enableTitle.boolValue == false && titleObj != null)
                        lsTarget.titleObj.gameObject.SetActive(false);

                    else if (enableTitle.boolValue == true && titleObj != null)
                        lsTarget.titleObj.gameObject.SetActive(true);

                    else if (enableTitle.boolValue == true && titleObj == null)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox("'Title Object' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableDescription.boolValue = GUILayout.Toggle(enableDescription.boolValue, new GUIContent("Enable Description"), customSkin.FindStyle("Toggle"));
                    enableDescription.boolValue = GUILayout.Toggle(enableDescription.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (enableDescription.boolValue == false && descriptionObj != null)
                        lsTarget.descriptionObj.gameObject.SetActive(false);

                    else if (enableDescription.boolValue == true && descriptionObj != null)
                        lsTarget.descriptionObj.gameObject.SetActive(true);

                    else if (enableDescription.boolValue == true && descriptionObj == null)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox("'Description Object' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableStatusLabel.boolValue = GUILayout.Toggle(enableStatusLabel.boolValue, new GUIContent("Enable Status Label"), customSkin.FindStyle("Toggle"));
                    enableStatusLabel.boolValue = GUILayout.Toggle(enableStatusLabel.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (enableStatusLabel.boolValue == false && statusObj != null)
                        lsTarget.statusObj.gameObject.SetActive(false);

                    else if (enableStatusLabel.boolValue == true && statusObj != null)
                        lsTarget.statusObj.gameObject.SetActive(true);

                    else if (enableStatusLabel.boolValue == true && statusObj == null)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox("'Status Label' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    forceCanvasGroup.boolValue = GUILayout.Toggle(forceCanvasGroup.boolValue, new GUIContent("Force Canvas Group"), customSkin.FindStyle("Toggle"));
                    forceCanvasGroup.boolValue = GUILayout.Toggle(forceCanvasGroup.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enablePressAnyKey.boolValue = GUILayout.Toggle(enablePressAnyKey.boolValue, new GUIContent("Enable Press Any Key"), customSkin.FindStyle("Toggle"));
                    enablePressAnyKey.boolValue = GUILayout.Toggle(enablePressAnyKey.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (enablePressAnyKey.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        useSpecificKey.boolValue = GUILayout.Toggle(useSpecificKey.boolValue, new GUIContent("Use Specific PAK key"), customSkin.FindStyle("Toggle"));
                        useSpecificKey.boolValue = GUILayout.Toggle(useSpecificKey.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                        GUILayout.EndHorizontal();
                    }

                    if (lsTarget.objectAnimator == null && enablePressAnyKey.boolValue == true)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox("'Main Animator' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableVirtualLoading.boolValue = GUILayout.Toggle(enableVirtualLoading.boolValue, new GUIContent("Enable Virtual Loading"), customSkin.FindStyle("Toggle"));
                    enableVirtualLoading.boolValue = GUILayout.Toggle(enableVirtualLoading.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (enableVirtualLoading.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Virtual Load Time"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(virtualLoadingTimer, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Fading Speed"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(fadingAnimationSpeed, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    if (useSpecificKey.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("PAK Hotkey"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(keyCode, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    if (enablePressAnyKey.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("PAK Countdown"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(pakCountdownTimer, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.Space(10);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Events Header"));

                    EditorGUILayout.PropertyField(onBeginEvents);
                    EditorGUILayout.PropertyField(onFinishEvents);

                    GUILayout.Space(6);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif