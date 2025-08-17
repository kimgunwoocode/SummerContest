using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Singleton : MonoBehaviour
{
    public TextAsset initdata;
    public static Singleton GameManager_Instance { get; private set; }
    [SerializeField] List<Component> scriptEntries = new();
    private Dictionary<Type, Component> scriptMap = new();

    private void Awake() {
        if (GameManager_Instance == null) {
            GameManager_Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildDictionary();
            GameDataManager GameDataManager = Get<GameDataManager>();
            if (GameDataManager.GameData == null && SceneManager.GetActiveScene().name != "Title")
            {
                SaveFileManager.Load_forNewGame(initdata.text, 100);
            }
        }

        else if (GameManager_Instance != this) {
            Destroy(gameObject);
        }
    }

    public T Get<T>() where T : Component {
        if (scriptMap.TryGetValue(typeof(T), out var comp))
            return comp as T;

        Debug.LogWarning($"{typeof(T).Name}이 등록되지 않았습니다.");
        return null;
    }


    private void BuildDictionary() {
        foreach (var entry in scriptEntries) {
            if (entry != null) {
                var type = entry.GetType();
                if (!scriptMap.ContainsKey(type))
                    scriptMap[type] = entry;
                else
                    Debug.LogWarning($"중복된 타입 등록: {type.Name}");
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("ScriptEntries 채우기")]
    private void FillScriptEntriesEditor()
    {
        FillScriptEntries();
    }
#endif
    public void FillScriptEntries()
    {
        Component[] allComponents = GetComponents<Component>();
        scriptEntries.Clear();

        foreach (var comp in allComponents)
        {
            if (comp is Transform || comp is Singleton) continue;

            if (!scriptEntries.Contains(comp))
            {
                scriptEntries.Add(comp);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        //Debug.Log("GameManager 오브젝트 파괴됨");
    }
#endif
}