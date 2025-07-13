using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class Pause : MonoBehaviour
{
    public GameObject PausePanel;         // 퍼즈UI 화면
    internal bool isPause;                // 퍼즈중인지 판별

    public float clickUp = 1.3f;          // 클릭 시 확대 비율
    public float duration = 0.15f;        // 애니메이션 시간

    private Vector3 originalScale = new Vector3(1f, 1f, 1f);        // 원래 크기


    private Dictionary<GameObject, Tween> buttonTweens = new();     // 버튼별 트윈 저장용 딕셔너리



    [Header("퍼즈 화면 버튼")]
    public GameObject SettingButton;
    public GameObject CollectionButton;
    public GameObject CharacterButton;
    [Header("퍼즈 화면 정보창")]
    public GameObject SettingPanel;
    public GameObject CollectionPanel;
    public GameObject CharacterPanel;

    private List<GameObject> allPanels;
    private List<GameObject> allButtons;
    void Start()
    {
        // 퍼즈 관련 리스트
        allPanels = new List<GameObject> { SettingPanel, CollectionPanel, CharacterPanel };
        allButtons = new List<GameObject> { SettingButton, CollectionButton, CharacterButton };


        // Time.timeScale = 0f; // 작동 테스트용!!
        PausePanel.SetActive(false);                    // 게임 시작시 퍼즈화면 비활성화 초기화
        isPause = false;                                // 게임 시작시 false로
        ClickButton(CharacterPanel, CharacterButton);   // 게임 시작시 캐릭터정보화면 활성화로 초기화


    }


    void Update()
    {
        // esc 클릭
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("클릭됨");
            Pausing();
        }

    }

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
            ClickButton(CharacterPanel, CharacterButton); // 캐릭터 정보창 보이게 초기화
        }
        isPause = !isPause;
    }


    // 버튼 클릭시 실행
    private void ClickButton(GameObject targetPanel, GameObject targetButton)
    {
        // 특정 정보창만 활성화
        foreach (var panel in allPanels)
        {
            panel.SetActive(panel == targetPanel);
        }

        // 누른 버튼의 크기 고정, 나머지 버튼 초기화
        foreach (var button in allButtons)
        {
            // 클릭 여부 설정
            var pauseScript = button.GetComponent<PauseButton>();
            pauseScript.isclick = (button == targetButton);

            var btnTransform = button.transform;

            // 현재 실행중인 트윈 있을시 중단
            if (buttonTweens.TryGetValue(button, out Tween existingTween))
            {
                existingTween.Kill();
            }

            Tween newTween;                 // 새 트윈 생성
            // 버튼에 따라 맞는 애니메이션 실행
            if (button == targetButton)
            {
                newTween = btnTransform.DOScale(originalScale * clickUp, duration).SetUpdate(true);
            }
            else
            {
                newTween = btnTransform.DOScale(originalScale, duration).SetUpdate(true);
            }

            buttonTweens[button] = newTween; // 트윈 할당
        }
    }

    // 각 버튼의 On Click()에 참조
    public void OnSettingButtonClicked()
    {
        ClickButton(SettingPanel, SettingButton);
    }

    public void OnCollectionButtonClicked()
    {
        ClickButton(CollectionPanel, CollectionButton);
    }

    public void OnCharacterButtonClicked()
    {
        ClickButton(CharacterPanel, CharacterButton);
    }
}
