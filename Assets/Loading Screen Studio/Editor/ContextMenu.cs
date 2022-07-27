#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Michsky.LSS
{
    public class ContextMenu : Editor
    {
        static void CreateObject(string resourcePath)
        {
            GameObject clone = Instantiate(Resources.Load<GameObject>(resourcePath), Vector3.zero, Quaternion.identity) as GameObject;
            clone.name = clone.name.Replace("(Clone)", "").Trim();

            if (Selection.activeGameObject != null)
                clone.transform.SetParent(Selection.activeGameObject.transform, false);

            Undo.RegisterCreatedObjectUndo(clone, "Created LSS Manager");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        [MenuItem("Tools/Loading Screen Studio/Create Loading Manager", false, 0)]
        static void ADDLMT()
        {
            CreateObject("Loading Manager");
        }

        [MenuItem("Tools/Loading Screen Studio/Show Loading Screens", false, 0)]
        static void SHOWLS()
        {
            Selection.activeObject = Resources.Load("Loading Screens/" + LoadingScreen.prefabName.Replace(" (UnityEngine.GameObject)", "").Trim());
        }

        [MenuItem("GameObject/Loading Screen Studio/Create Loading Manager", false, 0)]
        static void ADDLM()
        {
            CreateObject("Loading Manager");
        }
    }
}
#endif