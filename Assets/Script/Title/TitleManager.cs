using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public string StartSceneName;
    [Space]
    public InitSaveData InitData;
    public GameManager GameManager;
    public GameDataManager GameDataManager;



    private void Awake()
    {
        GameManager = Singleton.GameManager_Instance.Get<GameManager>();
        GameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
    }


    public void ContinueSavedGame(int index)
    {
        SaveFileManager.Load(index);
    }

    public void StartNewGame()
    {
        GameDataManager.LoadGameData(InitData.InitData);
        GameManager.LoadData__SavePoint();
        SceneManager.LoadScene(StartSceneName);
    }
}
