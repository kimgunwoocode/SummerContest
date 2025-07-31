using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour {
    [Header("Transforms")]
    [SerializeField] private Transform groundCheckerTransform;
    [SerializeField] private Transform ceilingCheckerTransform;
    [SerializeField] private Transform[] wallcheckTransforms;
    [SerializeField] private Transform[] cornerCheckTransforms;

    internal Vector2 _currentInput;
    private Rigidbody2D _rb;

    private bool _cachedQueryStartInColliders;
    private Vector2 _calculatedVelocity;

    private BoxCollider2D _playerCollider;
    private Collider2D _currentPlatform;

    private ScriptablePlayerMovementStats _movementStats;
    private PlayerManager _PM;
    private GameDataManager _data;
    private bool isControllablePlayer = true;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        _movementStats = GetComponent<PlayerManager>().playerMovementStats;
        _PM = GetComponent<PlayerManager>();
        _playerCollider = GetComponent<BoxCollider2D>();
        _data = Singleton.GameManager_Instance.Get<GameDataManager>();
    }

    private void Start() {
        _leftBonusJump = _movementStats.bonusJump;
        _moveDirection = Vector2.zero;
        _isDashing = false;
        _isAbleToDash = true;
        _isJumped = false;
        _moveDirection.x = -1;
        _rb.gravityScale = 0f;

    }

    private void AnimationState() {
        _PM.Anima.SetDash(_isDashing);
        _PM.Anima.SetGrounded(_isGrounded);
        _PM.Anima.SetGlide(_isGlide);
        _PM.Anima.SetClimb(_isClimb[0] || _isClimb[1]);
        _PM.Anima.SetStun(_isStun);

        if (_isIdle && _isGrounded) {
            _PM.Anima.SetSpeed(0);
        } else if (!_isIdle && _isCrouch && _isGrounded) {
            _PM.Anima.SetSpeed(1);
        } else if (!_isIdle && !_isCrouch && _isGrounded) {
            _PM.Anima.SetSpeed(2);
        }

    }

    #region Movement
    private bool _isCrouch;
    private Vector2 _moveDirection;
    private bool _isDashing;
    private bool _isAbleToDash;
    private bool _isGlide;
    private bool _isPlatformDownRequestExist;
    private bool[] _isClimb = new bool[2];
    private float _wallLeftTime;
    private bool _isStun = false;

    private IEnumerator DownPlatform() {
        _isPlatformDownRequestExist = true;
        BoxCollider2D _platformC = _currentPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(_playerCollider, _platformC);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(_playerCollider, _platformC, false);
        _isPlatformDownRequestExist = false;
    }

    internal void OnMovePerformed(InputAction.CallbackContext context) {
        if (!isControllablePlayer) return;
        _currentInput = context.ReadValue<Vector2>();
    }

    internal void OnMoveCanceled(InputAction.CallbackContext context) {
        _currentInput = Vector2.zero;
    }

    internal void OnCrouchPerformed(InputAction.CallbackContext context) {
        _isCrouch = _movementStats.IsCrounchActionByToggle ? !_isCrouch : true;
    }

    internal void OnCrouchCanceled(InputAction.CallbackContext context) {
        _isCrouch = _movementStats.IsCrounchActionByToggle ? _isCrouch : false;
    }

    internal void OnGlidePerformed(InputAction.CallbackContext context) {
        if (!_data.PlayerAbility[4]) return;
        _isGlide = _movementStats.IsGlideActionByToggle ? !_isGlide : true;
    }

    internal void OnGlideCanceled(InputAction.CallbackContext context) {
        if (!_data.PlayerAbility[4]) return;
        _isGlide = _movementStats.IsGlideActionByToggle ? !_isGlide : false;
    }

    internal void OnDashPerformed(InputAction.CallbackContext context) {
        if (!_data.PlayerAbility[0] || !_isAbleToDash || _isDashing || !isControllablePlayer) return;
        _isAbleToDash = false;
        _isDashing = true;
        _calculatedVelocity.y = 0;
        _PM.IsInvincible = true;
        StartCoroutine(StopDash());
        StartCoroutine(DashCooldown());
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(_movementStats.DashTime);
        _isDashing = false;
        _PM.IsInvincible = false;
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(_movementStats.DashCooldown);
        _isAbleToDash = true;
    }

    internal void OnPlatformDown(InputAction.CallbackContext context) {
        if (_currentPlatform != null) {
            StartCoroutine(DownPlatform());
        }
    }

    private void UpdateMoveDir()
    {
        if (_isDashing) return;
        if (_currentInput.x == 0) return;
        _moveDirection.x = _currentInput.x;
    }

    private void Climb() {
        if(_isClimbable[0]) {
            _isClimb[0] = true;
        } else {
            _isClimb[0] = false;
        }

        if (_isClimbable[1]) {
            _isClimb[1] = true;
        } else {
            _isClimb[1] = false;
        }
    }

    private void Move() {
        UpdateMoveDir();
        Climb();

        if((_isClimb[0] && _currentInput.x <= 0 && !_isWallJumping) || _isClimb[1] && _currentInput.x >= 0 && !_isWallJumping) {
            _calculatedVelocity.y = 0;
        }
        if (!isControllablePlayer) return;
        _calculatedVelocity.x = _isWallJumping ? _calculatedVelocity.x : (_isTouchingWall[0] && _moveDirection.x < 0) || (_isTouchingWall[1] && _moveDirection.x > 0) || (_isClimb[0] && _currentInput.x <= 0) || (_isClimb[1] && _currentInput.x >= 0) ? 0f : _isDashing ? (_movementStats.DashSpeed * _moveDirection.x * Time.fixedDeltaTime) : _isCrouch ? ((_isGrounded ? _movementStats.CrounchSpeed : _movementStats.WalkSpeed) * Time.fixedDeltaTime * _currentInput.x) : (_movementStats.WalkSpeed * Time.fixedDeltaTime * _currentInput.x);
    }

    public void Knockback(Vector3 attackerPos, int power = 4, float stunTime = 1f) {
        StartCoroutine(RunKnockback(attackerPos, power, stunTime));
    }

    private IEnumerator RunKnockback(Vector3 attackerPos, int power, float stunTime) {
        int direction = attackerPos.x - transform.position.x > 0 ? -1 : 1;
        _calculatedVelocity = new Vector2(power * 2 * direction, power * 3);
        isControllablePlayer = false;
        _isStun = true;
        yield return new WaitForSeconds(stunTime);
        isControllablePlayer = true;
        _isStun = false;
    }

    internal Vector2 ApplyMove() {
        return _calculatedVelocity;
    }

    internal float ApplyMoveDir() {
        return _moveDirection.x;
    }
    #endregion

    #region Jump
    private bool _isJumpEndedEarly;
    private bool _heldJump;
    private bool _isJumped;
    private bool _isJumpRequestExist;

    private float _jumpPressTime;
    private float _leftGroundTime;
    private float _groundedTime;

    private int _leftBonusJump;

    private int _wallJumpDirection;
    private bool _isWallJumping;



    internal void OnJumpPerformed(InputAction.CallbackContext context) {
        _heldJump = true;
        _jumpPressTime = Time.time;
        _isJumpRequestExist = true;
    }

    private void ExecuteJump(int jumpType)
    { // 1 : bonus Jump
        _moveDirection = _currentInput.x == 0 ? _moveDirection : _currentInput;
        if (jumpType == 0) {
            _isJumped = true;
        }
        else if (jumpType == 1) {
            if (!_PM.Abilitis[2]) return;
            _leftBonusJump -= 1;
            _PM.Anima.EnterDoubleJump();
        }
        _calculatedVelocity.y = _movementStats.JumpForce;
        _isJumpRequestExist = false;
    }

    private IEnumerator WallJump() {
        _isWallJumping = true;
        float wallJumpDirection = _wallJumpDirection;
        float targetVelocityX = wallJumpDirection * _movementStats.WallJumpXVelocity;
        float startVelocityX = _calculatedVelocity.x;

         _calculatedVelocity.y = _movementStats.JumpForce;

        float lerpDuration = _movementStats.WallJumpingDuration;
        float timeElapsed = 0f;

        _isJumped = true;
        _isJumpRequestExist = false;
        _leftBonusJump = Mathf.Max(0, _leftBonusJump);

        while (timeElapsed < lerpDuration) {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / lerpDuration;
            _calculatedVelocity.x = Mathf.Lerp(startVelocityX, targetVelocityX, 1-t);
            yield return null;
        }

        _calculatedVelocity.x = targetVelocityX;
        _isWallJumping = false;
    }

    internal void OnJumpCanceled(InputAction.CallbackContext context) {
        _heldJump = false;
    }

    private bool CheckJumpEndedBeforeApex() {
        return (_calculatedVelocity.y > 0 && !_isJumpEndedEarly && !_isGrounded && !_heldJump);
    }

    private void JumpRequestValidation()
    {
        if (_isDashing || !isControllablePlayer || _isStun) return;
        _isJumpEndedEarly = CheckJumpEndedBeforeApex();

        bool jumpBufferValidation = ((_groundedTime - _jumpPressTime) < _movementStats.JumpBufferTime) && _isGrounded;

        bool coyoteJumpValidation = (_jumpPressTime - _leftGroundTime) < _movementStats.CoyoteTime;

        bool bonusJumpValidation = !_isGrounded && _leftBonusJump > 0;

        bool wallJumpValidation = !_isGrounded && (_isClimb[0] || _isClimb[1] || (_jumpPressTime - _wallLeftTime < _movementStats.WallJumpBufferTime));

        if (!_isJumpRequestExist && !jumpBufferValidation) return;

        bool isNormalJump = (coyoteJumpValidation || jumpBufferValidation || bonusJumpValidation);
        if (wallJumpValidation) StartCoroutine(WallJump());
        else if (isNormalJump) ExecuteJump((!_isGrounded && !jumpBufferValidation && !coyoteJumpValidation) ? 1 : 0);
        
    }
    #endregion

    #region Collision Check
    private bool[] _isTouchingWall = new bool[2];//_isTouchingWall[0] means left side, the other one means right side
    internal bool _isGrounded = false;
    internal bool _isCeiling = false;
    internal bool _isStuckInPlatform = false;
    internal bool[] _isClimbable = new bool[2];//Its functionality is identical to what was described earlier.

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        //check Ground hit
        bool groundCheck = CheckGround();

        //check wall hit
        bool[] wallCheck = CheckWall();

        //check ceiling hit
        bool ceilingCheck = CheckCeiling();

        bool platformStuckCheck = CheckIsStuckInPlatform();

        bool[] climbableCheck = CheckClimbable();

        bool[] cornerCheck = CheckCorner();

        //landed on ground
        if (groundCheck && !_isGrounded) {
            _isGrounded = true;
            _isJumpEndedEarly = false;
            _isJumped = false;
            _leftBonusJump = _movementStats.bonusJump;
            _groundedTime = Time.time;
            _isPlatformDownRequestExist = false;
            _wallLeftTime = 0f;

        //leave from ground
        } else if (!groundCheck && _isGrounded) {
            _isGrounded = false;
            _leftGroundTime = Time.time;
            _currentPlatform = null;
            _PM.Anima.SetGrounded(false);
        }

        if (ceilingCheck && !_isCeiling) {
            _isCeiling = true;
            _calculatedVelocity.y = _calculatedVelocity.y > 0 ? _calculatedVelocity.y * -0.1f : _calculatedVelocity.y;

        } else if (!ceilingCheck && _isCeiling) {
            _isCeiling = false;
        }

        if (wallCheck[0] && !_isTouchingWall[0])
        {
            _isTouchingWall[0] = true;

        }
        else if (!wallCheck[0] && _isTouchingWall[0])
        {
            _isTouchingWall[0] = false;
        }

        if (wallCheck[1] && !_isTouchingWall[1]) {
            _isTouchingWall[1] = true;

        } else if (!wallCheck[1] && _isTouchingWall[1]) {
            _isTouchingWall[1] = false;
        }

        //landed on wall in left side
        if (climbableCheck[0] && !_isClimbable[0]) {
            _isClimbable[0] = true;
            _wallJumpDirection = 1;
        } else if (!climbableCheck[0] && _isClimbable[0]) {
            _isClimbable[0] = false;
            _wallLeftTime = Time.time;
        }

        //landed on wall in right side
        if (climbableCheck[1] && !_isClimbable[1]) {
            _isClimbable[1] = true;
            _wallJumpDirection = -1;

        } else if (!climbableCheck[1] && _isClimbable[1]) {
            _isClimbable[1] = false;
            _wallLeftTime = Time.time;
        }

        if (platformStuckCheck && !_isPlatformDownRequestExist) {
            transform.position = transform.position + new Vector3(0, 0.1f, 0);
        }else if(platformStuckCheck && _isPlatformDownRequestExist) {
            _isGrounded = false;
        }

        if (cornerCheck[0] && !_isGrounded) {
            _rb.transform.position = _rb.transform.position + new Vector3(0, 0.1f, 0);
        }else if (cornerCheck[1] && !_isGrounded) {
            _rb.transform.position = _rb.transform.position - new Vector3(0, 0.1f, 0);
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }

    private bool CheckGround() {
        LayerMask checkGround = 0; //00000000

        foreach(LayerMask groundLayer in _movementStats.GroundLayers) 
            checkGround |= groundLayer;

        if (!_isPlatformDownRequestExist) {
            checkGround |= _movementStats.PlatformLayers;
        }


        _currentPlatform = Physics2D.OverlapBox(groundCheckerTransform.position - new Vector3(0, _movementStats.GroundCheckDistance / 2), new Vector2(transform.localScale.x * 0.85f, _movementStats.GroundCheckDistance / 2), 0f, _movementStats.PlatformLayers);

        return Physics2D.OverlapBox(groundCheckerTransform.position - new Vector3(0, _movementStats.GroundCheckDistance / 2), new Vector2(transform.localScale.x * 0.85f, _movementStats.GroundCheckDistance / 2), 0f, checkGround);
        
        

    }
    private bool[] CheckWall() {
        LayerMask wallLayer = 0;

        foreach (LayerMask wall in _movementStats.GroundLayers)
            wallLayer |= wall;

        bool[] results = new bool[2];
        results[0] = Physics2D.OverlapBox(wallcheckTransforms[0].position, new Vector2(0.1f, transform.localScale.y * 0.8f), 0f, wallLayer);
        results[1] = Physics2D.OverlapBox(wallcheckTransforms[1].position, new Vector2(0.1f, transform.localScale.y * 0.8f), 0f, wallLayer);

        return results;
    }

    private bool[] CheckClimbable() {
        if (_isGrounded) return new bool[] {false, false};
        LayerMask wallLayer = _movementStats.ClimbableWallLayer;
        Vector3 leftSidePos = wallcheckTransforms[0].position;
        Vector3 rightSidePos = wallcheckTransforms[1].position;

        bool[] results = new bool[2];
        bool leftSide = false;
        bool rightSide = false;

        rightSide = Physics2D.OverlapBox(rightSidePos, new Vector2(0.1f, transform.localScale.y * 0.8f), 0f, wallLayer);
        leftSide = Physics2D.OverlapBox(leftSidePos, new Vector2(0.1f, transform.localScale.y * 0.8f), 0f, wallLayer);

        results[0] = leftSide;
        results[1] = rightSide;

        return results;
    }

    private bool CheckCeiling() {
        LayerMask checkCeiling = 0; //00000000

        foreach (LayerMask groundLayer in _movementStats.GroundLayers)
            checkCeiling |= groundLayer;

        return Physics2D.OverlapBox(ceilingCheckerTransform.position + new Vector3(0, _movementStats.CeilingCheckDistance / 2), new Vector2(transform.localScale.x * 0.85f, _movementStats.CeilingCheckDistance / 2), 0f, checkCeiling);
    }

    private bool CheckIsStuckInPlatform() {
        LayerMask checkIsPlatform = 0;
        checkIsPlatform |= _movementStats.PlatformLayers;

        return Physics2D.OverlapBox(groundCheckerTransform.position + new Vector3(0, _movementStats.GroundCheckDistance), new Vector2(transform.localScale.x * 0.85f, _movementStats.GroundCheckDistance / 2), 0f, checkIsPlatform);
    }

    private bool[] CheckCorner() {
        bool[] results = new bool[2];
        int index = 0;

        foreach (Transform corner in cornerCheckTransforms) {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(corner.position, Vector2.down, _movementStats.GroundCheckDistance/2);

            results[index] = hit;
            
            index++;
        }

        return results;
    }

    #endregion

    #region Gravity
    private void Gravity()
    {
        //touching the ground
        if (_isGrounded && _calculatedVelocity.y <= 0f) _calculatedVelocity.y = -_movementStats.GravityByNormalForce;

        //in mid-air
        else {
            float midAirGravity = _movementStats.MidAirGravity;
            if (_isClimb[0] || _isClimb[1]) { midAirGravity = 0f; }
            
            else if (!_isGrounded && _isGlide && _calculatedVelocity.y < 0) { midAirGravity = _movementStats.MidAirGravity * _movementStats.GlideGravity; _calculatedVelocity.y = -_movementStats.GlideFallSpeed; } 
            else if ((_isJumped) && Mathf.Abs(_calculatedVelocity.y) < _movementStats.ApexThreadHold) { midAirGravity = _movementStats.MidAirGravity * _movementStats.ApexModifier;} 
            else if (_calculatedVelocity.y < 0f) {midAirGravity = _movementStats.MidAirGravity * _movementStats.GravityModifierWhenFalling;}
            
            else if (_isJumpEndedEarly) {midAirGravity = _movementStats.MidAirGravity * _movementStats.GravityModifierWhenJumpEndedEarly;}
            else if (_calculatedVelocity.y > 0) {}

            _calculatedVelocity.y = Mathf.MoveTowards(_calculatedVelocity.y, _movementStats.MaxFallingSpeed, Time.fixedDeltaTime * midAirGravity);
        }
    }
    #endregion

    #region Debugging
    [SerializeField] ScriptablePlayerMovementStats _GizmoStats;
    private void OnDrawGizmos()
    {
        if (groundCheckerTransform != null)
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(groundCheckerTransform.position - new Vector3(0, _GizmoStats.GroundCheckDistance / 2), new Vector2(transform.localScale.x * 0.85f, _GizmoStats.GroundCheckDistance / 2));

            Gizmos.color = _isStuckInPlatform ? Color.green : Color.red;
            Gizmos.DrawWireCube(groundCheckerTransform.position + new Vector3(0, _GizmoStats.GroundCheckDistance), new Vector2(transform.localScale.x * 0.85f, _GizmoStats.GroundCheckDistance / 2));
        }

        if (wallcheckTransforms[0] != null) {
            Gizmos.color = _isTouchingWall[0] ? Color.green : _isClimb[0] ? Color.blue : Color.red;
            Gizmos.DrawWireCube(wallcheckTransforms[0].position, new Vector2(0.1f, transform.localScale.y * 0.8f));
        }
        if(wallcheckTransforms[1] != null) {
            Gizmos.color = _isTouchingWall[1] ? Color.green : _isClimb[1] ? Color.blue : Color.red;
            Gizmos.DrawWireCube(wallcheckTransforms[1].position, new Vector2(0.1f, transform.localScale.y * 0.8f));
        }

        if (ceilingCheckerTransform != null) {
            Gizmos.color = _isCeiling ? Color.green : Color.red;
            Gizmos.DrawWireCube(ceilingCheckerTransform.position + new Vector3(0, _GizmoStats.GroundCheckDistance / 2), new Vector2(transform.localScale.x * 0.85f, _GizmoStats.GroundCheckDistance / 2));
        }
    }
    #endregion

    #region Frame Buffer
    private const int FRAME_BUFFER_SIZE = 20;
    private float[] _prevInputInfo = new float[FRAME_BUFFER_SIZE];
    private int _frameIndex = 0;

    private bool _isIdle;

    private void Update() {
        _prevInputInfo[_frameIndex] = _currentInput.x;
        _frameIndex = (_frameIndex + 1) % FRAME_BUFFER_SIZE;

        float _inputSum = 0;
        foreach (float inputInfo in _prevInputInfo) {
            _inputSum += Mathf.Abs(inputInfo);

        }

        _isIdle = _inputSum == 0 ? true : false;
    }
    #endregion


    private void FixedUpdate() {
        CheckCollisions();
        Gravity();
        Move();
        JumpRequestValidation();
        AnimationState();
        _PM.Anima.flip(_moveDirection.x > 0);
        _rb.linearVelocity = _calculatedVelocity;
    }
}