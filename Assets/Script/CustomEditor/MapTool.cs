using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;

public class MapTool : EditorWindow
{
    [MenuItem("Window/MapTool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MapTool));
    }

    void OnGUI()
    {
        // The actual window code goes here
    }
}
