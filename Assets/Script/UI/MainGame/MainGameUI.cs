using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class MainGameUI : MonoBehaviour
{
    GameDataManager data;

    public Image breatGauge;        // 브레스 게이지 이미지
    public Text MoneyText;          // 가진 돈 텍스트

    [Header("Hp 관련")]
    public GameObject heartPrefab;  // 하트 프리팹
    public Transform heartContainer;// 하트들이 자식으로 정렬될 부모 오브젝트

    public Sprite fullHeart;        // 채워진 하트 스프라이트
    public Sprite emptyHeart;       // 빈 하트 스프라이트

    // UI에서 따로 카운팅하는 체력 관련
    private int UICurrentHP;
    private int UIMaxHP;
    private List<Image> heartImages = new(); // 하트 이미지 리스트

    void Start()
    {
        data = Singleton.GameManager_Instance.Get<GameDataManager>();
        UICurrentHP = data.CurrentHP;
        UIMaxHP = data.MaxHP;

        MoneyText.text = data.Money.ToString(); // 돈 텍스트 초기화
    }

    void Update()
    {
        MoneyText.text = data.Money.ToString(); // 돈 텍스트 초기화

        float fillAmount = data.CurrentBreathGauge / data.MaxBreathGauge;
        breatGauge.fillAmount = Mathf.Clamp01(fillAmount); // 게이지 UI 초기화


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


    // 최대체력 갱신시 호출
    public void InitializeHP(int maxHP)
    {
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
        // 스프라이트 초기화
        UpdateHP(data.CurrentHP, data.MaxHP);
    }

    // 체력 변동시 호출
    public void UpdateHP(int currentHP, int maxHP)
    {
        // MaxHP가 변경된 경우 리스트 길이 맞추기
        if (maxHP != heartImages.Count)
        {
            InitializeHP(maxHP);
        }
        // currentHP가 0이면 모든 하트를 빈 스프라이트로 바꾼 후 리턴
        if (currentHP == 0)
        {
            for (int i = 0; i < heartImages.Count; i++)
            {
                heartImages[i].sprite = emptyHeart;
            }
            return;
        }
        // 체력 증감
        for (int i = 0; i < heartImages.Count; i++) // i = 인덱스번호
        {
            if (i > currentHP - 1)
            {
                heartImages[i].sprite = emptyHeart; // 체력 없음 스프라이트
            }
            else
            {
                heartImages[i].sprite = fullHeart;  // 체력 있음 스프라이트
            }
        }
    }

}
