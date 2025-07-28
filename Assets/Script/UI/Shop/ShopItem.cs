using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ShopItem : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    GameDataManager data;

    [Header("아이템 정보")]
    public Image ItemIcon;             // 아이템 아이콘 이미지
    public int ItemPrice;              // 아이템 가격 
    public string MyName;              // 해당 버튼에 할당될 이름
    public string MyInfor;             // 해당 버튼에 할당될 정보
    public Sprite MySprite;            // 해당 버튼에 할당될 스프라이트

    [Header("텍스트 및 이미지 출력 창")]
    public Text ItemName;             // 호버한 아이템 이름 출력 텍스트박스
    public Text ItemInfor;            // 호버한 아이템 정보 출력 텍스트박스
    public Image ItemImage;           // 호버한 아이템 이미지 출력 이미지창

    [Header("애니메이션 관련")]
    public float hoverUp = 1.05f;          // 호버 시 확대 비율
    public float duration = 0.15f;        // 애니메이션 시간

    private Vector3 originalScale;        // 원래 크기
    private Tween currentTween;           // 현재 트윈

    void Start()
    {
        data = Singleton.GameManager_Instance.Get<GameDataManager>();
        originalScale = transform.localScale;        // 시작 크기 저장
    }
    public void ClickButton()
    {
        if (data.Money >= ItemPrice)
        {
            data.Money -= ItemPrice;
            ItemDeactivate();
        }
        else
        {
            // 흔들리는 애니메이션 추가
        }
    }

    // 아이템 클릭 활성화 상태
    public void ItemActivate()
    {
        ItemIcon.color = new Color(1f, 1f, 1f, 1f);
    }
    // 아이템 클릭 비활성화 상태
    public void ItemDeactivate()
    {
        ItemIcon.color = new Color(0.3f, 0.3f, 0.3f, 1f);
    }

    // 호버 관리
    public void OnPointerEnter(PointerEventData eventData)
    {
        currentTween?.Kill();
        currentTween = transform.DOScale(originalScale * hoverUp, duration).SetUpdate(true);

        // 해당 아이콘의 정보 보이기
        ItemName.text = MyName;
        ItemInfor.text = MyInfor;
        ItemImage.color = new Color(1f, 1f, 1f, 1f);
        ItemImage.sprite = MySprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 버튼 애니메이션 초기화
        currentTween?.Kill();
        currentTween = transform.DOScale(originalScale, duration).SetUpdate(true);

        // 커서를 옮길 시 정보 보이지 않게 하기
        ItemName.text = "";
        ItemInfor.text = "";
        ItemImage.color = new Color(1f, 1f, 1f, 0f);
    }
}
