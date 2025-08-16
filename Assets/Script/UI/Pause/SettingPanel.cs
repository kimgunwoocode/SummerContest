using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingPanel : MonoBehaviour
{
    public UISoundManamger UISoundManamger;

    private void OnEnable()
    {
        UISoundManamger.SetSliderValue();
    }

    public void TitleButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }
}
