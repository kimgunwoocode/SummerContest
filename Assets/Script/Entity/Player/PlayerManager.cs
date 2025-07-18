using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerManager : AbstractEntity {
    private PlayerInput_Action _inputActions;
    private Rigidbody2D _rb;

    private PlayerMovement _movement;
    private PlayerAttack _attack;
    private PlayerAnimation _anima;
    private GameDataManager _data;

    private int _maxHealth;
    private int _currentHealth;
    


    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _anima = GetComponent<PlayerAnimation>();
        _attack = GetComponent<PlayerAttack>();
    }

    private void Start() {
        _data = Singleton.GameManager_Instance.Get<GameDataManager>();
        if (_data == null) Debug.LogError("Can't found GameDataManager at GameManager");

        _maxHealth = _data.MaxHP;
        _currentHealth = _data.CurrentHP;
    }

    private void OnEnable() {
        _movement = GetComponent<PlayerMovement>();
        if (_movement == null) {
            Debug.LogError("PlayerMovement 컴포넌트를 찾을 수 없습니다!");
            return;
        }

        _inputActions = new PlayerInput_Action();

        _inputActions.Player.Jump.performed += _movement.OnJumpPerformed;
        _inputActions.Player.Jump.canceled += _movement.OnJumpCanceled;

        _inputActions.Player.Move.performed += _movement.OnMovePerformed;
        _inputActions.Player.Move.canceled += _movement.OnMoveCanceled;

        _inputActions.Player.Crouch.performed += _movement.OnCrouchPerformed;
        _inputActions.Player.Crouch.canceled += _movement.OnCrouchCanceled;

        //_inputActions.Player.Glide

        _inputActions.Player.Dash.performed += _movement.OnDashPerformed;
        //inputActions.Player.Dash.canceled += movement.OnDashCanceled;

        _inputActions.Player.Enable();
    }

    private void OnDisable() {
        _inputActions.Player.Jump.performed -= _movement.OnJumpPerformed;
        _inputActions.Player.Jump.canceled -= _movement.OnJumpCanceled;

        _inputActions.Player.Move.performed -= _movement.OnMovePerformed;
        _inputActions.Player.Move.canceled -= _movement.OnMoveCanceled;

        _inputActions.Player.Crouch.performed -= _movement.OnCrouchPerformed;
        _inputActions.Player.Crouch.canceled -= _movement.OnCrouchCanceled;

        _inputActions.Player.Dash.performed -= _movement.OnDashPerformed;
        //inputActions.Player.Dash.canceled -= movement.OnDashCanceled;

        _inputActions.Player.Disable();
    }

    public override void Attack() {
    }

    protected override void Move() {
        //rb.linearVelocity = movement.ApplyMove();
    }

    
    public void TakeDamage(int damage, int hitDir){

    }

    private void Die(){

    }

    private void Knockback(int dir) {

    }

    private void Update() {
    }


    private void FixedUpdate() {
        Move();
    }
}
