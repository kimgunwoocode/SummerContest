using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("카메라 타겟")]
    [SerializeField] private Transform _target;
    [SerializeField] private Rigidbody2D _targetRigidbody;

    [Header("카메라 이동 속도 설정")]
    [SerializeField] private float _baseFollowSpeed = 5f;
    [SerializeField] private float _speedMultiplier = 0.2f;

    [Header("플레이어 데드존 설정")]
    [SerializeField] private Vector2 _deadZoneSize = new Vector2(1f, 0.5f);

    [Header("마우스 시야 이동 설정")]
    [SerializeField] private float _maxMouseOffset = 2f;
    [SerializeField] private float _mouseFollowSpeed = 0f;
    [SerializeField] private Vector2 _biasOffset = Vector2.zero;

    [Header("카메라 바운드 설정")]
    [SerializeField] private Collider2D[] _cameraBoundsList;
    [SerializeField] private int _stageIndex = 0;

    private Camera _cam;
    private Vector3 _offset;
    private Collider2D _currentBounds;
    private Vector3 _smoothedFollowPos;
    private Vector3 _currentMouseOffset;
    private bool _transitioning = false;

    private float _targetZoom;
    private float _zoomSpeed = 5f;

    private void Awake()
    {
        _cam = Camera.main;
        _offset = new Vector3(0, 0, -10);
        _targetZoom = _cam.orthographicSize;
        SetStageIndex(_stageIndex);

        if (_target != null)
        {
            _smoothedFollowPos = _target.position;
            Vector3 startCamPos = CalculateClampedCameraPosition(_smoothedFollowPos);
            transform.position = startCamPos;
        }
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = _target.position;
        float dx = Mathf.Abs(playerPos.x - _smoothedFollowPos.x);
        float dy = Mathf.Abs(playerPos.y - _smoothedFollowPos.y);

        float followSpeed = _baseFollowSpeed;
        if (_targetRigidbody != null)
        {
            followSpeed += _targetRigidbody.linearVelocity.magnitude * _speedMultiplier;
        }

        if (dx > _deadZoneSize.x || dy > _deadZoneSize.y)
        {
            _smoothedFollowPos = Vector3.Lerp(_smoothedFollowPos, playerPos, followSpeed * Time.fixedDeltaTime);
        }

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector2 offsetFromCenter = (mouseScreen - screenCenter) / screenCenter;
        Vector3 targetMouseOffset = new Vector3(offsetFromCenter.x, offsetFromCenter.y, 0f) * _maxMouseOffset;
        targetMouseOffset += (Vector3)_biasOffset;

        if (_mouseFollowSpeed <= 0f)
        {
            _currentMouseOffset = targetMouseOffset;
        }
        else
        {
            _currentMouseOffset = Vector3.Lerp(_currentMouseOffset, targetMouseOffset, _mouseFollowSpeed * Time.fixedDeltaTime);
        }

        Vector3 targetCamPos = _smoothedFollowPos + _currentMouseOffset;
        targetCamPos.z = _offset.z;

        Vector2 camSize = GetCameraWorldSize();
        Bounds bounds = _currentBounds.bounds;
        Vector3 boundsCenter = bounds.center;
        Vector3 newCamPos = targetCamPos;

        bool lockX = camSize.x >= bounds.size.x;
        bool lockY = camSize.y >= bounds.size.y;

        if (lockX) newCamPos.x = boundsCenter.x;
        else newCamPos.x = Mathf.Clamp(targetCamPos.x, bounds.min.x + camSize.x / 2f, bounds.max.x - camSize.x / 2f);

        if (lockY) newCamPos.y = boundsCenter.y;
        else newCamPos.y = Mathf.Clamp(targetCamPos.y, bounds.min.y + camSize.y / 2f, bounds.max.y - camSize.y / 2f);

        if (_transitioning)
        {
            transform.position = Vector3.Lerp(transform.position, newCamPos, 8f * Time.fixedDeltaTime);
            if (Vector3.Distance(transform.position, newCamPos) < 0.01f)
            {
                _transitioning = false;
            }
        }
        else
        {
            transform.position = newCamPos;
        }

        // 줌 인아웃 관련
        if (_cam.orthographicSize != _targetZoom)
        {
            _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _targetZoom, _zoomSpeed * Time.fixedDeltaTime);
        }
    }
    private Vector3 CalculateClampedCameraPosition(Vector3 desiredPos)
    {
        Vector2 camSize = GetCameraWorldSize();
        Bounds bounds = _currentBounds.bounds;
        Vector3 boundsCenter = bounds.center;

        bool lockX = camSize.x >= bounds.size.x;
        bool lockY = camSize.y >= bounds.size.y;

        Vector3 newCamPos = desiredPos;

        if (lockX) newCamPos.x = boundsCenter.x;
        else newCamPos.x = Mathf.Clamp(desiredPos.x, bounds.min.x + camSize.x / 2f, bounds.max.x - camSize.x / 2f);

        if (lockY) newCamPos.y = boundsCenter.y;
        else newCamPos.y = Mathf.Clamp(desiredPos.y, bounds.min.y + camSize.y / 2f, bounds.max.y - camSize.y / 2f);

        newCamPos.z = _offset.z;
        return newCamPos;
    }

    private Vector2 GetCameraWorldSize()
    {
        float height = _cam.orthographicSize * 2f;
        float width = height * _cam.aspect;
        return new Vector2(width, height);
    }

    public void SetStageIndex(int index)
    {
        if (index < 0 || index >= _cameraBoundsList.Length) return;
        _stageIndex = index;
        _currentBounds = _cameraBoundsList[index];
        _transitioning = true;
    }

    public void SetTarget(Transform newTarget, Rigidbody2D rb = null)
    {
        _target = newTarget;
        _targetRigidbody = rb;
        _smoothedFollowPos = newTarget.position;
    }

    public void SetBias(Vector2 newBias)
    {
        _biasOffset = newBias;
    }

    public void ClearBias()
    {
        _biasOffset = Vector2.zero;
    }

    public void SetCameraPositionInstant(Vector3 worldPosition)
    {
        _smoothedFollowPos = worldPosition;
        transform.position = new Vector3(worldPosition.x, worldPosition.y, _offset.z);
    }

    public void SetZoom(float newSize)
    {
        _targetZoom = newSize;
    }

    public float GetZoom()
    {
        return _cam != null ? _cam.orthographicSize : 0f;
    }
}
