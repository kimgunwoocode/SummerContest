using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeyManager))]
public class KeyManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        KeyManager KeyManager = (KeyManager)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("사용자 키 설정 적용"))
        {
            KeyManager.InitializeUserBindingsFromInput();
        }

        if (GUILayout.Button("Jump 바인딩 확인"))
        {
            Debug.Log(KeyManager.GetCurrentKeyDisplay("Jump"));
        }

        if (GUILayout.Button("Jump 바인딩 변경"))
        {
            KeyManager.StartListeningForKey("Jump");
        }
    }
}
