using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Subtitle : MonoBehaviour
{
    [SerializeField] private Transform _iconTrans;
    [SerializeField] private Transform _bagTrans;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _bag;
    [SerializeField] private GameObject _tip;

    private TextMeshProUGUI _selfUGUI;
    private RectTransform _selfRTrans;
    private Color _originalIconColor;
    private Color _originalBagColor;
    private Vector3 _iconOriginPos;
    private readonly Vector3 _bagOffset = new Vector3(-205f, 0f, 0f);
    private bool _isAnimating = false;
    private string _currentText = "";
    private bool _isItem = false;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _selfUGUI = GetComponent<TextMeshProUGUI>();
        _selfRTrans = GetComponent<RectTransform>();

        // 원본 색상 저장
        _originalIconColor = _icon.color;
        _originalBagColor = _bag.color;

        // 초기 상태 설정
        _iconOriginPos = _iconTrans.position;
        ResetToInitialState();
    }

    private void ResetToInitialState()
    {
        // 텍스트 초기화
        _selfUGUI.text = "";

        // 위치 초기화
        _iconTrans.position = _iconOriginPos;
        _bagTrans.position = _iconOriginPos + _bagOffset;

        // 색상 초기화 (투명)
        SetAlpha(_icon, 0f);
        SetAlpha(_bag, 0f);

        // Tip 비활성화
        if (_tip != null)
            _tip.SetActive(false);
    }

    private void SetAlpha(Image image, float alpha)
    {
        Color color = image == _icon ? _originalIconColor : _originalBagColor;
        color.a = alpha;
        image.color = color;
    }

    public void SubPrint(string body)
    {
        if (string.IsNullOrEmpty(body)) return;

        // 진행 중인 애니메이션 중단
        StopCurrentAnimation();

        _currentText = body;
        _selfUGUI.text = body;

        // 텍스트 크기에 맞춰 UI 조정
        AdjustUIForText(body);
    }

    public void InitImage(Sprite interactedSprite)
    {
        if (interactedSprite != null)
        {
            _icon.sprite = interactedSprite;
        }

        // 텍스트가 설정되어 있다면 UI 재조정
        if (!string.IsNullOrEmpty(_currentText))
        {
            AdjustUIForText(_currentText);
        }
    }

    private void AdjustUIForText(string text)
    {
        // 텍스트 박스 크기 조정
        Vector2 newSize = _selfRTrans.sizeDelta;
        newSize.x = 20 * text.Length;
        _selfRTrans.sizeDelta = newSize;

        // 아이콘 위치 조정
        float moveX = -0.5f * newSize.x - 235f;
        Vector3 newIconPos = _iconOriginPos + new Vector3(moveX, 0f, 0f);
        Vector3 newBagPos = newIconPos + _bagOffset;

        _iconTrans.position = newIconPos;
        _bagTrans.position = newBagPos;
    }

    public void Switch(bool isItem)
    {
        if (string.IsNullOrEmpty(_currentText))
        {
            Debug.LogWarning("Switch() called but no text is set. Call SubPrint() first.");
            return;
        }

        _isItem = isItem;

        if (!_isAnimating)
        {
            StartCoroutine(ShowAndHideRoutine());
        }
    }

    private void StopCurrentAnimation()
    {
        if (_isAnimating)
        {
            StopAllCoroutines();
            DOTween.Kill(_iconTrans);
            DOTween.Kill(_selfRTrans);
            _isAnimating = false;
        }
        ResetToInitialState();
    }

    private IEnumerator ShowAndHideRoutine()
    {
        _isAnimating = true;

        // === 등장 애니메이션 ===
        yield return StartCoroutine(ShowAnimation());

        // === 대기 시간 ===
        float displayDuration = Mathf.Max(1f, _currentText.Length / 8f);
        yield return new WaitForSeconds(displayDuration);

        // === 퇴장 애니메이션 ===
        yield return StartCoroutine(HideAnimation());

        _isAnimating = false;
    }

    private IEnumerator ShowAnimation()
    {
        // Tip 활성화
        if (_tip != null)
            _tip.SetActive(true);

        // 아이콘 표시
        SetAlpha(_icon, 1f);

        if (_isItem)
        {
            // 아이템 모드: 가방과 아이콘이 함께 이동
            SetAlpha(_bag, 1f);

            Vector3 bagTargetPos = _iconTrans.position + _bagOffset;
            _bagTrans.position = bagTargetPos;

            // 아이콘 이동 애니메이션
            DOTween.Kill(_iconTrans);
            yield return _iconTrans.DOMove(bagTargetPos, 0.8f).SetEase(Ease.InOutQuad).WaitForCompletion();
        }
        else
        {
            // 일반 모드: 텍스트 박스가 위에서 내려옴
            SetAlpha(_bag, 0f);

            Vector3 startPos = _selfRTrans.position + new Vector3(0, 152f, 0);
            Vector3 targetPos = _selfRTrans.position;

            _selfRTrans.position = startPos;
            yield return _selfRTrans.DOMove(targetPos, 0.3f).SetEase(Ease.OutCubic).WaitForCompletion();
        }
    }

    private IEnumerator HideAnimation()
    {
        // 텍스트 제거
        _selfUGUI.text = "";

        // Tip 비활성화
        if (_tip != null)
            _tip.SetActive(false);

        // 색상 페이드아웃
        SetAlpha(_icon, 0f);
        if (_isItem)
            SetAlpha(_bag, 0f);

        // 위치 복구
        _iconTrans.position = _iconOriginPos;
        _bagTrans.position = _iconOriginPos + _bagOffset;

        yield return null; // 한 프레임 대기
    }
}