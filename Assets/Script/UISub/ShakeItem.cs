using UnityEngine;
using DG.Tweening;

public class ShakeItem : MonoBehaviour
{
    [Header("흔들림 설정")]
    public float floatHeight = 20f;       // 위아래로 이동할 거리
    public float duration = 1.2f;         // 한 번 왔다 갔다 하는 데 걸리는 시간

    private Vector3 _originalPosition;

    void Start()
    {
        _originalPosition = transform.localPosition;

        // 위아래 반복 애니메이션
        transform.DOLocalMoveY(_originalPosition.y + floatHeight, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
