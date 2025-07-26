using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform groundCheckerTransform;
    [SerializeField] private Transform ceilingCheckerTransform;
    [SerializeField] private Transform[] wallRaycastPoints;

    internal Vector2 _currentInput;
    private Rigidbody2D _rb;

    private bool _cachedQueryStartInColliders;
    private Vector2 _calculatedVelocity;

    private ScriptablePlayerMovementStats _movementStats;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        _movementStats = GetComponent<PlayerManager>().playerMovementStats;
    }

    private void Start() {
        _leftBonusJump = _movementStats.bonusJump;
        _moveDirection = Vector2.zero;
        _isDashing = false;
        _isAbleToDash = true;
        _isJumped = false;
        _moveDirection.x = -1;

    }

    private void Update()
    {

    }

    #region Movement
    private bool _isCrouch;
    private Vector2 _moveDirection;
    private bool _isDashing;
    private bool _isAbleToDash;
    private bool _isGlide;

    internal void OnMovePerformed(InputAction.CallbackContext context) {
        _currentInput = context.ReadValue<Vector2>();
    }

    internal void OnMoveCanceled(InputAction.CallbackContext context) {
        _currentInput = Vector2.zero;
    }

    internal void OnCrouchPerformed(InputAction.CallbackContext context) {
        _isCrouch = _movementStats.isCrounchActionByToggle ? !_isCrouch : true;
    }

    internal void OnCrouchCanceled(InputAction.CallbackContext context) {
        _isCrouch = _movementStats.isCrounchActionByToggle ? _isCrouch : false;
    }

    internal void OnGlidePerformed(InputAction.CallbackContext context) {
        if (!_movementStats.IsGlideUnlocked) return;
        _isGlide = _movementStats.isGlideActionByToggle ? !_isGlide : true;
    }

    internal void OnGlideCanceled(InputAction.CallbackContext context) {
        if (!_movementStats.IsGlideUnlocked) return;
        _isGlide = _movementStats.isGlideActionByToggle ? !_isGlide : false;
    }

    internal void OnDashPerformed(InputAction.CallbackContext context) {
        if (!_movementStats.IsDashUnlocked || !_isAbleToDash || _isDashing) return;
        _isAbleToDash = false;
        _isDashing = true;
        _calculatedVelocity.y = 0;
        StartCoroutine(StopDash());
        StartCoroutine(DashCooldown());
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(_movementStats.DashTime);
        _isDashing = false;
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(_movementStats.DashCooldown);
        _isAbleToDash = true;
    }

    private void UpdateMoveDir()
    {
        if (_isDashing) return;
        if (_currentInput.x == 0) return;
        _moveDirection.x = _currentInput.x;
    }
    private void Move()
    {
        UpdateMoveDir();

        _calculatedVelocity.x = _isTouchingWall ? 0f : _isDashing ? (_movementStats.DashSpeed * _moveDirection.x * Time.fixedDeltaTime) : _isCrouch ? ((_isGrounded ? _movementStats.CrounchSpeed : _movementStats.WalkSpeed) * Time.fixedDeltaTime * _currentInput.x) : (_movementStats.WalkSpeed * Time.fixedDeltaTime * _currentInput.x);
    }


    internal Vector2 ApplyMove() {
        return _calculatedVelocity;
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
            if (!_movementStats.IsDoubleJumpUnloceked) return;
            _leftBonusJump -= 1;
        }
        _calculatedVelocity.y = _movementStats.JumpForce;
        _isJumpRequestExist = false;
    }

    internal void OnJumpCanceled(InputAction.CallbackContext context) {
        _heldJump = false;
    }

    private bool CheckJumpEndedBeforeApex() {
        return (_calculatedVelocity.y > 0 && !_isJumpEndedEarly && !_isGrounded && !_heldJump);
    }

    private void JumpRequestValidation()
    {
        if (_isDashing) return;
        _isJumpEndedEarly = CheckJumpEndedBeforeApex();

        bool jumpBufferValidation = ((_groundedTime - _jumpPressTime) < _movementStats.JumpBufferTime) && _isGrounded;

        bool coyoteJumpValidation = (_jumpPressTime - _leftGroundTime) < _movementStats.CoyoteTime;

        bool bonusJumpValidation = !_isGrounded && _leftBonusJump > 0;

        if (!_isJumpRequestExist && !jumpBufferValidation) return;

        bool canJump = (coyoteJumpValidation || jumpBufferValidation || bonusJumpValidation);
        if (canJump) ExecuteJump((!_isGrounded && !jumpBufferValidation && !coyoteJumpValidation) ? 1 : 0);
    }
    #endregion

    #region CollisionCheck
    private bool _isTouchingWall;
    internal bool _isGrounded = false;
    internal bool _isCeiling = false;

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        //check Ground hit
        bool groundCheck = CheckGround();

        //check wall hit
        bool wallCheck = CheckWall();

        //check ceiling hit
        bool ceilingCheck = CheckCeiling();

        //landed on ground
        if (groundCheck && !_isGrounded) {
            _isGrounded = true;
            _isJumpEndedEarly = false;
            _isJumped = false;
            _leftBonusJump = _movementStats.bonusJump;
            _groundedTime = Time.time;

        //leave from ground
        } else if (!groundCheck && _isGrounded) {
            _isGrounded = false;
            _leftGroundTime = Time.time;
        }

        if (ceilingCheck && !_isCeiling) {
            _isCeiling = true;
            _calculatedVelocity.y = _calculatedVelocity.y > 0 ? _calculatedVelocity.y * -0.1f : _calculatedVelocity.y;

        } else if (!ceilingCheck && _isCeiling) {
            _isCeiling = false;
        }

        if (wallCheck && !_isTouchingWall)
        {
            _isTouchingWall = true;

        }
        else if (!wallCheck && _isTouchingWall)
        {
            _isTouchingWall = false;
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }

    private bool CheckGround()
    {
        LayerMask checkGround = 0; //00000000

        foreach(LayerMask groundLayer in _movementStats.groundLayers) 
            checkGround |= groundLayer;
        foreach (LayerMask passableLayer in _movementStats.passableLayers)
            checkGround |= passableLayer;
        
        return Physics2D.OverlapBox(groundCheckerTransform.position - new Vector3(0, _movementStats.groundCheckDistance / 2), new Vector2(transform.localScale.x * 0.85f, _movementStats.groundCheckDistance / 2), 0f, checkGround);
        
        

    }
    private bool CheckWall()
    {
        Vector2 rayCastDir = (_currentInput.x > 0) ? Vector2.right : Vector2.left;

        foreach (Transform point in wallRaycastPoints)
        {
            foreach (LayerMask groundLayer in _movementStats.groundLayers) {
                RaycastHit2D hit = Physics2D.Raycast(point.position, rayCastDir, _movementStats.wallCheckDistance, groundLayer);
                //                                                                                        ^^^^^^^^^^^^^^^^^^^^  it's because the wall functions as the ground
                if (hit.collider != null) {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckCeiling() {
        LayerMask checkCeiling = 0; //00000000

        foreach (LayerMask groundLayer in _movementStats.groundLayers)
            checkCeiling |= groundLayer;

        return Physics2D.OverlapBox(ceilingCheckerTransform.position + new Vector3(0, _movementStats.ceilingCheckDistance / 2), new Vector2(transform.localScale.x * 0.85f, _movementStats.ceilingCheckDistance / 2), 0f, checkCeiling);
    }

    #endregion
    #region Gravity
    private void Gravity()
    {
        //touching the ground
        if (_isGrounded && _calculatedVelocity.y <= 0f) _calculatedVelocity.y = _movementStats.GravityByNormalForce;

        //in mid-air
        else
        {
            float midAirGravity = _movementStats.MidAirGravity;
            if (!_isGrounded && _isGlide && _calculatedVelocity.y < 0) { midAirGravity = _movementStats.MidAirGravity * _movementStats.GlideGravity; _calculatedVelocity.y = -_movementStats.GlideFallSpeed; }
            else if ((_isJumped) && Mathf.Abs(_calculatedVelocity.y) < _movementStats.ApexThreadHold) midAirGravity = _movementStats.MidAirGravity * _movementStats.ApexModifier;
            else if (_calculatedVelocity.y < 0f) midAirGravity = _movementStats.MidAirGravity * _movementStats.GravityModifierWhenFalling;
            else if (_isJumpEndedEarly) midAirGravity = _movementStats.MidAirGravity * _movementStats.GravityModifierWhenJumpEndedEarly;

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
            Gizmos.DrawWireCube(groundCheckerTransform.position - new Vector3(0, _GizmoStats.groundCheckDistance / 2), new Vector2(transform.localScale.x * 0.85f, _GizmoStats.groundCheckDistance / 2));
        }

        if (ceilingCheckerTransform != null) {
            Gizmos.color = _isCeiling ? Color.green : Color.red;
            Gizmos.DrawWireCube(ceilingCheckerTransform.position + new Vector3(0, _GizmoStats.groundCheckDistance / 2), new Vector2(transform.localScale.x * 0.85f, _GizmoStats.groundCheckDistance / 2));
        }

        if (wallRaycastPoints != null && _moveDirection.x != 0)
        {
            Vector3 gizmoDirection = (_moveDirection.x > 0) ? Vector3.right : Vector3.left;
            Gizmos.color = _isTouchingWall ? Color.blue : Color.gray;
            foreach (Transform point in wallRaycastPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawLine(point.position, point.position + gizmoDirection * _GizmoStats.wallCheckDistance);
                }
            }
        }
        else if (wallRaycastPoints != null && _moveDirection.x == 0)
        {
            Gizmos.color = Color.gray;
            foreach (Transform point in wallRaycastPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawLine(point.position, point.position + Vector3.right * _GizmoStats.wallCheckDistance);
                }
            }
        }
    }
    #endregion

    private void FixedUpdate()
    {
        CheckCollisions();
        Gravity();
        Move();
        JumpRequestValidation();
        _rb.linearVelocity = _calculatedVelocity;
    }
}