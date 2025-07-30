using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class TitleUI_Manager : MonoBehaviour
{
    [Header("Screen")]
    public GameObject SelectPanel_Screen;
    public Button[] SelectPanel_SaveFile;
    [HideInInspector] public List<bool> isExistSaveFile;
    [Header("Text")]
    public TMP_Text[] SaveFileDate;

    private void Start()
    {
        SelectPanel_Screen.SetActive(false);
        SetSaveFileButton();
    }

    public void SetSaveFileButton()
    {
        int i = 1;
        foreach (Button screen in SelectPanel_SaveFile)
        {
            string path = SaveFileManager.GetPath(i);
            if (!File.Exists(path))
            {
                isExistSaveFile.Add(true);
                screen.interactable = false;
            }
            else
            {
                isExistSaveFile.Add(false);
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
}
