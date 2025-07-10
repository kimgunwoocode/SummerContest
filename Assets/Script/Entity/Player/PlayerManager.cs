using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerManager : AbstractEntity {
    [Header("Jump System")]
    [SerializeField] private float initialJumpForce;
    [SerializeField] private float jumpMultipler;
    [SerializeField] private float maxJumpTime;
    private float jumpTimer;
    private bool isJumping;

    private PlayerInput_Action inputActions;
    private Vector2 currentInput;
    private Rigidbody2D rb;
    private Vector2 initialGravity;



    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        initialGravity = Physics2D.gravity;
        inputActions = new PlayerInput_Action();

        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Jump.canceled += OnJumpCanceled;
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnEnable() {
        inputActions.Player.Enable();
    }

    private void OnDisable() {
        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.Jump.canceled -= OnJumpCanceled;
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        
        inputActions.Player.Disable();
    }

    public override void Attack() {
    }

    protected override void Move() {
        rb.linearVelocity = new Vector2(currentInput.x * walkSpeed * Time.deltaTime, rb.linearVelocityY);
    }

    private void OnMovePerformed(InputAction.CallbackContext context) {
        currentInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context) {
        currentInput = Vector2.zero;
    }
    
    private void OnJumpPerformed(InputAction.CallbackContext context) {
        if (isGrounded) {
            rb.linearVelocityY = initialJumpForce;
            isJumping = true;
        }
    }

    private void Jump() {
        if (isJumping && rb.linearVelocityY > 0) {
            jumpTimer += Time.deltaTime;
            if (jumpTimer > maxJumpTime) isJumping = false;
            rb.linearVelocityY += Mathf.Abs(initialGravity.y) * jumpMultipler * Time.fixedDeltaTime;
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext context) {
        isJumping = false;
        jumpTimer = 0;
    }


    public override void TakeDamage(int damage, int hitDir){
    }

    protected override void Die(){
    }


    private void CheckGround() {
        isGrounded = Physics2D.OverlapCircle(groundCheckerTransform.position, groundCheckDistance, groundLayer);
        rb.gravityScale = isGrounded ? 2 : 4;
    }

    private void Update() {
        CheckGround();
    }


    private void FixedUpdate() {
        Move();
        Jump();
    }
}
