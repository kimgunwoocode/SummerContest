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
        movement = GetComponent<PlayerMovement>();

        inputActions = new PlayerInput_Action();

        inputActions.Player.Jump.performed += movement.OnJumpPerformed;
        inputActions.Player.Jump.canceled += movement.OnJumpCanceled;

        inputActions.Player.Move.performed += movement.OnMovePerformed;
        inputActions.Player.Move.canceled += movement.OnMoveCanceled;

        inputActions.Player.Sprint.performed += movement.OnSprintPerformed;
        inputActions.Player.Sprint.canceled += movement.OnSprintCanceled;
    }

    private void Start() {
        data = Singleton.Get<GameDataManager>();
        if (data == null) Debug.LogError("Can't found GameDataManager at GameManager");

        maxHealth = data.MaxHP;
        currentHealth = maxHealth;
    }

    private void OnEnable() {
        inputActions.Player.Enable();
    }

    private void OnDisable() {
        inputActions.Player.Jump.performed -= movement.OnJumpPerformed;
        inputActions.Player.Jump.canceled -= movement.OnJumpCanceled;

        inputActions.Player.Move.performed -= movement.OnMovePerformed;
        inputActions.Player.Move.canceled -= movement.OnMoveCanceled;
        
        inputActions.Player.Sprint.performed -= movement.OnSprintPerformed;
        inputActions.Player.Sprint.canceled -= movement.OnSprintCanceled;

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
