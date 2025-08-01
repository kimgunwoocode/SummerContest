using UnityEngine;
using UnityEngine.SceneManagement;
public class SettingPanel : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {

    }

    public void TitleButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }
}
