using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class TitleUI_Manager : MonoBehaviour
{
    [Header("Screen")]
    public GameObject SelectPanel_Screen;
    public Button[] SelectPanel_SaveFile;
    [Header("Text")]
    public TMP_Text[] SaveFileDate;

    private void Start()
    {
        SelectPanel_Screen.SetActive(false);
        int i = 1;
        foreach (Button screen in SelectPanel_SaveFile)
        {
            string path = SaveFileManager.GetPath(i);
            if (!File.Exists(path))
            {
                screen.interactable = false;
            }
            else
            {
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
