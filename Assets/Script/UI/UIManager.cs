using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    internal bool isPause;                // 퍼즈중인지 판별

    public GameObject PausePanel;         // 퍼즈UI 화면
    public GameObject MainGamePanel;     // 메인게임 UI 화면
    public GameObject SavePointPanel;    // 세이브 포인트 UI 화면
    public GameObject ShopPanel;         // 상점 UI 화면
    public GameObject CloseButton;         // 창 닫기 제어하는 버튼

    /*
    public GameObject MainGamePrefab;     // 메인게임 UI 프리팹
    public GameObject SavePointPrefab;    // 세이브 포인트 UI 프리팹
    public GameObject ShopPrefab;         // 상점 UI 프리팹
*/

    // public Transform ActivePanel;        // 활성화 된 창 생성 위치
    private GameObject ActivePanel = null;       // 활성화된 창

    void Start()
    {
        // 게임 시작시 활성화 등 초기화
        PausePanel.SetActive(false);
        SavePointPanel.SetActive(false);
        ShopPanel.SetActive(false);
        MainGamePanel.SetActive(true);
        CloseButton.SetActive(false);
        ActivePanel = null;

        isPause = false;                                // 게임 시작시 false로
    }

    

    // esc 누를 시 불러옴
    internal void Pausing()
    {
        // 타임 스케일 조절 여부를 다른 창이 활성화 되어 있는지로 판단
        if (ActivePanel == null)
        {
            if (PausePanel.activeSelf)
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
        else
        {
            if (PausePanel.activeSelf)
            {
                PausePanel.SetActive(false);              // 퍼즈 UI 화면 비활성화
            }
            else
            {
                PausePanel.SetActive(true);               // 퍼즈 UI 화면 활성화
            }
        }
    }

    // 세이브 포인트에 상호작용시 세이브포인트판넬 생성
    public void EnterSavePoint()
    {
        Time.timeScale = 0f;

        SavePointPanel.SetActive(true);
        CloseButton.SetActive(true);

        ActivePanel = SavePointPanel;
        // ActivePanelPrefab = Instantiate(SavePointPrefab, ActivePanel); // 세이브포인트 창 생성
    }

    // 상점 상호작용 시 상점 창 생성
    public void EnterShop()
    {
        Time.timeScale = 0f;

        ShopPanel.SetActive(true);
        CloseButton.SetActive(true);

        ActivePanel = ShopPanel;
        // ActivePanelPrefab = Instantiate(ShopPrefab, ActivePanel); // 상점 창 생성
    }

    // 현재 활성화된 추가 창을 닫을 시 호출
    public void ExitPanel()
    {
        ActivePanel.SetActive(false);
        CloseButton.SetActive(false);

        ActivePanel = null;

        Time.timeScale = 1f;
    }
}
