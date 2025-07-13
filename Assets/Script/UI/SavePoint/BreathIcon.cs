using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BreathIcon : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{


    public float hoverUp = 1.1f;          // 호버 시 확대 비율
    public float duration = 0.15f;        // 애니메이션 시간

    private Vector3 originalScale;        // 원래 크기

    private Tween currentTween;           // 현재 트윈




    void Start()
    {
        originalScale = transform.localScale;        // 시작 크기 저장
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //확대 말고 후광? 같은걸로 수정하면 좋을듯
        currentTween?.Kill();
        currentTween = transform.DOScale(originalScale * hoverUp, duration).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentTween?.Kill();
        currentTween = transform.DOScale(originalScale, duration).SetUpdate(true);
    }
}
