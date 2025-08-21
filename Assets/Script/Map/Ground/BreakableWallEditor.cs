#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BreakableWall))]
public class BreakableWallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 내가 정의한 필드만 수동으로 보여줌
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("변수 이름"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("animator"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sound"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif