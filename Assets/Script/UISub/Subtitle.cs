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
    private Color _color;

    private Vector3 _iconOriginPos;
    private readonly Vector3 _bagOffset = new Vector3(-205f, 0f, 0f);

    private bool _coroutine = false;
    private float _prevTextWidth = 0f;
    private string _body;
    private bool _isTem = false;

    private void Start()
    {
        _selfUGUI = GetComponent<TextMeshProUGUI>();
        _selfRTrans = GetComponent<RectTransform>();
        _color = new Color(1, 1, 1, 0);
        _selfUGUI.text = null;

        _iconOriginPos = _iconTrans.position; // 기준 위치 저장
    }

    public void Switch(bool istem)
    {
        _isTem = istem;
        if (!_coroutine)
            StartCoroutine(OnOff());
    }

    IEnumerator OnOff()
    {
        _coroutine = true;

        _color.a = 1;
        if(_tip != null)
        _tip.SetActive(true);
        _icon.color = _color;

        if (_isTem)
        {
            _bag.color = _color;

            Vector3 bagTargetPos = _iconTrans.position + _bagOffset;
            _bagTrans.position = bagTargetPos;

            DOTween.Kill(_iconTrans);
            _iconTrans.DOMove(bagTargetPos, 0.8f).SetEase(Ease.InOutQuad);
        }
        else
        {
            _color.a = 0;
            _bag.color = _color;

            Vector3 startPos = _selfRTrans.position + new Vector3(0, 152f, 0);
            Vector3 targetPos = _selfRTrans.position;

            _selfRTrans.position = startPos;
            _selfRTrans.DOMove(targetPos, 0.3f).SetEase(Ease.OutCubic);
        }

        yield return new WaitForSeconds(1f / 8 * _selfUGUI.text.Length);

        _selfUGUI.text = null;

        _color.a = 0;
        if(_tip != null)
        _tip.SetActive(false);
        _icon.color = _color;
        if (_isTem) _bag.color = _color;

        // 위치 원복
        _iconTrans.position = _iconOriginPos;
        _bagTrans.position = _iconOriginPos + _bagOffset;

        _prevTextWidth = 0f;
        _coroutine = false;
    }

    public void SubPrint(string body)
    {
        _body = body;

        // 실행 중이면 중단하고 위치 복구
        if (_coroutine)
        {
            StopAllCoroutines();
            _iconTrans.position = _iconOriginPos;
            _bagTrans.position = _iconOriginPos + _bagOffset;
            _coroutine = false;
        }

        _selfUGUI.text = body;

        Vector2 newSize = _selfRTrans.sizeDelta;
        newSize.x = 20 * body.Length;
        _selfRTrans.sizeDelta = newSize;

        float moveX = -0.5f * newSize.x - 235f;
        Vector3 newIconPos = _iconOriginPos + new Vector3(moveX, 0f, 0f);

        Vector3 newBagPos = newIconPos + _bagOffset;
        newBagPos.z = newIconPos.z + 1f;
        _bagTrans.position = newBagPos;

        _prevTextWidth = -moveX;
    }

    public void InitImage(Sprite interactedSprite)
    {
        // 아이콘 위치 초기화
        _iconTrans.position = _iconOriginPos;
        _bagTrans.position = _iconOriginPos + _bagOffset;

        _selfUGUI.text = _body;

        Vector2 newSize = _selfRTrans.sizeDelta;
        newSize.x = 22 * _body.Length;
        _selfRTrans.sizeDelta = newSize;

        float moveX = -0.5f * newSize.x - 205f;
        Vector3 newIconPos = _iconOriginPos + new Vector3(moveX, 0f, 0f);
        _iconTrans.position = newIconPos;
        _bagTrans.position = newIconPos + _bagOffset;

        _prevTextWidth = -moveX;

        _icon.sprite = interactedSprite;
    }
}