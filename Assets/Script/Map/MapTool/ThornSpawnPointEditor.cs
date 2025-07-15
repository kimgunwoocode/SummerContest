using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(ThornSpawnPoint))]
public class ThornSpawnPointEditor : Editor
{
    private List<ThornObject> thornObjects;
    private string[] thornObjectNames;
    private int selectedIndex = -1;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ThornSpawnPoint spawnPoint = (ThornSpawnPoint)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("ThornObject 연결", EditorStyles.boldLabel);

        if (thornObjects == null || thornObjectNames == null) return;

        int newIndex = EditorGUILayout.Popup("ThornObject", selectedIndex, thornObjectNames);

        if (newIndex != selectedIndex)// ThornObject 변경시
        {
            // 기존 연결된 ThornObject에서 제거
            if (spawnPoint.ThornObject != null)
            {
                Undo.RecordObject(spawnPoint.ThornObject, "Remove SpawnPoint");
                spawnPoint.ThornObject.ThornSpawnPoints.Remove(spawnPoint.gameObject);
                EditorUtility.SetDirty(spawnPoint.ThornObject);
            }

            selectedIndex = newIndex;

            if (selectedIndex == 0) // None
            {
                Undo.RecordObject(spawnPoint, "Unassign ThornObject");
                spawnPoint.ThornObject = null;

                // MapObjects 아래로 이동 + 맨 뒤로
                GameObject mapParent = GameObject.Find("MapObjects");
                if (mapParent != null)
                {
                    Undo.SetTransformParent(spawnPoint.transform, mapParent.transform, "Reparent to MapObjects");
                    spawnPoint.transform.SetParent(mapParent.transform);
                    spawnPoint.transform.SetSiblingIndex(mapParent.transform.childCount - 1); // 맨 아래로
                }
            }
            else
            {
                ThornObject selected = thornObjects[selectedIndex - 1];

                Undo.SetTransformParent(spawnPoint.transform, selected.transform, "Set Parent");
                spawnPoint.transform.SetParent(selected.transform);

                if (!selected.ThornSpawnPoints.Contains(spawnPoint.gameObject))
                {
                    Undo.RecordObject(selected, "Add Spawn Point");
                    selected.ThornSpawnPoints.Add(spawnPoint.gameObject);
                    EditorUtility.SetDirty(selected);
                }

                Undo.RecordObject(spawnPoint, "Link ThornObject");
                spawnPoint.ThornObject = selected;
                EditorUtility.SetDirty(spawnPoint);
            }
        }

        if (GUILayout.Button("목록 새로고침"))
        {
            RefreshThornObjects();
        }
    }

    private void OnEnable()
    {
        RefreshThornObjects();
    }

    private void RefreshThornObjects()
    {
        // Resources로 비활성 포함 검색
        ThornObject[] all = Resources.FindObjectsOfTypeAll<ThornObject>();
        thornObjects = new List<ThornObject>();

        foreach (var obj in all)
        {
            if (!EditorUtility.IsPersistent(obj) && obj.gameObject.scene.IsValid())
            {
                thornObjects.Add(obj);
            }
        }

        // 드롭다운 항목 이름 설정 (유니크 문자열 포함)
        thornObjectNames = new string[thornObjects.Count + 1];
        thornObjectNames[0] = "None";

        for (int i = 0; i < thornObjects.Count; i++)
        {
            // 이름이 같아도 중복되지 않도록 인스턴스 ID 포함
            thornObjectNames[i + 1] = $"{thornObjects[i].name} ({thornObjects[i].GetInstanceID()})";
        }

        ThornSpawnPoint spawnPoint = (ThornSpawnPoint)target;
        if (spawnPoint.ThornObject == null)
        {
            selectedIndex = 0;
        }
        else
        {
            int idx = thornObjects.IndexOf(spawnPoint.ThornObject);
            selectedIndex = idx >= 0 ? idx + 1 : 0;
        }
    }
}
