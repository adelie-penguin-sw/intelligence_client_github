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

    void OnGUI()
    {
        GUILayout.Label("url Settings", EditorStyles.boldLabel);
        urlType = (UrlType)EditorGUILayout.EnumPopup("urlType", urlType);
        Debug.LogError(urlType);
        switch(urlType)
        {
            case UrlType.TEST:
                NetworkManager.Instance.editorBaseUrl = "http://ec2-52-79-187-33.ap-northeast-2.compute.amazonaws.com:8080"; //테스트 서버 url
                break;
            case UrlType.DEPLOY:
                NetworkManager.Instance.editorBaseUrl = "http://ec2-3-38-74-157.ap-northeast-2.compute.amazonaws.com:8080"; //배포 서버 url
                break;
            case UrlType.LOCAL:
                NetworkManager.Instance.editorBaseUrl = "127.0.0.1";
                break;
        }
    }
}

public enum UrlType { TEST = 1, DEPLOY = 2, LOCAL = 3, }