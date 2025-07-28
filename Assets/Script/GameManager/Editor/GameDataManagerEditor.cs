using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[CustomEditor(typeof(AllItems))]
public class AllItemsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("아이템 자동 등록"))
        {
            RegisterAllItems();
        }
    }

    private void RegisterAllItems()
    {
        AllItems AllItems_ScriptableObject = (AllItems)target;
        if (AllItems_ScriptableObject == null)
        {
            Debug.LogError("AllItems ScriptableObject를 찾을 수 없습니다.");
            return;
        }

        Undo.RecordObject(AllItems_ScriptableObject, "AllItems 자동 등록");


        string[] guids = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/ItemData" });

        List<ItemData> foundItems = new();
        Dictionary<int, ItemData> allitems_dic = new();
        HashSet<int> seenIDs = new();
        List<int> duplicateIDs = new();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);

            if (item != null && item.itemID != 0)
            {
                if (!seenIDs.Add(item.itemID))
                {
                    duplicateIDs.Add(item.itemID);
                    Debug.Log($"[중복 ID 감지] itemID {item.itemID} - 경로: {path}");
                    continue;
                }

                foundItems.Add(item);
                allitems_dic[item.itemID] = item;
            }
        }

        foundItems = foundItems.OrderBy(item => item.itemID).ToList();

        // ScriptableObject에 반영
        AllItems_ScriptableObject.allitems = foundItems;
        AllItems_ScriptableObject.allitems_dic = allitems_dic;

        EditorUtility.SetDirty(AllItems_ScriptableObject);
        AssetDatabase.SaveAssets();

        // 로그 출력
        Debug.Log($"[AllItemsEditor] 총 {foundItems.Count}개의 아이템이 등록되었습니다.");
        if (duplicateIDs.Count > 0)
        {
            string dupText = string.Join(", ", duplicateIDs.Distinct());
            Debug.LogError($"[AllItemsEditor] 중복된 itemID 감지: {dupText}");
        }
    }
}
