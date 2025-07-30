using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class TitleManager : MonoBehaviour
{
    public string StartSceneName;
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
        SaveFileManager.Load(0);
        StartSceneName = "1-1_ForgottenNest";
        GameManager.CurrentScenePointID = -1;
        for (int i=1;i<=savefile_count;i++)
        {
            string path = SaveFileManager.GetPath(i);
            if (!File.Exists(path))
            {
                SaveFileManager.Save(GameDataManager.GameData,i);
            }
            if (i == savefile_count)
            {
                // 모든 세이브칸이 꽉 찼음. 새로운 게임을 시작하려면 공간을 확보해야함
            }
        }
        SceneManager.LoadScene(StartSceneName);
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
