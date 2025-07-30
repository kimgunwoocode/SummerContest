using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class ChracterPanel : MonoBehaviour
{
    GameDataManager data;

    [Header("장착 브레스 아이콘 관련")]
    public Image BreathIcon_1;
    public Image BreathIcon_2;
    public Image BreathIcon_3;

    public Sprite nullsprite;           // 비어있는 칸 이미지
    public Sprite Dragonsprite;         // 첫번째 칸 비어있을 시 기본 이미지

    [Header("Hp 관련")]
    public Image Dragonheart;       // 드래곤 하트 모은 정도
    public GameObject heartPrefab;  // 하트 프리팹
    public Transform heartContainer;// 하트들이 자식으로 정렬될 부모 오브젝트

    private List<Image> heartImages = new(); // 하트 이미지 리스트


    [Header("브레스 게이지 관련")]
    public Image Pepper;
    public Image BreathGauge;


    void Start()
    {
        Image[] breathIcons = { BreathIcon_1, BreathIcon_2, BreathIcon_3 };

        // 장착 브레스 아이콘 초기화
        for (int i = 0; i < 3; i++)
        {
            if (i < data.EquipSkill.Count)
            {
                // 스킬이 존재하면 해당 아이콘으로 설정
                breathIcons[i].sprite = data.allitems.allitems_dic[data.EquipSkill[i]].icon;
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

        // 최대 체력 수만큼 하트를 생성

        // 이전 하트들 제거
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }
        heartImages.Clear();

        // 최대 체력 수만큼 하트를 생성
        for (int i = 0; i < data.MaxHP; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            heartImages.Add(heart.GetComponent<Image>());
        }

        // 추가 필요 코드 

        // 모은 만큼 하트 조각 이미지 스프라이트 추가
        // 모은 만큼 고추 조각 이미지 스프라이트 추가
        // 브레스 게이지 크기 알맞는 스프라이트 출력

    }

}
