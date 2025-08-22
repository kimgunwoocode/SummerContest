using UnityEditor;

[CustomEditor(typeof(BurnWall))]
public class BurnWallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 내가 정의한 필드만 수동으로 보여줌
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("변수 이름"));

        serializedObject.ApplyModifiedProperties();
    }
}