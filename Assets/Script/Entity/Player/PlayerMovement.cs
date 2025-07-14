using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField] private ScriptablePlayerStats _stats;
    [Header("Transforms")]
    [SerializeField] private Transform groundCheckerTransform;
    [SerializeField] private Transform[] wallRaycastPoints;

    internal Vector2 _currentInput;
    private Rigidbody2D _rb;

    private bool _cachedQueryStartInColliders;
    private Vector2 _calculatedVelocity;
    private float _time;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }

    private void Update() {
        if (_isGrounded) {
            _coyoteTimer = _stats.CoyoteTime;
        } else {
            _coyoteTimer -= Time.deltaTime;
        }
        if (_jumpBufferTimer > 0) _jumpBufferTimer -= Time.deltaTime;
        //Debug.Log( _jumpBufferTimer + ", " + _isAbleToJumpBuffer);

        if (_isJumpEndedEarly) Debug.Log(true);

        
        
    }

    #region Movement
    internal void OnMovePerformed(InputAction.CallbackContext context) {
        _currentInput = context.ReadValue<Vector2>();
    }

    internal void OnMoveCanceled(InputAction.CallbackContext context) {
        _currentInput = Vector2.zero;
    }

    private void Move() {
        _calculatedVelocity.x = _isTouchingWall ? 0f : (_currentInput.x * _stats.WalkSpeed * Time.deltaTime);
    }

    internal Vector2 ApplyMove() {
        return _calculatedVelocity;
    }
    #endregion

    #region Jump
    private bool _isAbleToCoyoteJump;
    //private bool _isAbleToJumpBuffer;
    private bool _isJumpEndedEarly;
    private bool _heldJump;

    private float _jumpBufferTimer;
    private float _coyoteTimer;



    internal void OnJumpPerformed(InputAction.CallbackContext context) {
        _heldJump = true;
        _jumpBufferTimer = _stats.JumpBufferTime;
    }

    private void Jump() {
        _isJumpEndedEarly = CheckJumpEndedBeforeApex();
        if (_coyoteTimer > 0 && _jumpBufferTimer > 0) {
            _calculatedVelocity.y = _stats.JumpForce;
            _jumpBufferTimer = 0f;
        }
    }

    internal void OnJumpCanceled(InputAction.CallbackContext context) {
        _coyoteTimer = 0f;
        _heldJump = false;
    }

    private bool CheckJumpEndedBeforeApex() {
        return (_calculatedVelocity.y > 0 && !_isJumpEndedEarly && !_isGrounded && !_heldJump);
    }

    private bool JumpRequestValidation() {

        bool jumpBufferVaildation = (_jumpBufferTimer > 0) && (_isGrounded);

        bool coyoteJumpVaildation = (_isAbleToCoyoteJump) && (!_isGrounded);

        if (jumpBufferVaildation || coyoteJumpVaildation) return true;
        else return false;
    }

    #endregion

    #region CollisionCheck
    private bool _isTouchingWall;
    internal bool _isGrounded = false;
    private void CheckCollisions() {
        Physics2D.queriesStartInColliders = false;

        //check Ground hit
        bool groundCheck = CheckGround();

        //check wall hit
        bool wallCheck = CheckWall();

        //landed on ground
        if(groundCheck && !_isGrounded) {
            _isGrounded = true;
            _isAbleToCoyoteJump = true;
            //_isAbleToJumpBuffer = true;
            _isJumpEndedEarly = false;
        
        //leave from ground
        }else if(!groundCheck && _isGrounded) {
            _isGrounded = false;
        }

        if(wallCheck && !_isTouchingWall) {
            _isTouchingWall = true;

        }else if(!wallCheck && _isTouchingWall) {
            _isTouchingWall = false;
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }

    private bool CheckGround() {
        return Physics2D.OverlapBox(groundCheckerTransform.position - new Vector3(0, _stats.groundCheckDistance / 2), new Vector2(transform.localScale.x, _stats.groundCheckDistance /2), 0f, _stats.groundLayer);
        //_isGrounded = Physics2D.OverlapCircle(groundCheckerTransform.position, groundCheckDistance, groundLayer);
       
    }

    private bool CheckWall() {
        //if (_currentInput.x == 0) return false;

        Vector2 rayCastDir = (_currentInput.x > 0) ? Vector2.right : Vector2.left;

        foreach(Transform point in wallRaycastPoints) {
            RaycastHit2D hit = Physics2D.Raycast(point.position, rayCastDir, _stats.wallCheckDistance, _stats.groundLayer);
            //                                                                                        ^^^^^^^^^^^^^^^^^^^^  it's because the wall functions as the ground
            if(hit.collider != null) {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Gravity
    private void Gravity() {
        if (_isGrounded && _calculatedVelocity.y <= 0f) _calculatedVelocity.y = _stats.GravityByNormalForce;
        else {
            float midAirGravity = _stats.MidAirGravity;
            if (_calculatedVelocity.y < 0f) midAirGravity = _stats.MidAirGravity * _stats.GravityModifierWhenFalling;
            else if (_isJumpEndedEarly) midAirGravity = _stats.MidAirGravity * _stats.GravityModifierWhenJumpEndedEarly;
            _calculatedVelocity.y = Mathf.MoveTowards(_calculatedVelocity.y, _stats.MaxFallingSpeed, Time.fixedDeltaTime * midAirGravity);
        }
    }


    #endregion
   
    #region Debugging
    private void OnDrawGizmos() {
        if (groundCheckerTransform != null) {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(groundCheckerTransform.position - new Vector3(0, _stats.groundCheckDistance / 2), new Vector2(transform.localScale.x, _stats.groundCheckDistance /2));
        }

        if (wallRaycastPoints != null && _currentInput.x != 0) {
            Vector3 gizmoDirection = (_currentInput.x > 0) ? Vector3.right : Vector3.left;
            Gizmos.color = _isTouchingWall ? Color.blue : Color.gray;
            foreach (Transform point in wallRaycastPoints) {
                if (point != null) {
                    Gizmos.DrawLine(point.position, point.position + gizmoDirection * _stats.wallCheckDistance);
                }
            }
        } else if (wallRaycastPoints != null && _currentInput.x == 0) {
            Gizmos.color = Color.gray;
            foreach (Transform point in wallRaycastPoints) {
                if (point != null) {
                    Gizmos.DrawLine(point.position, point.position + Vector3.right * _stats.wallCheckDistance);
                }
            }
        }
    }
    #endregion

    private void FixedUpdate() {
        CheckCollisions();
        Gravity();
        Move();
        Jump();

        _rb.linearVelocity = _calculatedVelocity;
    }
}