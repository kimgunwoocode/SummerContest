using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("카메라 타겟")]
    [SerializeField] GameObject Player;
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

    [Header("Background Sprites")]
    [SerializeField] private GameObject[] _backgrounds;
    private GameObject _currentBG;

    private GameManager GameManager;
    private PlayerManager playerManager;

    public Camera _cam;
    private Vector3 _offset;
    private Bounds _currentBounds;
    private Vector3 _smoothedFollowPos;
    private Vector3 _currentMouseOffset;
    private bool _transitioning = false;

    private Vector2 screenCenter;

    [HideInInspector] public float _targetZoom;
    private float _zoomSpeed = 10f;

    private Transform _backgroundTarget;

    private void Awake()
    {
        GameManager = Singleton.GameManager_Instance.Get<GameManager>();
        _stageIndex = GameManager.CurrentStartSceneCameraArea;
        _currentBG = _backgrounds[0];

        if (Player == null)
            Player = GameObject.FindWithTag("Player");
        if (_target == null)
            _target = Player?.transform;
        if (_targetRigidbody == null)
            _targetRigidbody = Player?.GetComponent<Rigidbody2D>();

        playerManager = Player.GetComponent<PlayerManager>();

        if (_cam == null)
            _cam = Camera.main;

        _offset = new Vector3(0, 0, -10f);

        _targetZoom = _cam.orthographicSize;

        SetStageIndex(_stageIndex);
        SetScreenCenter();
        SetBackgroundTarget(_cam.transform);

    }
    private void Start()
    {
        if (_target != null)
        {
            _smoothedFollowPos = _target.position;
            transform.position = _target.position;
        }
    }

    private void LateUpdate()
    {
        MoveCamera_InLateUpdate();
        ZoomInOut_InLateUpdate();
        BackgroundMove_InLateUpdate();
        SetBackgroundSize();
    }

    #region LateUpdate 관련
    private Vector3 Set_smoothedFollowPos()
    {
        Vector3 playerPos = _target.position;
        float dx = Mathf.Abs(playerPos.x - _smoothedFollowPos.x);
        float dy = Mathf.Abs(playerPos.y - _smoothedFollowPos.y);

        float followSpeed = _baseFollowSpeed + _targetRigidbody.linearVelocity.magnitude * _speedMultiplier;

        if (dx > _deadZoneSize.x || dy > _deadZoneSize.y)
        {
            _smoothedFollowPos = Vector3.Lerp(_smoothedFollowPos, playerPos, followSpeed * Time.deltaTime);
        }

        return _smoothedFollowPos;
    }
    private Vector3 Set_currentMouseOffset()
    {
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
            _currentMouseOffset = Vector3.Lerp(_currentMouseOffset, targetMouseOffset, _mouseFollowSpeed * Time.deltaTime);
        }

        return _currentMouseOffset;
    }
    private void MoveCamera_InLateUpdate()
    {
        _smoothedFollowPos = Set_smoothedFollowPos();
        _currentMouseOffset = Set_currentMouseOffset();

        Vector3 targetCamPos = _smoothedFollowPos + _currentMouseOffset;
        targetCamPos.z = _offset.z;

        Vector2 camSize = GetCameraWorldSize();
        Vector3 boundsCenter = _currentBounds.center;
        Vector3 newCamPos = targetCamPos;

        bool lockX = camSize.x >= _currentBounds.size.x;
        bool lockY = camSize.y >= _currentBounds.size.y;

        if (lockX) newCamPos.x = boundsCenter.x;
        else newCamPos.x = Mathf.Clamp(targetCamPos.x, _currentBounds.min.x + camSize.x / 2f, _currentBounds.max.x - camSize.x / 2f);

        if (lockY) newCamPos.y = boundsCenter.y;
        else newCamPos.y = Mathf.Clamp(targetCamPos.y, _currentBounds.min.y + camSize.y / 2f, _currentBounds.max.y - camSize.y / 2f);

        if (_transitioning)
        {
            transform.position = Vector3.Lerp(transform.position, newCamPos, 8f * Time.deltaTime);
            if (Vector3.Distance(transform.position, newCamPos) < 0.01f)
            {
                _transitioning = false;
            }
        }
        else
        {
            transform.position = newCamPos;
        }
    }

    private void ZoomInOut_InLateUpdate()
    {
        if (_cam.orthographicSize != _targetZoom)
        {
            _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _targetZoom, _zoomSpeed * Time.deltaTime);
        }
    }

    private void BackgroundMove_InLateUpdate() {
        _currentBG.transform.position = new Vector3(_backgroundTarget.position.x, _backgroundTarget.position.y, 0);
    }

    /*
     * 카메라 바로 이동... 인데 이제 안씀
    public Vector3 CalculateClampedCameraPosition(Vector3 desiredPos)
    {
        Vector2 camSize = GetCameraWorldSize();
        Vector3 boundsCenter = _currentBounds.center;

        bool lockX = camSize.x >= _currentBounds.size.x;
        bool lockY = camSize.y >= _currentBounds.size.y;

        Vector3 newCamPos = desiredPos;

        if (lockX) newCamPos.x = boundsCenter.x;
        else newCamPos.x = Mathf.Clamp(desiredPos.x, _currentBounds.min.x + camSize.x / 2f, _currentBounds.max.x - camSize.x / 2f);

        if (lockY) newCamPos.y = boundsCenter.y;
        else newCamPos.y = Mathf.Clamp(desiredPos.y, _currentBounds.min.y + camSize.y / 2f, _currentBounds.max.y - camSize.y / 2f);

        newCamPos.z = _offset.z;
        return newCamPos;
    }
    */

    private Vector2 GetCameraWorldSize()
    {
        float height = _cam.orthographicSize * 2f;
        float width = height * _cam.aspect;
        return new Vector2(width, height);
    }
    public void SetScreenCenter() // 화면 비율 적용시키기 (화면비율 변경시 호출하기)
    {
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }
#endregion

    public void SetStageIndex(int index)
    {
        if (index < 0 || index >= _cameraBoundsList.Length) return;
        _stageIndex = index;
        _currentBounds = _cameraBoundsList[index].bounds;
        _transitioning = true;
    }

    public void SetBias(Vector2 newBias)
    {
        _biasOffset = newBias;
    }
    public void ClearBias()
    {
        _biasOffset = Vector2.zero;
    }

    /// <summary>
    /// 카메라 사이즈 변경
    /// 즉각 적용 X, 줌인아웃 효과
    /// </summary>
    /// <param name="newSize">
    /// 양수만 적용됨</param>
    /// <param name="zoomSpeed">
    /// 양수만 적용됨</param>
    public void SetZoom(float newSize, float zoomSpeed)
    {
        if (newSize > 0f) _targetZoom = newSize;
        if (zoomSpeed > 0f) _zoomSpeed = zoomSpeed;
    }
    /// <summary>
    /// 카메라 사이즈 변경
    /// 사이즈 즉각 적용
    /// </summary>
    /// <param name="newSize">
    /// 양수만 적용됨</param>
    public void SetZoom(float newSize)
    {
        if (newSize <= 0f) return;
        _targetZoom = newSize;
        _cam.orthographicSize = newSize;
    }

    public void SetBackgroundTarget(Transform target) {
        _backgroundTarget = target;
    }

    public void SetBackgroundSize() {

        float cameraHeight = _cam.orthographicSize * 2;

        float cameraWidth = cameraHeight * _cam.aspect;

        SpriteRenderer spriteRenderer = _currentBG.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null) {
            float spriteWidth = spriteRenderer.sprite.bounds.size.x;
            float spriteHeight = spriteRenderer.sprite.bounds.size.y;

            float scaleX = cameraWidth / spriteWidth;
            float scaleY = cameraHeight / spriteHeight;

            float scale = Mathf.Max(scaleX, scaleY);
            _currentBG.transform.localScale = new Vector3(scale * 1f, scale * 1f, 1);
        } else {
            Debug.LogError("SpriteRenderer didn't found");
        }
    }
}
