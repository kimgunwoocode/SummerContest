using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
public class CollectionPanel : MonoBehaviour
{

    public float clickUp = 1.1f;          // 클릭 시 확대 비율
    public float duration = 0.15f;        // 애니메이션 시간

    private Vector3 originalScale = new Vector3(1f, 1f, 1f);        // 원래 크기


    [Header("퍼즈 - 도감 - 서브화면 버튼")]
    public GameObject EnemyButton;
    public GameObject ItemButton;
    public GameObject DocumentButton;


    private List<GameObject> allsubButtons;     // 도감 서브 버튼 리스트
    /*
        [Header("텍스트 및 이미지 출력 창")]
        public Text itemNameText;               // 아이템 이름 출력 텍스트박스
        public Text itemInforText;              // 아이템 정보 출력 텍스트박스
        public Image itemImageImage;             // 아이템 이미지 출력 이미지창
    */

    [Header("버튼 클릭시 나타나는 좌측 버튼 관련")]
    public GameObject ButtonContainer;         // 도감 좌측페이지 버튼을 담는 컨테이너

    public List<GameObject> CollectiopnButtons = new(); // 좌측 버튼 리스트

    private Dictionary<GameObject, Tween> buttonTweens = new();     // 버튼별 트윈 저장용 딕셔너리


    void OnEnable()
    {
        allsubButtons = new List<GameObject> { EnemyButton, ItemButton, DocumentButton };

        ClickButton(EnemyButton); // 활성화 될 때 적화면으로 초기화

    }


    // 서브 탭(적, 아이템, 문서) 클릭시 실행
    private void ClickButton(GameObject targetButton)
    {
        // 11개 씩 나타나는, 페이지를 넘기거나 하는 추가적인 수정 있을 예정. 고려하여 작성함

        foreach (GameObject child in CollectiopnButtons)
        {
            CollectionButton codexBtn = child.GetComponent<CollectionButton>();

            for (int i = 0; i < 11; i++)
            {
                // 구체적인 수치는 변경 필요!!
                if (targetButton == EnemyButton) // 적 ID 라인으로 변경 필요
                {
                    codexBtn.MyId = 1000 + i;
                }
                else if (targetButton == ItemButton) // 아이템 ID 수정 완료, 아이템 수 추가될 시 변경 필요
                {
                    codexBtn.MyId = 1017 + i;
                    if (i == 5) break;
                }
                else if (targetButton == DocumentButton) // 문서 ID 라인으로 변경 필요
                {
                    codexBtn.MyId = 3000 + i;
                }
            }
        }
        /*
            // 이전 자식 버튼 제거
            foreach (Transform child in ButtonContainer)
            {
                Destroy(child.gameObject);
            }

            // 버튼 프리팹 생성
            for (int i = 0; i < 11; i++)
            {
                GameObject Colletionbutton = Instantiate(buttonPrefab, ButtonContainer);

                CollectionButton codexBtn = Colletionbutton.GetComponent<CollectionButton>();

                // 버튼에서의 연결
                codexBtn.ItemName = itemNameText;
                codexBtn.ItemInfor = itemInforText;
                codexBtn.ItemImage = itemImageImage;

                // 구체적인 수치는 변경 필요!!
                if (targetButton == EnemyButton) // 적 ID 라인으로 변경 필요
                {
                    codexBtn.MyId = 1000 + i;
                }
                else if (targetButton == ItemButton) // 아이템 ID 수정 완료, 아이템 수 추가될 시 변경 필요
                {
                    codexBtn.MyId = 1017 + i;
                    if (i == 5) break;
                }
                else if (targetButton == DocumentButton) // 문서 ID 라인으로 변경 필요
                {
                    codexBtn.MyId = 3000 + i;
                }

            }*/


        // 누른 버튼의 크기 고정, 나머지 버튼 초기화
        foreach (var button in allsubButtons)
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
    public void OnEnemyButtonClicked()
    {
        ClickButton(EnemyButton);
    }

    public void OnItemButtonClicked()
    {
        ClickButton(ItemButton);
    }

    public void OnDocumentButtonClicked()
    {
        ClickButton(DocumentButton);
    }


}
