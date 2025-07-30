using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("Screen")]
    public GameObject SelectPanel_Screen;
    public Button[] SelectPanel_SaveFile;
    public Button[] DeleteSaveFile;
    public GameObject Warnning_Screen;
    [HideInInspector] public List<bool> isExistSaveFile;
    [Header("Text")]
    public TMP_Text[] SaveFileDate;

    [Space(30)]

    [Header("test")]
    public TMP_Text testText;
    [Space]
    public string StartSceneName;
    [Space]
    [Header("InitData")]
    public TextAsset InitData;
    public TextAsset SavePointID_json;
    GameManager GameManager;
    GameDataManager GameDataManager;

    int selected_index = -1;

    Dictionary<int, string> SavePointID_list = new();


    private void Awake()
    {
        GameManager = Singleton.GameManager_Instance.Get<GameManager>();
        GameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();

        SavePointID_list = DictionaryFromJson(SavePointID_json.text);
    }

    private void Start()
    {
        SelectPanel_Screen.SetActive(false);
        Warnning_Screen.SetActive(false);
        SetSaveFileButton();
    }

    #region UI
    public void SetSaveFileButton()
    {
        int i = 1;
        foreach (Button screen in SelectPanel_SaveFile)
        {
            string path = SaveFileManager.GetPath(i);
            if (!File.Exists(path))
            {
                isExistSaveFile.Add(false);
                screen.interactable = false;
                DeleteSaveFile[i-1].interactable = false;
            }
            else
            {
                isExistSaveFile.Add(true);
                string json = File.ReadAllText(path);
                SerializableSaveData serializable = JsonUtility.FromJson<SerializableSaveData>(json);
                SaveFileDate[i].text = serializable.Day;
            }
            i++;
        }
    }

    public void Open_SelectPanel_Screen()
    {
        SelectPanel_Screen.SetActive(true);
    }

    public void Closs_SelectPanel_Screen()
    {
        SelectPanel_Screen.SetActive(false);
    }

    public void Open_warnning_screen(int index)
    {
        selected_index = index;
        Warnning_Screen.SetActive(true);
    }
    public void Closs_warnning_screen()
    {
        Warnning_Screen.SetActive(false);
    }
    #endregion

    public void ContinueSavedGame(int index)
    {
        SaveFileManager.Load(index);
        StartSceneName = SavePointID_list[GameDataManager.SpawnPoint];
        GameManager.CurrentScenePointID = -GameDataManager.SpawnPoint;

        Debug.Log(InitData.text);

        SceneManager.LoadScene(StartSceneName);
    }

    public void StartNewGame()
    {
        int index = -1;
        for (int i = 0; i < isExistSaveFile.Count; i++)
        {
            //Debug.Log(i);
            if (!isExistSaveFile[i])
            {
                //Debug.Log("find");
                index = i+1;
                break;
            }
        }
        Debug.Log(index);
        if (index < 0)
        {
            // 세이브파일 슬롯이 가득찬 상황
            Debug.Log("슬롯 가득참");
            return;
        }
        else
            SaveFileManager.Load_forNewGame(InitData.text, index);


        Debug.Log(GameDataManager.GameData.Slot + " 번 슬롯  --> \n" + InitData.text);


        StartSceneName = "1-1_ForgottenNest";
        GameManager.CurrentScenePointID = -1;
        SceneManager.LoadScene(StartSceneName);
    }

    public void deleteSaveFile()
    {
        SaveFileManager.deleteSaveFile(selected_index);
        SetSaveFileButton();
        Closs_warnning_screen();
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
