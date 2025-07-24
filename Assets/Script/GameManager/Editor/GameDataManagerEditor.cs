using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[CustomEditor(typeof(GameDataManager))]
public class GameDataManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("아이템 등록"))
        {
            RegisterAllItems();
        }
    }

    private void RegisterAllItems()
    {
        GameDataManager manager = (GameDataManager)target;
        if (manager == null)
        {
            Debug.LogError("GameDataManager target is null");
            return;
        }

        // 1. Assets/ItemData 하위에서 모든 ItemData SO 불러오기
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
                if (seenIDs.Contains(item.itemID))
                {
                    duplicateIDs.Add(item.itemID);
                    Debug.LogError($"[중복 ID 감지] itemID {item.itemID} - 파일 경로: {path}");
                    continue; // 중복된 항목은 등록하지 않음
                }

                seenIDs.Add(item.itemID);
                foundItems.Add(item);
                allitems_dic[item.itemID] = item;
            }
        }

        // 2. itemID 기준 정렬
        foundItems = foundItems.OrderBy(item => item.itemID).ToList();

        // 3. GameDataManager에 할당
        Undo.RecordObject(manager, "아이템 자동 등록");
        manager.allitems = foundItems;
        manager.allitems_dic = allitems_dic;
        EditorUtility.SetDirty(manager);

        // 4. 로그 출력
        Debug.Log($"[GameDataManagerEditor] 총 {foundItems.Count}개의 아이템이 등록되었습니다.");
        Debug.LogWarning($"[GameDataManagerEditor] {guids.Length - foundItems.Count}개의 아이템이 제외되었습니다. (itemID가 초기화되지 않아 0 이거나 중복된 itemeID)");

        if (duplicateIDs.Count > 0)
        {
            string dupText = string.Join(", ", duplicateIDs.Distinct());
            Debug.LogError($"[GameDataManagerEditor] 중복된 itemID 감지: {dupText}");
        }
    }
}
