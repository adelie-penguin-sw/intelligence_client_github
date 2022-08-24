using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NetworkUrlChanger : EditorWindow
{
    public static UrlType urlType = UrlType.TEST;
   
    // Add menu item named "My Window" to the Window menu
    [MenuItem("Network/NetworkUrlChanger")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(NetworkUrlChanger));
    }

    protected void OnEnable()
    {
        urlType = (UrlType)EditorPrefs.GetInt("urlType");
    }
    protected void OnDisable()
    {
        EditorPrefs.SetInt("urlType", (int)urlType);
    }


    void OnGUI()
    {
        GUILayout.Label("url Settings", EditorStyles.boldLabel);
        urlType = (UrlType)EditorGUILayout.EnumPopup("urlType", urlType);
        EditorPrefs.SetInt("urlType", (int)urlType);
    }
}
