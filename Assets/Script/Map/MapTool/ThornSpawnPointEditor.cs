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
        EditorGUILayout.LabelField("ThornObject ����", EditorStyles.boldLabel);

        if (thornObjects == null || thornObjectNames == null) return;

        int newIndex = EditorGUILayout.Popup("ThornObject", selectedIndex, thornObjectNames);

        if (newIndex != selectedIndex)// ThornObject �����
        {
            // ���� ����� ThornObject���� ����
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

                // MapObjects �Ʒ��� �̵� + �� �ڷ�
                GameObject mapParent = GameObject.Find("MapObjects");
                if (mapParent != null)
                {
                    Undo.SetTransformParent(spawnPoint.transform, mapParent.transform, "Reparent to MapObjects");
                    spawnPoint.transform.SetParent(mapParent.transform);
                    spawnPoint.transform.SetSiblingIndex(mapParent.transform.childCount - 1); // �� �Ʒ���
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

        if (GUILayout.Button("��� ���ΰ�ħ"))
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
        // Resources�� ��Ȱ�� ���� �˻�
        ThornObject[] all = Resources.FindObjectsOfTypeAll<ThornObject>();
        thornObjects = new List<ThornObject>();

        foreach (var obj in all)
        {
            if (!EditorUtility.IsPersistent(obj) && obj.gameObject.scene.IsValid())
            {
                thornObjects.Add(obj);
            }
        }

        // ��Ӵٿ� �׸� �̸� ���� (����ũ ���ڿ� ����)
        thornObjectNames = new string[thornObjects.Count + 1];
        thornObjectNames[0] = "None";

        for (int i = 0; i < thornObjects.Count; i++)
        {
            // �̸��� ���Ƶ� �ߺ����� �ʵ��� �ν��Ͻ� ID ����
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
