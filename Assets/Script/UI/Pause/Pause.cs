using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class Pause : MonoBehaviour
{

    public GameObject PausePanel;         // 퍼즈UI 화면
    public GameObject currentSubPanel;    // 현재 활성화된 서브화면 프리팹

    public float clickUp = 1.2f;          // 클릭 시 확대 비율
    public float duration = 0.15f;        // 애니메이션 시간

    private Vector3 originalScale = new Vector3(1f, 1f, 1f);        // 원래 크기


    private Dictionary<GameObject, Tween> buttonTweens = new();     // 버튼별 트윈 저장용 딕셔너리



    [Header("퍼즈 - 서브화면 버튼")]
    public GameObject SettingButton;
    public GameObject CollectionButton;
    public GameObject CharacterButton;

    [Header("퍼즈 - 서브화면")]
    public GameObject SettingPanel;
    public GameObject CollectionPanel;
    public GameObject CharacterPanel;
    /*
    public GameObject SettingPanelPrefab;
    public GameObject CollectionPanelPrefab;
    public GameObject CharacterPanelPrefab;

    private List<GameObject> allPanelPrefabs;
    */

    private List<GameObject> allSubPanel;
    private List<GameObject> allButtons;
    private List<GameObject> allsubButtons;     // 도감 서브 버튼 리스트
    void Start()
    {
        // 퍼즈 관련 리스트
        allSubPanel = new List<GameObject> { SettingPanel, CollectionPanel, CharacterPanel };
        allButtons = new List<GameObject> { SettingButton, CollectionButton, CharacterButton };


        // Time.timeScale = 0f; // 작동 테스트용!!
    }

    void OnEnable()
    {
        ClickButton(CharacterPanel, CharacterButton); // 활성화 될 때 캐릭터정보화면으로 초기화
    }

    // 버튼 클릭시 실행
    private void ClickButton(GameObject targetPanel, GameObject targetButton)
    {
        // 특정 정보창만 활성화
        if (currentSubPanel != null)
        {
            currentSubPanel.SetActive(false); // 기존 화면 비활성화
        }

        currentSubPanel = targetPanel;
        currentSubPanel.SetActive(true);
        // currentSubPanel = Instantiate(targetPanel, PausePanel.transform); // 새 서브창 생성


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
