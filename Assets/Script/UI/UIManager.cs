using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject PausePanel;         // 퍼즈UI 화면
    internal bool isPause;                // 퍼즈중인지 판별

    void Start()
    {
        PausePanel.SetActive(false);                    // 게임 시작시 퍼즈화면 비활성화 초기화
        isPause = false;                                // 게임 시작시 false로
    }


    // esc 누를 시 불러옴
    internal void Pausing()
    {

        if (isPause == true)
        {
            Time.timeScale = 1f;                      // 타임스케일

            PausePanel.SetActive(false);              // 퍼즈 UI 화면 비활성화
        }
        else
        {
            Time.timeScale = 0f;                      // 타임스케일

            PausePanel.SetActive(true);               // 퍼즈 UI 화면 활성화
        }
        isPause = !isPause;
    }


}
