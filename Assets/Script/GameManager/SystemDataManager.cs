using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SystemDataManager : MonoBehaviour
{
    public SystemData SystemData;

    private void Awake()
    {
        Load();
    }

    private void OnApplicationQuit()
    {
        Save(SystemData);
    }

    public string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, $"SystemData.json");
    }

    public void Save(SystemData data)
    {
        data.Day = DateTime.Now.ToString("yy/MM/dd-HH:mm");

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(), json);

#if UNITY_EDITOR
        // Assets 내부 경로 (Unity가 인식 가능)
        string relativePath = "Assets/InitData/EditorSaveData";

        // 실제 OS상의 경로로 변환
        string fullPath = Path.Combine(Application.dataPath.Replace("/Assets", ""), relativePath);

        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);

        // Unity가 파일을 인식할 수 있도록 강제 새로고침
        AssetDatabase.Refresh();

        string path = Path.Combine(fullPath, $"SystemData.json");

        File.WriteAllText(path, json);
#endif
    }

    public void Load()
    {
        if (!File.Exists(GetPath()))
        {
            Debug.LogWarning($"SystemData.json 파일이 없습니다.");
            return;
        }
        string json = File.ReadAllText(GetPath());

        SystemData SystemData_fromJson = JsonUtility.FromJson<SystemData>(json);
        SystemData = SystemData_fromJson;
    }
}

[Serializable]
public class SystemData
{
    public string Day;

    public float MouseSensitivity;

    public float MasterVolume;
    public float BGMVolume;
    public float SFXVolume;
}