using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerManager : AbstractEntity {
    [SerializeField] private float smallJumpForce;
    [SerializeField] private float bigJumpForce;

    private PlayerInput_Action inputActions;
    private Vector2 currentInput;
    private Rigidbody2D rb;
    private bool isInAir;
    private double holdingTime;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInput_Action();

        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnEnable() {
        inputActions.Player.Enable();
    }

    private void OnDisable() {
        inputActions.Player.Disable();
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
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
    
    private void OnJump(InputAction.CallbackContext context) {
        if(context.interaction is HoldInteraction) {
            if(context.duration > 0.5 && context.duration < 1.2) {
                holdingTime = context.duration;
            }else if(context.duration > 1.2) {
                holdingTime = 1.2;
            } else {
                holdingTime = 0.5;
            }
        }else if(context.interaction is PressInteraction) {

        }
    }


    private void CheckGround() {
        isGrounded = Physics2D.OverlapCircle(groundCheckerTransform.position, groundCheckDistance, groundLayer);
        isInAir = isGrounded ? false : isInAir;
    }

    private void Update() {
        Move();
        CheckGround();
    }


    private void FixedUpdate() {
        Move();
    }
}
