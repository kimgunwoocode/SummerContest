using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
public class ChracterPanel : MonoBehaviour
{
    GameDataManager data;

    [Header("하트, 고추 플레이버 텍스트")]

    public TextMeshProUGUI Heart_Text;
    public TextMeshProUGUI Pepper_Text;


    [Header("장착 브레스 아이콘 관련")]
    public Image BreathIcon_1;
    public Image BreathIcon_2;
    public Image BreathIcon_3;

    public Sprite nullsprite;           // 비어있는 칸 이미지
    public Sprite Dragonsprite;         // 첫번째 칸 비어있을 시 기본 이미지

    [Header("Hp 관련")]
    public Image Dragonheart;       // 드래곤 하트 모은 정도 출력할 이미지
    public Sprite Dragonheart_0; // 비어있는 스프라이트
    public Sprite Dragonheart_1;    // 출력할 스프라이트 (1은 조각 1개, 2는 조각 2개. 3은 모든 조각 다 모으면 출력)
    public Sprite Dragonheart_2;
    public Sprite Dragonheart_3;
    /*
    public GameObject heartPrefab;  // 하트 프리팹
    public Transform heartContainer;// 하트들이 자식으로 정렬될 부모 오브젝트
    */

    public List<GameObject> heartImages = new(); // 하트 이미지 리스트

    [Header("브레스 게이지 관련")]
    public Image Pepper;
    public Sprite Pepper_0;
    public Sprite Pepper_1;    // 출력할 스프라이트 (1은 조각 1개, 2는 조각 2개. 3은 모든 조각 다 모으면 출력)
    public Sprite Pepper_2;
    public Sprite Pepper_3;
    public Image BreathGauge;
    public Sprite[] gaugeSprites;   // 게이지 스프라이트 리스트, 스프라이트 차례로 추가 필요



    void OnEnable()
    {
        data = Singleton.GameManager_Instance.Get<GameDataManager>();
        Image[] breathIcons = { BreathIcon_1, BreathIcon_2, BreathIcon_3 };

        // 플레이버 텍스트 초기화
        // 텍스트 수정 필요
        switch (data.GettedItems[1001] / 3)
        {
            case 0:
                Heart_Text.text = "어쩌구";
                break;
            case 1:
                Heart_Text.text = "저쩌구";
                break;
            case 2:
                Heart_Text.text = "그쩌구";
                break;
            default:
                Heart_Text.text = "???";
                break;
        }

        switch (data.GettedItems[1002] / 3)
        {
            case 0:
                Pepper_Text.text = "어쩌구";
                break;
            case 1:
                Pepper_Text.text = "저쩌구";
                break;
            case 2:
                Pepper_Text.text = "그쩌구";
                break;
            default:
                Pepper_Text.text = "???";
                break;
        }

        // 장착 브레스 아이콘 초기화
        for (int i = 0; i < 3; i++)
        {
            if (i < data.EquipSkill.Count)
            {
                // 스킬이 존재하면 해당 아이콘으로 설정
                breathIcons[i].sprite = data.allitems[data.EquipSkill[i]].icon;
            }
            else if (i == 0)
            {
                breathIcons[i].sprite = Dragonsprite;
            }
            else
            {
                breathIcons[i].sprite = nullsprite;
            }
        }
        /*
                // 최대 체력 수만큼 하트를 생성
                // 이전 하트들 제거
                foreach (Transform child in heartContainer)
                {
                    Debug.Log("하트 프리팹 제거");
                    Destroy(child.gameObject);
                }

                // 최대 체력 수만큼 하트를 생성
                for (int i = 0; i < data.MaxHP; i++)
                {
                    Debug.Log("하트 프리팹 생성");
                    GameObject heart = Instantiate(heartPrefab, heartContainer);
                }
        */

        // 전체 비활성
        foreach (GameObject child in heartImages)
        { child.gameObject.SetActive(false); }
        // 최대체력만큼 활성
        for (int i = 0; i < data.MaxHP / 2; i++)
        {
            heartImages[i].gameObject.SetActive(true);
        }

        // 모은 만큼 하트 조각 이미지 스프라이트 추가
        // 아이템 번호 수정 필요!!! (수정 완료)
        // 최종적으로 다 모았을 시 몇개인지 나오면 첫 조건문 몇개일 시 Dragonheart_3으로 변경 가능
        if (data.GettedItems[1001] % 3 == 0)
        {
            Dragonheart.sprite = Dragonheart_0;
        }
        else if (data.GettedItems[1001] % 3 == 1)
        {
            Dragonheart.sprite = Dragonheart_1;
        }
        else if (data.GettedItems[1001] % 3 == 2)
        {
            Dragonheart.sprite = Dragonheart_2;
        }


        // 모은 만큼 고추 조각 이미지 스프라이트 추가
        // 아이템 번호 수정 필요!!!!!!! (수정 완료)
        if (data.GettedItems[1002] % 3 == 0)
        {
            Pepper.sprite = Pepper_0;
        }
        else if (data.GettedItems[1002] % 3 == 1)
        {
            Pepper.sprite = Pepper_1;
        }
        else if (data.GettedItems[1002] % 3 == 2)
        {
            Pepper.sprite = Pepper_2;
        }


        // 브레스 게이지 크기 알맞는 스프라이트 출력
        // 아이템 번호 수정 필요!!!! (수정 완료)
        int index = data.GettedItems[1002] / 3;
        BreathGauge.sprite = gaugeSprites[index];


    }

}
