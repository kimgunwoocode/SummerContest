using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerManager : AbstractEntity {
    private PlayerInput_Action _inputActions;
    private Rigidbody2D _rb;

    private PlayerMovement _movement;
    private PlayerAttack _attack;
    private PlayerAnimation _anima;
    private PlayerInteraction _interaction;
    private GameDataManager _data;

    private int _maxHealth;
    private int _currentHealth;


    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _movement = GetComponent<PlayerMovement>();
        _interaction = GetComponent<PlayerInteraction>();
    }

    private void Start() {
        _data = Singleton.GameManager_Instance.Get<GameDataManager>();
        if (_data == null) Debug.LogError("Can't found GameDataManager at GameManager");

        _maxHealth = _data.MaxHP;
        _currentHealth = _data.CurrentHP;
    }

    private void OnEnable() {
        _inputActions = new PlayerInput_Action();

        _inputActions.Player.Jump.performed += _movement.OnJumpPerformed;
        _inputActions.Player.Jump.canceled += _movement.OnJumpCanceled;

        _inputActions.Player.Move.performed += _movement.OnMovePerformed;
        _inputActions.Player.Move.canceled += _movement.OnMoveCanceled;

        _inputActions.Player.Crouch.performed += _movement.OnCrouchPerformed;
        _inputActions.Player.Crouch.canceled += _movement.OnCrouchCanceled;

        _inputActions.Player.Glide.performed += _movement.OnGlidePerformed;
        _inputActions.Player.Glide.canceled += _movement.OnGlideCanceled;

        _inputActions.Player.Dash.performed += _movement.OnDashPerformed;

        _inputActions.Player.Interact.performed += _interaction.OnInteraction;

        _inputActions.Player.Enable();
    }

    private void OnDisable() {
        _inputActions.Player.Jump.performed -= _movement.OnJumpPerformed;
        _inputActions.Player.Jump.canceled -= _movement.OnJumpCanceled;

        _inputActions.Player.Move.performed -= _movement.OnMovePerformed;
        _inputActions.Player.Move.canceled -= _movement.OnMoveCanceled;

        _inputActions.Player.Crouch.performed -= _movement.OnCrouchPerformed;
        _inputActions.Player.Crouch.canceled -= _movement.OnCrouchCanceled;

        _inputActions.Player.Glide.performed += _movement.OnGlidePerformed;
        _inputActions.Player.Glide.canceled += _movement.OnGlideCanceled;

        _inputActions.Player.Dash.performed -= _movement.OnDashPerformed;

        _inputActions.Player.Interact.performed -= _interaction.OnInteraction;

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

    internal void UnlockAbility(int id) {
        /// <summary>
        /// ID. 해금되는 기능
        /// 0. 돌진
        /// 1. 브레스
        /// 2. 이단 점프
        /// 3. 낙하공격
        /// 4. 활공
        /// 5. 벽타기
        /// </summary>
    }

    private void Update() {

    }


    private void FixedUpdate() {
        Move();
    }
}
