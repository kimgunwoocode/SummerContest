using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#region Serializable Structures

[System.Serializable]
public class IntBoolPair
{
    public int Key;
    public bool Value;

    public IntBoolPair(int key, bool value)
    {
        Key = key;
        Value = value;
    }
}

[System.Serializable]
public class SerializableSaveData
{
    public string Name;
    public string Day;
    public SerializableMapData MapData;
    public SerializablePlayerData PlayerData;
}

[System.Serializable]
public class SerializableMapData
{
    public List<IntBoolPair> InteractionObjects;
    public List<ShopData> Shops;
    public List<IntBoolPair> SpawnPoints;
    public int SpawnPoint;
}

[System.Serializable]
public class SerializablePlayerData
{
    public int MaxHP;
    public int CurrentHP;
    public int ATK;
    public float MaxBreathGauge;
    public float CurrentBreathGauge;
    public int Money;
    public List<int> EquipSkill;
    public List<bool> PlayerAbility;
    public List<IntBoolPair> PlayerSkill;
    public List<IntBoolPair> GettedItems;
}

#endregion

public static class SaveFileManager
{
    private static string GetPath(int slotIndex)
    {
#if UNITY_EDITOR
        // Assets 내부 경로 (Unity가 인식 가능)
        string relativePath = "Assets/InitData/EditorSaveData";

        // 실제 OS상의 경로로 변환
        string fullPath = Path.Combine(Application.dataPath.Replace("/Assets", ""), relativePath);

        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);

        // Unity가 파일을 인식할 수 있도록 강제 새로고침
        AssetDatabase.Refresh();

        return Path.Combine(fullPath, $"save_slot_{slotIndex}.json");
#else
    return Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");
#endif
    }


    #region Save
    public static void Save(SaveData data, int slotIndex)
    {
        // 슬롯 번호 저장
        data.Slot = slotIndex;

        // 이름 설정
        if (string.IsNullOrEmpty(data.Name))
            data.Name = $"SaveFile{slotIndex + 1}";

        // 날짜 형식 지정
        data.Day = DateTime.Now.ToString("yy/MM/dd-HH:mm");

        // SaveData → SerializableSaveData 변환
        var serializableData = new SerializableSaveData
        {
            Name = data.Name,
            Day = data.Day,
            MapData = new SerializableMapData
            {
                InteractionObjects = ConvertDict(data.MapData.InteractionObjects),
                Shops = data.MapData.Shops,
                SpawnPoints = ConvertDict(data.MapData.SpawnPoints),
                SpawnPoint = data.MapData.SpawnPoint
            },
            PlayerData = new SerializablePlayerData
            {
                MaxHP = data.PlayerData.MaxHP,
                CurrentHP = data.PlayerData.CurrentHP,
                ATK = data.PlayerData.ATK,
                MaxBreathGauge = data.PlayerData.MaxBreathGauge,
                CurrentBreathGauge = data.PlayerData.CurrentBreathGauge,
                Money = data.PlayerData.Money,
                EquipSkill = data.PlayerData.EquipSkill,
                PlayerAbility = data.PlayerData.PlayerAbility,
                PlayerSkill = ConvertDict(data.PlayerData.PlayerSkill),
                GettedItems = ConvertDict(data.PlayerData.GettedItems)
            }
        };

        // 저장
        string json = JsonUtility.ToJson(serializableData, true);
        File.WriteAllText(GetPath(slotIndex), json);

        Debug.Log($"저장 완료: 슬롯 {slotIndex} → {data.Name} ({data.Day}) " + json);
    }

    private static List<IntBoolPair> ConvertDict(Dictionary<int, bool> dict)
    {
        var list = new List<IntBoolPair>();
        foreach (var kv in dict)
            list.Add(new IntBoolPair(kv.Key, kv.Value));
        return list;
    }
    #endregion

    #region Load
    public static SaveData LoadFromSaveFile(int slotIndex)
    {
        if (!File.Exists(GetPath(slotIndex)))
        {
            Debug.LogWarning($"저장 슬롯 {slotIndex}에 해당하는 파일이 없습니다.");
            return null;
        }

        string json = File.ReadAllText(GetPath(slotIndex));
        SerializableSaveData serializable = JsonUtility.FromJson<SerializableSaveData>(json);

        // 역직렬화: SerializableSaveData → SaveData
        SaveData result = new SaveData
        {
            Slot = slotIndex,
            Name = serializable.Name,
            Day = serializable.Day,
            MapData = new MapData
            {
                InteractionObjects = ToDictionary(serializable.MapData.InteractionObjects),
                Shops = serializable.MapData.Shops,
                SpawnPoints = ToDictionary(serializable.MapData.SpawnPoints),
                SpawnPoint = serializable.MapData.SpawnPoint
            },
            PlayerData = new PlayerData
            {
                MaxHP = serializable.PlayerData.MaxHP,
                CurrentHP = serializable.PlayerData.CurrentHP,
                ATK = serializable.PlayerData.ATK,
                MaxBreathGauge = serializable.PlayerData.MaxBreathGauge,
                CurrentBreathGauge = serializable.PlayerData.CurrentBreathGauge,
                Money = serializable.PlayerData.Money,
                EquipSkill = serializable.PlayerData.EquipSkill,
                PlayerAbility = serializable.PlayerData.PlayerAbility,
                PlayerSkill = ToDictionary(serializable.PlayerData.PlayerSkill),
                GettedItems = ToDictionary(serializable.PlayerData.GettedItems)
            }
        };

        Debug.Log($"슬롯 {slotIndex} 로드 완료: {result.Name} ({result.Day})");
        return result;
    }

    public static void Load(int slotIndex)
    {
        SaveData loaded = LoadFromSaveFile(slotIndex);
        if (loaded != null)
        {
            Singleton.GameManager_Instance.Get<GameDataManager>().LoadGameData(loaded);
            Debug.Log($"GameDataManager에 SaveData 적용 완료 (슬롯 {slotIndex})");
        }
        else
        {
            Debug.LogWarning($"슬롯 {slotIndex} 로드 실패");
        }
    }

    private static Dictionary<int, bool> ToDictionary(List<IntBoolPair> list)
    {
        var dict = new Dictionary<int, bool>();
        foreach (var pair in list)
            dict[pair.Key] = pair.Value;
        return dict;
    }
    #endregion



    [MenuItem("Tools/SaveInitData")]
    public static void SaveInitDataToSlot0()
    {
        const string saveDataPath = "Assets/InitData/InitData.asset";
        SaveData initData = AssetDatabase.LoadAssetAtPath<InitSaveData>(saveDataPath).InitData;

        if (initData == null)
        {
            EditorUtility.DisplayDialog("에러", "InitData.asset을 찾을 수 없습니다.", "확인");
            return;
        }

        SaveFileManager.Save(initData, 0);
        Debug.Log("InitData가 슬롯 0번에 저장되었습니다.");
    }
}