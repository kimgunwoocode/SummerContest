using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Singleton))]
public class SingletonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // 기존 인스펙터 그리기

        Singleton singleton = (Singleton)target;

        GUILayout.Space(10);
        if (GUILayout.Button("컴포넌트 자동 등록"))
        {
            Undo.RecordObject(singleton, "Fill Script Entries"); // 되돌리기 목록에 추가
            singleton.FillScriptEntries(); //함수 호출
            EditorUtility.SetDirty(singleton); // 변경사항 존재 표시하기
        }
    }
}
