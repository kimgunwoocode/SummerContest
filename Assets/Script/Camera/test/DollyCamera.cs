using UnityEngine;
using UnityEngine.InputSystem;

public class DollyCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _followSpeed = 5f;
    private float _initialS;
    [SerializeField] private float _maxOffsetDistance = 2f;
    [SerializeField] private float _mouseInfluenceRange = 3f;
    [SerializeField] private Vector2 _deadZoneSize = new Vector2(2f, 1f);

    [Header("Stage Bound Areas")]
    [SerializeField] private Transform[] _minBoundObjects;  // 좌하단 오브젝트들
    [SerializeField] private Transform[] _maxBoundObjects;  // 우상단 오브젝트들

    [SerializeField] private int _stageIndex = 0;  // 진척도 인덱스

    private Vector2 _minBounds;
    private Vector2 _maxBounds;

    private Camera _cam;
    private Vector3 _offset;
    Vector3 directionalOffset = Vector3.zero;

    private void Awake()
    {
        _initialS = _followSpeed;
        _cam = Camera.main;
        _offset = new Vector3(0f, 0f, -10f);

        if (_target == null)
        {
            Debug.LogWarning("DollyCamera: Target not assigned.");
        }

        UpdateBoundsFromStage();
    }

    private void FixedUpdate()
    {
        if (_target == null || Mouse.current == null) return;

        // 진척도 인덱스에 따른 바운드 갱신
        UpdateBoundsFromStage();

        Vector3 playerPosition = _target.position;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = _cam.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0f));
        mouseWorldPosition.z = 0f;

        Vector3 toMouse = mouseWorldPosition - playerPosition;
        float absX = Mathf.Abs(toMouse.x);
        float absY = Mathf.Abs(toMouse.y);
        float distance = toMouse.magnitude;

        Vector3 directionalOffset = Vector3.zero;
        if (absX > _deadZoneSize.x || absY > _deadZoneSize.y)
        {
            float t = Mathf.Clamp01((distance - 0.5f) / _mouseInfluenceRange);
            directionalOffset = toMouse.normalized * _maxOffsetDistance * t;
        }

        Vector3 targetPosition = new Vector3(
            playerPosition.x + directionalOffset.x,
            playerPosition.y + directionalOffset.y,
            _offset.z
        );

        Vector2 cameraSize = GetCameraWorldSize();
        float clampedX = Mathf.Clamp(targetPosition.x, _minBounds.x + cameraSize.x / 2f, _maxBounds.x - cameraSize.x / 2f);
        float clampedY = Mathf.Clamp(targetPosition.y, _minBounds.y + cameraSize.y / 2f, _maxBounds.y - cameraSize.y / 2f);
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, _offset.z);

        transform.position = Vector3.Lerp(transform.position, clampedPosition, _followSpeed * Time.fixedDeltaTime);
    }

    private void UpdateBoundsFromStage()
    {
        if (_stageIndex < 0 || _stageIndex >= _minBoundObjects.Length || _stageIndex >= _maxBoundObjects.Length)
        {
            Debug.LogWarning($"Invalid stage index: {_stageIndex}");
            return;
        }

        _minBounds = _minBoundObjects[_stageIndex].position;
        _maxBounds = _maxBoundObjects[_stageIndex].position;
    }

    private Vector2 GetCameraWorldSize()
    {
        float height = _cam.orthographicSize * 2f;
        float width = height * _cam.aspect;
        return new Vector2(width, height);
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    public void SetStageIndex(int index)
    {
        _stageIndex = index;
        UpdateBoundsFromStage();
    }

    public void CamAccel() {
        _followSpeed = _initialS * 4f;
        Debug.Log("Accel!");
    }

    public void InitSpeed()
    {
        _followSpeed = _initialS;
        Debug.Log("Init!");
    }
}