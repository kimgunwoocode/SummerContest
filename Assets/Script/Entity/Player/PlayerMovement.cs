using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement Info")]
    [SerializeField] private float walkSpeed;

    [Header("Wall Check")]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform[] wallRaycastPoints;
    private bool isTouchingWall;

    [Header("Jump System")]
    [SerializeField] private float initialJumpForce;
    [SerializeField] private float jumpMultipler;
    [SerializeField] private float maxJumpTime;

    [Header("Ground Check")]
    [SerializeField] protected float groundCheckDistance = 0.1f;
    [SerializeField] protected Transform groundCheckerTransform;
    [SerializeField] protected LayerMask groundLayer;
    internal bool isGrounded;

    [Header("Jump Utils(for responsive and smoothness control)")]
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private bool isJumping;

    internal Vector2 currentInput;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    internal void OnMovePerformed(InputAction.CallbackContext context) {
        currentInput = context.ReadValue<Vector2>();
    }

    internal void OnMoveCanceled(InputAction.CallbackContext context) {
        currentInput = Vector2.zero;
    }

    internal void OnJumpPerformed(InputAction.CallbackContext context) {
        jumpBufferCounter = jumpBufferTime;
    }

    private void Jump() {
        if (coyoteCounter > 0 && jumpBufferCounter > 0) {
            rb.linearVelocityY = initialJumpForce;
            jumpBufferCounter = 0f;
            isJumping = true;
        }
    }

    internal void OnJumpCanceled(InputAction.CallbackContext context) {
        coyoteCounter = 0f;
    }

    internal Vector2 GetMoveValue() {
        return new Vector2(isTouchingWall ? 0f : (currentInput.x * walkSpeed * Time.deltaTime), rb.linearVelocityY);
    }

    internal void CheckGround() {
        isGrounded = Physics2D.OverlapBox(groundCheckerTransform.position - new Vector3(0, groundCheckDistance / 2), new Vector2(transform.localScale.x, groundCheckDistance/2), 0f, groundLayer);
        isGrounded = Physics2D.OverlapCircle(groundCheckerTransform.position, groundCheckDistance, groundLayer);
       
    }

    internal void CheckWall() {
        isTouchingWall = false;

        if (currentInput.x == 0) return;

        Vector2 rayCastDir = (currentInput.x > 0) ? Vector2.right : Vector2.left;

        foreach(Transform point in wallRaycastPoints) {
            RaycastHit2D hit = Physics2D.Raycast(point.position, rayCastDir, wallCheckDistance, groundLayer);
            //                                                                                  ^^^^^^^^^^^  it's because the wall functions as the ground
            if(hit.collider != null) {
                Debug.Log("yes! a hit!");
                isTouchingWall = true;
                break;
            }
        }
    }
    private void OnDrawGizmos() {
        if (groundCheckerTransform != null) {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(groundCheckerTransform.position - new Vector3(0, groundCheckDistance / 2), new Vector2(transform.localScale.x, groundCheckDistance/2));
        }

        if (wallRaycastPoints != null && currentInput.x != 0) {
            Vector3 gizmoDirection = (currentInput.x > 0) ? Vector3.right : Vector3.left;
            Gizmos.color = isTouchingWall ? Color.blue : Color.gray;
            foreach (Transform point in wallRaycastPoints) {
                if (point != null) {
                    Gizmos.DrawLine(point.position, point.position + gizmoDirection * wallCheckDistance);
                }
            }
        } else if (wallRaycastPoints != null && currentInput.x == 0) {
            Gizmos.color = Color.gray;
            foreach (Transform point in wallRaycastPoints) {
                if (point != null) {
                    Gizmos.DrawLine(point.position, point.position + Vector3.right * wallCheckDistance);
                }
            }
        }
    }

    private void FixedUpdate() {
        CheckGround();
        CheckWall();
        Jump();

        if (isGrounded) {
            coyoteCounter = coyoteTime;
        } else {
            coyoteCounter -= Time.fixedDeltaTime;
        }

        jumpBufferCounter -= Time.fixedDeltaTime;
    }
}