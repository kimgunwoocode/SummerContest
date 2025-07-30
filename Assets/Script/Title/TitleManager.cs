using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("test")]
    public TMP_Text testText;
    [Space]
    public string StartSceneName;
    public TitleUI_Manager TitleUIManager;
    [Space]
    [Header("InitData")]
    public TextAsset InitData;
    public TextAsset SavePointID_json;
    GameManager GameManager;
    GameDataManager GameDataManager;

    int savefile_count = 3;

    Dictionary<int, string> SavePointID_list = new();


    private void Awake()
    {
        GameManager = Singleton.GameManager_Instance.Get<GameManager>();
        GameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();

        SavePointID_list = DictionaryFromJson(SavePointID_json.text);
    }


    public void ContinueSavedGame(int index)
    {
        SaveFileManager.Load(index);
        StartSceneName = SavePointID_list[GameDataManager.SpawnPoint];
        GameManager.CurrentScenePointID = -GameDataManager.SpawnPoint;
        SceneManager.LoadScene(StartSceneName);
    }

    public void StartNewGame()
    {
        int index = -1;
        for (int i = 0; i < TitleUIManager.isExistSaveFile.Count; i++)
        {
            Debug.Log(i);
            if (!TitleUIManager.isExistSaveFile[i])
            {
                Debug.Log("find");
                index = i+1;
                break;
            }
        }
        Debug.Log(index);
        if (index < 0)
        {
            // 세이브파일 슬롯이 가득찬 상황
            return;
        }
        else
            SaveFileManager.Load_forNewGame(InitData.text, index);


        StartSceneName = "1-1_ForgottenNest";
        GameManager.CurrentScenePointID = -1;
        SceneManager.LoadScene(StartSceneName);
    }

    public void deleteSaveFile(int slot)
    {
        SaveFileManager.deleteSaveFile(slot);
        TitleUIManager.SetSaveFileButton();
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public Dictionary<int, string> DictionaryFromJson(string SavePointID_json)
    {
        SavePointID_Wrapper savepointID_wrapper = JsonUtility.FromJson<SavePointID_Wrapper>(SavePointID_json);
        Dictionary<int, string> savepointID_list = new();
        foreach (var sp in savepointID_wrapper.savepoint_list)
        {
            savepointID_list[sp.ID] = sp.ScenName;
        }
        return savepointID_list;

    }

    [System.Serializable]
    private class SavePointID_Wrapper
    {
        public List<SavePoint_class> savepoint_list;
    }
    [System.Serializable]
    private class SavePoint_class
    {
        public int ID;
        public string ScenName;
    }
}
