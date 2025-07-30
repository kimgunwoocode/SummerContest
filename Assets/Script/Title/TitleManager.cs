using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class TitleManager : MonoBehaviour
{
    public string StartSceneName;
    [Space]
    [Header("InitData")]
    public TextAsset InitData;
    public TextAsset SavePointID_json;
    GameManager GameManager;
    GameDataManager GameDataManager;

    Dictionary<int, string> SavePointID_list = new();


    private void Awake()
    {
        GameManager = Singleton.GameManager_Instance.Get<GameManager>();
        GameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();

        SavePointID_list = MapTool.DictionaryFromJson(SavePointID_json.text);
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
        SceneManager.LoadScene(StartSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
