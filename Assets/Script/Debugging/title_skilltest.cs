using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class title_skilltest : MonoBehaviour
{
    public string scenename;
    public TextAsset InitData;

    public void func()
    {
        SaveFileManager.Load_forNewGame(InitData.text, 100);
        SceneManager.LoadScene(scenename);
    }
}
