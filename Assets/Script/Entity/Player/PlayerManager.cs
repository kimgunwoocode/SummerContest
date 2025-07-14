using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerManager : AbstractEntity {

    private PlayerInput_Action inputActions;
    private Rigidbody2D rb;
    private Vector2 initialGravity;

    private PlayerMovement movement;



    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();

        initialGravity = Physics2D.gravity;
        inputActions = new PlayerInput_Action();

        inputActions.Player.Jump.performed += movement.OnJumpPerformed;
        inputActions.Player.Jump.canceled += movement.OnJumpCanceled;
        inputActions.Player.Move.performed += movement.OnMovePerformed;
        inputActions.Player.Move.canceled += movement.OnMoveCanceled;
    }

    private void OnEnable() {
        inputActions.Player.Enable();
    }

    private void OnDisable() {
        inputActions.Player.Jump.performed -= movement.OnJumpPerformed;
        inputActions.Player.Jump.canceled -= movement.OnJumpCanceled;
        inputActions.Player.Move.performed -= movement.OnMovePerformed;
        inputActions.Player.Move.canceled -= movement.OnMoveCanceled;
        
        inputActions.Player.Disable();
    }

    public override void Attack() {
    }

    protected override void Move() {
        rb.linearVelocity = movement.GetMoveValue();
    }

    /*
    public override void TakeDamage(int damage, int hitDir){
    }
    protected override void Die(){
    }
    */

    private void Update() {
    }


    private void FixedUpdate() {
        Move();
        //Jump();
    }
}
