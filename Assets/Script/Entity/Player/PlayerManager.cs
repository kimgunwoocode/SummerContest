using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerManager : AbstractEntity {
    private PlayerInput_Action inputActions;
    private Rigidbody2D rb;

    private PlayerMovement movement;
    private GameDataManager data;

    private int maxHealth;
    private int currentHealth;
    


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        data = Singleton.GameManager_Instance.Get<GameDataManager>();
        if (data == null) Debug.LogError("Can't found GameDataManager at GameManager");

        maxHealth = data.MaxHP;
        currentHealth = data.CurrentHP;
    }

    private void OnEnable() {
        movement = GetComponent<PlayerMovement>();
        if (movement == null)
        {
            Debug.LogError("PlayerMovement 컴포넌트를 찾을 수 없습니다!");
            return;
        }

        inputActions = new PlayerInput_Action();

        inputActions.Player.Jump.performed += movement.OnJumpPerformed;
        inputActions.Player.Jump.canceled += movement.OnJumpCanceled;

        inputActions.Player.Move.performed += movement.OnMovePerformed;
        inputActions.Player.Move.canceled += movement.OnMoveCanceled;

        inputActions.Player.Crouch.performed += movement.OnCrouchPerformed;
        inputActions.Player.Crouch.canceled += movement.OnCrouchCanceled;

        inputActions.Player.Dash.performed += movement.OnDashPerformed;
        //inputActions.Player.Dash.canceled += movement.OnDashCanceled;

        inputActions.Player.Enable();
    }

    private void OnDisable() {
        inputActions.Player.Jump.performed -= movement.OnJumpPerformed;
        inputActions.Player.Jump.canceled -= movement.OnJumpCanceled;

        inputActions.Player.Move.performed -= movement.OnMovePerformed;
        inputActions.Player.Move.canceled -= movement.OnMoveCanceled;

        inputActions.Player.Crouch.performed -= movement.OnCrouchPerformed;
        inputActions.Player.Crouch.canceled -= movement.OnCrouchCanceled;

        inputActions.Player.Dash.performed -= movement.OnDashPerformed;
        //inputActions.Player.Dash.canceled -= movement.OnDashCanceled;

        inputActions.Player.Disable();
    }

    public override void Attack() {
    }

    protected override void Move() {
        //rb.linearVelocity = movement.ApplyMove();
    }

    /*
    public override void TakeDamage(int damage, int hitDir){

    }

    protected override void Die(){

    }

    private void Knockback(int dir) {

    }
    */

    private void Update() {
    }


    private void FixedUpdate() {
        Move();
    }
}
