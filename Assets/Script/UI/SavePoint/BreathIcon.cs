using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class BreathIcon : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    GameDataManager data;

    [Header("해당 버튼의 정보")]
    public int MyId;                      // 해당 버튼에 할당된 장비의 아이디 번호
    public string MyName;                 // 해당 버튼에 할당될 이름
    public string MyInfor;                // 해당 버튼에 할당될 정보
    public Sprite MySprite;               // 해당 버튼에 할당될 스프라이트

    [Header("텍스트 및 이미지 출력 창")]
    public Text BreathName;               // 호버한 브레스 이름 출력 텍스트박스
    public Text BreathInfor;              // 호버한 브레스 정보 출력 텍스트박스
    public Image BreathImage;             // 호버한 브레스 이미지 출력 이미지창

    [Header("장착 정보")]
    public Image SetImage_1;              // 슬롯 및 슬롯 리스트
    public Image SetImage_2;
    public Image SetImage_3;

    private Image[] SetImages = new Image[3];

    [Header("애니메이션 관련")]
    public float hoverUp = 1.1f;          // 호버 시 확대 비율
    public float duration = 0.15f;        // 애니메이션 시간

    private Vector3 originalScale;        // 원래 크기
    private Tween currentTween;           // 현재 트윈


    void Start()
    {
        data = Singleton.GameManager_Instance.Get<GameDataManager>();
        originalScale = transform.localScale;        // 시작 크기 저장
        // 이미지 리스트
        SetImages[0] = SetImage_1;
        SetImages[1] = SetImage_2;
        SetImages[2] = SetImage_3;
    }
    public void ClickButton()
    {
        if (data.PlayerSkill.TryGetValue(MyId, out bool isUnlocked) && isUnlocked)
        {
            if (data.EquipSkill.Contains(MyId))              // 이미 장착 중이면 해제
            {
                Unequip();
            }
            else if (data.EquipSkill.Count >= 3) return;    // 슬롯 꽉 찼으면 리턴
            else Equip();
        }
    }

    public void Equip()
    {
        // 비어있는 슬롯 찾기
        for (int i = 0; i < SetImages.Length; i++)
        {
            if (SetImages[i].sprite == null)
            {
                SetImages[i].sprite = MySprite;
                data.EquipSkill.Add(MyId);  // 장착 정보 게임매니저한테 전달
                break;
            }
        }
    }
    public void Unequip()
    {
        data.EquipSkill.Remove(MyId);       // 장착 해제 정보 게임매니저한테 전달

        // 본인의 스프라이트를 장착한 슬롯 찾아서 제거
        for (int i = 0; i < SetImages.Length; i++)
        {
            if (SetImages[i].sprite == MySprite)
            {
                SetImages[i].sprite = null; // 빈 슬롯 이미지 스프라이트로 대체 필요
                break;
            }
        }

        ReorderSlots();                     // 슬롯 재정렬

    }

    // 슬롯 재정렬
    public void ReorderSlots()
    {
        // 빈 슬롯 찾고 앞으로 당김
        for (int i = 0; i < SetImages.Length - 1; i++)
        {
            if (SetImages[i].sprite == null)
            {
                for (int j = i + 1; j < SetImages.Length; j++)
                {
                    if (SetImages[j].sprite != null)
                    {
                        SetImages[i].sprite = SetImages[j].sprite;
                        SetImages[j].sprite = null;
                        break;
                    }
                }
            }
        }
    }
    // 호버 관리
    public void OnPointerEnter(PointerEventData eventData)
    {
        //확대 말고 후광? 같은걸로 수정해도 좋을듯
        currentTween?.Kill();
        currentTween = transform.DOScale(originalScale * hoverUp, duration).SetUpdate(true);

        // 해당 아이콘의 정보 보이기
        // 장비 얻었는지 여부에 따라 표기가 다름
        if (data.PlayerSkill.TryGetValue(MyId, out bool isUnlocked) && isUnlocked)
        {
            BreathName.text = MyName;
            BreathInfor.text = MyInfor;
            BreathImage.color = new Color(1f, 1f, 1f, 1f);
            BreathImage.sprite = MySprite;
        }
        else
        {
            BreathName.text = "???";
            BreathInfor.text = "???";
            BreathImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            BreathImage.sprite = MySprite;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 버튼 애니메이션 초기화
        currentTween?.Kill();
        currentTween = transform.DOScale(originalScale, duration).SetUpdate(true);

        // 커서를 옮길 시 정보 보이지 않게 하기
        BreathName.text = "";
        BreathInfor.text = "";
        BreathImage.color = new Color(1f, 1f, 1f, 0f);
    }
}
