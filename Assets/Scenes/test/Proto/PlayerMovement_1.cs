using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement_1 : MonoBehaviour
{
    [SerializeField] private ScriptablePlayerStats _data;
    [Header("Transforms")]
    [SerializeField] private Transform groundCheckerTransform;
    [SerializeField] private Transform[] wallRaycastPoints;

    internal Vector2 _currentInput;
    private bool _isSprint = true;
    private Rigidbody2D _rb;

    private bool _cachedQueryStartInColliders;
    private Vector2 _calculatedVelocity;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
            Debug.LogError("Animator component is missing!");
        else
            _animator.applyRootMotion = false;
        _rb = GetComponent<Rigidbody2D>();
        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        _leftBonusJump = _data.bonusJump;
    }

    private void Update()
    {

    }

    #region Movement
    internal void OnMovePerformed(InputAction.CallbackContext context)
    {
        _currentInput = context.ReadValue<Vector2>();
    }

    internal void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _currentInput = Vector2.zero;
    }

    internal void OnSprintPerformed(InputAction.CallbackContext context)
    {
        _isSprint = false;
    }

    internal void OnSprintCanceled(InputAction.CallbackContext context)
    {
        _isSprint = true;
    }

    private void Move()
    {
        _calculatedVelocity.x = _isTouchingWall ? 0f : _isSprint ? (_currentInput.x * _data.RunSpeed * Time.fixedDeltaTime) : (_currentInput.x * _data.WalkSpeed * Time.fixedDeltaTime);
    }

    internal Vector2 ApplyMove()
    {
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
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL in OnJumpPerformed!");
            return;
        }
        _heldJump = true;
        _jumpPressTime = Time.time;
        _isJumpRequestExist = true;
        //_jumpBufferTimer = _data.JumpBufferTime;
        //Jump();
    }

    private void ExecuteJump(int jumpType)
    { // 1 : bonus Jump
        if (jumpType == 0)
        {
            _isJumped = true;
        }
        else if (jumpType == 1)
        {
            _leftBonusJump -= 1;
        }
        _calculatedVelocity.y = _data.JumpForce;
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
        _isJumpEndedEarly = CheckJumpEndedBeforeApex();

        bool jumpBufferValidation = ((_groundedTime - _jumpPressTime) < _data.JumpBufferTime) && _isGrounded;

        bool coyoteJumpValidation = (_jumpPressTime - _leftGroundTime) < _data.CoyoteTime;

        bool bonusJumpValidation = !_isGrounded && _leftBonusJump > 0;

        if (!_isJumpRequestExist && !jumpBufferValidation) return;

        bool canJump = (coyoteJumpValidation || jumpBufferValidation || bonusJumpValidation);
        if (canJump) ExecuteJump((!_isGrounded && !jumpBufferValidation && !coyoteJumpValidation) ? 1 : 0);
        if (_animator != null)
            _animator.SetTrigger("Jumped 0");
        _isJumpRequestExist = false;
    }
    private void HandleRotation()
    {
        if (_currentInput.x > 0) // D키
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (_currentInput.x < 0) // W키
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
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
            _leftBonusJump = _data.bonusJump;
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
        return Physics2D.OverlapBox(groundCheckerTransform.position - new Vector3(0, _data.groundCheckDistance / 2), new Vector2(transform.localScale.x, _data.groundCheckDistance / 2), 0f, _data.groundLayer);
        //_isGrounded = Physics2D.OverlapCircle(groundCheckerTransform.position, groundCheckDistance, groundLayer);

    }

    private bool CheckWall()
    {
        Vector2 rayCastDir = (_currentInput.x > 0) ? Vector2.right : Vector2.left;

        foreach (Transform point in wallRaycastPoints)
        {
            RaycastHit2D hit = Physics2D.Raycast(point.position, rayCastDir, _data.wallCheckDistance, _data.groundLayer);
            //                                                                                        ^^^^^^^^^^^^^^^^^^^^  it's because the wall functions as the ground
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
        if (_isGrounded && _calculatedVelocity.y <= 0f) _calculatedVelocity.y = _data.GravityByNormalForce;

        //in mid-air
        else
        {
            float midAirGravity = _data.MidAirGravity;
            if ((_isJumped) && Mathf.Abs(_calculatedVelocity.y) < _data.ApexThreadHold) midAirGravity = _data.MidAirGravity * _data.ApexModifier;
            else if (_calculatedVelocity.y < 0f) midAirGravity = _data.MidAirGravity * _data.GravityModifierWhenFalling;
            else if (_isJumpEndedEarly) midAirGravity = _data.MidAirGravity * _data.GravityModifierWhenJumpEndedEarly;
            _calculatedVelocity.y = Mathf.MoveTowards(_calculatedVelocity.y, _data.MaxFallingSpeed, Time.fixedDeltaTime * midAirGravity);
        }
    }



    #endregion

    #region Debugging
    private void OnDrawGizmos()
    {
        if (groundCheckerTransform != null)
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(groundCheckerTransform.position - new Vector3(0, _data.groundCheckDistance / 2), new Vector2(transform.localScale.x, _data.groundCheckDistance / 2));
        }

        if (wallRaycastPoints != null && _currentInput.x != 0)
        {
            Vector3 gizmoDirection = (_currentInput.x > 0) ? Vector3.right : Vector3.left;
            Gizmos.color = _isTouchingWall ? Color.blue : Color.gray;
            foreach (Transform point in wallRaycastPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawLine(point.position, point.position + gizmoDirection * _data.wallCheckDistance);
                }
            }
        }
        else if (wallRaycastPoints != null && _currentInput.x == 0)
        {
            Gizmos.color = Color.gray;
            foreach (Transform point in wallRaycastPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawLine(point.position, point.position + Vector3.right * _data.wallCheckDistance);
                }
            }
        }
    }
    #endregion

    private void LateUpdate()
    {
        UpdateAnimatorParameters();
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        Gravity();
        Move();
        //Jump();
        JumpRequestValidation();
        _rb.linearVelocity = _calculatedVelocity;

        HandleRotation();
        UpdateAnimatorParameters();
    }

    private void UpdateAnimatorParameters()
    {
        if (_animator == null) return;

        bool isPressingWD = _currentInput.x != 0; // W나 D 키를 누르고 있을 때만 true

        _animator.SetBool("isRunning", _isSprint && isPressingWD);
        _animator.SetBool("isWalking", !_isSprint && isPressingWD);

    }
}