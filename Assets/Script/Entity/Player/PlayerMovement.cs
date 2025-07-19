using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private ScriptablePlayerStats _statsData;
    [Header("Transforms")]
    [SerializeField] private Transform groundCheckerTransform;
    [SerializeField] private Transform[] wallRaycastPoints;

    internal Vector2 _currentInput;
    private Rigidbody2D _rb;

    private bool _cachedQueryStartInColliders;
    private Vector2 _calculatedVelocity;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        _leftBonusJump = 1;//_statsData.bonusJump;

        _moveDirection = Vector2.zero;
        _isDashing = false;
        _isAbleToDash = true;
        _isJumped = false;
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
    internal void OnMovePerformed(InputAction.CallbackContext context)
    {
        _currentInput = context.ReadValue<Vector2>();
    }

    internal void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _currentInput = Vector2.zero;
    }

    internal void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        _isCrouch = _statsData.isCrounchActionByToggle ? !_isCrouch : true;
    }

    internal void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        _isCrouch = _statsData.isCrounchActionByToggle ? _isCrouch : false;
    }

    internal void OnGlideCanceled(InputAction.CallbackContext context)
    {

    }

    internal void OnDashPerformed(InputAction.CallbackContext context) {
        if (!_isAbleToDash || _isDashing) return;
        _isAbleToDash = false;
        _isDashing = true;
        _calculatedVelocity.y = 0;
        StartCoroutine(StopDash());
        StartCoroutine(DashCooldown());
    }

    private IEnumerator StopDash() {
        yield return new WaitForSeconds(_statsData.DashTime);
        _isDashing = false;
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(_statsData.DashCooldown);
        _isAbleToDash = true;
    }

    private void UpdateMoveDir()
    {
        if (_isDashing || !_isGrounded) return;
        _moveDirection = _currentInput;
    }

    private void Move() {
        UpdateMoveDir();

        _calculatedVelocity.x = _isTouchingWall ? 0f : _isDashing ? (_statsData.DashSpeed * _moveDirection.x * Time.fixedDeltaTime) : _isCrouch ? (_statsData.CrounchSpeed * Time.fixedDeltaTime * (_isGrounded ? _currentInput.x : _moveDirection.x)) : (_statsData.WalkSpeed * Time.fixedDeltaTime * (_isGrounded ? _currentInput.x : _moveDirection.x));
    }
    internal void OnGlidePerformed(InputAction.CallbackContext context) {
        _isGlide = _statsData.isGlideActionByToggle ? !_isGlide : true;
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




    internal void OnJumpPerformed(InputAction.CallbackContext context)
    {
        _heldJump = true;
        _jumpPressTime = Time.time;
        _isJumpRequestExist = true;
    }

    private void ExecuteJump(int jumpType)
    { // 1 : bonus Jump
        _moveDirection = _currentInput;
        if (jumpType == 0)
        {
            _isJumped = true;
        }
        else if (jumpType == 1)
        {
            _leftBonusJump -= 1;
        }
        _calculatedVelocity.y = _statsData.JumpForce;
        _isJumpRequestExist = false;
    }

    internal void OnJumpCanceled(InputAction.CallbackContext context)
    {
        _heldJump = false;
    }

    private bool CheckJumpEndedBeforeApex()
    {
        return (_calculatedVelocity.y > 0 && !_isJumpEndedEarly && !_isGrounded && !_heldJump);
    }

    private void JumpRequestValidation()
    {
        if (_isDashing) return;
        _isJumpEndedEarly = CheckJumpEndedBeforeApex();

        bool jumpBufferValidation = ((_groundedTime - _jumpPressTime) < _statsData.JumpBufferTime) && _isGrounded;

        bool coyoteJumpValidation = (_jumpPressTime - _leftGroundTime) < _statsData.CoyoteTime;

        bool bonusJumpValidation = !_isGrounded && _leftBonusJump > 0;

        if (!_isJumpRequestExist && !jumpBufferValidation) return;

        bool canJump = (coyoteJumpValidation || jumpBufferValidation || bonusJumpValidation);
        if (canJump) ExecuteJump((!_isGrounded && !jumpBufferValidation && !coyoteJumpValidation) ? 1 : 0);
    }
    #endregion

    #region CollisionCheck
    private bool _isTouchingWall;
    internal bool _isGrounded = false;
    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        //check Ground hit
        bool groundCheck = CheckGround();

        //check wall hit
        bool wallCheck = CheckWall();

        //landed on ground
        if (groundCheck && !_isGrounded)
        {
            _isGrounded = true;
            _isJumpEndedEarly = false;
            _isJumped = false;
            _leftBonusJump = _statsData.bonusJump;
            _groundedTime = Time.time;

            //leave from ground
        }
        else if (!groundCheck && _isGrounded)
        {
            _isGrounded = false;
            _leftGroundTime = Time.time;
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
        return Physics2D.OverlapBox(groundCheckerTransform.position - new Vector3(0, _statsData.groundCheckDistance / 2), new Vector2(transform.localScale.x * 0.85f, _statsData.groundCheckDistance / 2), 0f, _statsData.groundLayer);
        //_isGrounded = Physics2D.OverlapCircle(groundCheckerTransform.position, groundCheckDistance, groundLayer);

    }
    private bool CheckWall()
    {
        Vector2 rayCastDir = (_currentInput.x > 0) ? Vector2.right : Vector2.left;

        foreach (Transform point in wallRaycastPoints)
        {
            RaycastHit2D hit = Physics2D.Raycast(point.position, rayCastDir, _statsData.wallCheckDistance, _statsData.groundLayer);
            //                                                                                              ^^^^^^^^^^^^^^^^^^^^  it's because the wall functions as the ground
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
    #region Gravity
    private void Gravity()
    {
        //touching the ground
        if (_isGrounded && _calculatedVelocity.y <= 0f) _calculatedVelocity.y = _statsData.GravityByNormalForce;

        //in mid-air
        else
        {
            float midAirGravity = _statsData.MidAirGravity;
            if (!_isGrounded && _isGlide && _calculatedVelocity.y < 0) midAirGravity = _statsData.MidAirGravity * _statsData.GlideGravity;
            else if ((_isJumped) && Mathf.Abs(_calculatedVelocity.y) < _statsData.ApexThreadHold) midAirGravity = _statsData.MidAirGravity * _statsData.ApexModifier;
            else if (_calculatedVelocity.y < 0f) midAirGravity = _statsData.MidAirGravity * _statsData.GravityModifierWhenFalling;
            else if (_isJumpEndedEarly) midAirGravity = _statsData.MidAirGravity * _statsData.GravityModifierWhenJumpEndedEarly;
            _calculatedVelocity.y = Mathf.MoveTowards(_calculatedVelocity.y, _statsData.MaxFallingSpeed, Time.fixedDeltaTime * midAirGravity);
        }
    }



    #endregion

    #region Debugging
    private void OnDrawGizmos()
    {
        if (groundCheckerTransform != null)
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(groundCheckerTransform.position - new Vector3(0, _statsData.groundCheckDistance / 2), new Vector2(transform.localScale.x, _statsData.groundCheckDistance / 2));
        }

        if (wallRaycastPoints != null && _moveDirection.x != 0)
        {
            Vector3 gizmoDirection = (_moveDirection.x > 0) ? Vector3.right : Vector3.left;
            Gizmos.color = _isTouchingWall ? Color.blue : Color.gray;
            foreach (Transform point in wallRaycastPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawLine(point.position, point.position + gizmoDirection * _statsData.wallCheckDistance);
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
                    Gizmos.DrawLine(point.position, point.position + Vector3.right * _statsData.wallCheckDistance);
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
        //Jump();
        JumpRequestValidation();
        _rb.linearVelocity = _calculatedVelocity;
    }
}