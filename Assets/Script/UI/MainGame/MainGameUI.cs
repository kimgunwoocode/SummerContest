using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class MainGameUI : MonoBehaviour
{
    GameDataManager data;

    public Text MoneyText;          // 가진 돈 텍스트
    [Header("브레스 아이콘 관련")]
    public Image breatGaugeIcon_1;
    public Image breatGaugeIcon_2;
    public Image breatGaugeIcon_3;


    public Sprite nullsprite;           // 비어있는 칸 이미지
    public Sprite Dragonsprite;         // 첫번째 칸 비어있을 시 기본 이미지

    [Header("브레스 게이지 관련")]
    public Image breathGauge;        // 브레스 게이지 이미지
    public Image breathGauge_back;  // 브레스 게이지 뒷배경
    public Sprite[] gaugeSprites;   // 게이지 스프라이트 리스트, 스프라이트 차례로 추가 필요

    [Header("Hp 관련")]
    /*
    public GameObject heartPrefab;  // 하트 프리팹
    public Transform heartContainer;// 하트들이 자식으로 정렬될 부모 오브젝트
    */

    public Sprite fullHeart;        // 채워진 하트 스프라이트
    public Sprite halfHeart;        // 반만 채워진 하트 스프라이트
    public Sprite emptyHeart;       // 빈 하트 스프라이트

    // UI에서 따로 카운팅하는 체력 관련, 삭제가능
    private int UICurrentHP;
    private int UIMaxHP;
    public List<Image> heart_image = new(); // 하트 이미지 리스트

    void Awake()
    {
        data = Singleton.GameManager_Instance.Get<GameDataManager>();
        UICurrentHP = data.CurrentHP;
        UIMaxHP = data.MaxHP;

        MoneyText.text = data.Money.ToString(); // 돈 텍스트 초기화

        InitializeHP(UIMaxHP);
    }

    private void Start()
    {
        if(!data.PlayerAbility[1])
        {
            GetAbility_Breath(false);

        }
    }

    void Update()
    {
        MoneyText.text = data.Money.ToString(); // 돈 텍스트 초기화

        float fillAmount = data.CurrentBreathGauge / data.MaxBreathGauge;
        breathGauge.fillAmount = Mathf.Clamp01(fillAmount); // 게이지 UI 초기화


        // 임시 확인용 체력 변화 감지
        if (UICurrentHP != data.CurrentHP)
        {
            UICurrentHP = data.CurrentHP;
            UpdateHP(data.CurrentHP, data.MaxHP);
        }
        // 최대체력 갱신 감지시 호출
        if (UIMaxHP != data.MaxHP)
        {
            UIMaxHP = data.MaxHP;
            InitializeHP(data.MaxHP);
        }
    }

    // 장착 브레스 바뀌었을 시 호출
    public void BreathIconFix()
    {
        Image[] breathIcons = { breatGaugeIcon_1, breatGaugeIcon_2, breatGaugeIcon_3 };
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
    }

    // 최대브레스게이지 갱신시 호출
    public void InitializeBreathGauge()
    {
        // 아이템 번호 고추 조각으로 변경 필요!!!!!!! (수정 완료)
        breathGauge.sprite = gaugeSprites[data.GettedItems[1002] / 3];
    }
    public void GetAbility_Breath(bool active)
    {
        breathGauge.enabled = active;
        breathGauge_back.enabled = active;
        breatGaugeIcon_2.enabled = active;
        breatGaugeIcon_3.enabled = active;
    }

    // 최대체력 갱신시 호출
    public void InitializeHP(int maxHP)
    {
        // 전체 비활성
        foreach (Image child in heart_image)
        { child.gameObject.SetActive(false); }
        // 최대체력만큼 활성
        for (int i = 0; i < maxHP; i++)
        {
            if (i%2 == 1)
               heart_image[i/2].gameObject.SetActive(true);
        }

        // 스프라이트 초기화
        UpdateHP(data.CurrentHP, data.MaxHP);

        /*
            // 이전 하트들 제거
            foreach (Transform child in heartContainer)
            {
                Destroy(child.gameObject);
            }
            heartImages.Clear();

            // 최대 체력 수만큼 하트를 생성
            for (int i = 0; i < maxHP; i++)
            {
                GameObject heart = Instantiate(heartPrefab, heartContainer);
                heartImages.Add(heart.GetComponent<Image>());
            }
            */

    }

    // 체력 변동시 호출
    public void UpdateHP(int currentHP, int maxHP)
    {
        // MaxHP가 변경된 경우 리스트 길이 맞추기
        if (maxHP != UIMaxHP)
        {
            UIMaxHP = data.MaxHP;
            InitializeHP(data.MaxHP);
        }

        
        // currentHP가 0이면 모든 하트를 빈 스프라이트로 바꾼 후 리턴
        if (currentHP == 0)
        {
            for (int i = 0; i < data.MaxHP; i++)
            {
                heart_image[i].sprite = emptyHeart;
            }
            return;
        }
        // 체력 증감
        int halfheart_index = currentHP % 2 == 1 ? currentHP / 2 : currentHP / 2 - 1;

        if (currentHP % 2 == 1)
        {
            for (int i = 0; i < data.MaxHP/2; i++)
            {
                if (i < currentHP / 2)
                {
                    heart_image[i].sprite = fullHeart;
                }
                else if (i == currentHP / 2)
                {
                    heart_image[i].sprite = halfHeart;
                }
                else
                {
                    heart_image[i].sprite = emptyHeart;
                }
            }
        }
        else
        {
            for (int i = 0; i < data.MaxHP / 2; i++)
            {
                if (i < currentHP / 2)
                {
                    heart_image[i].sprite = fullHeart;
                }
                else
                {
                    heart_image[i].sprite = emptyHeart;
                }
            }
        }
    }

}
