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


        // 1. 불러오기
        string[] guids = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/ItemData" });

        List<ItemData> foundItems = new();
        Dictionary<int, ItemData> allitems_dic = new();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            if (item != null && item.itemID != 0) // 테스트 아이템 제외 (그리고 ID 미설정한 아이템도 제외됨)
            {
                foundItems.Add(item);
                allitems_dic[item.itemID] = item;
            }
        }

        // 2. itemID 기준 정렬
        foundItems = foundItems.OrderBy(item => item.itemID).ToList();

        // 3. GameDataManager에 할당
        Undo.RecordObject(manager, "아이템 자동 등록");
        manager.allitems = foundItems;
        EditorUtility.SetDirty(manager);

        Debug.Log($"[GameDataManagerEditor] 총 {foundItems.Count}개의 아이템이 등록되었습니다.");
        Debug.LogWarning($"[GameDataManagerEditor] 총 {guids.Length-foundItems.Count}개의 아이템이 제외되었습니다. 테스트아이템이 아니라면, ID설정을 해주시기 바랍니다.");
    }
}
