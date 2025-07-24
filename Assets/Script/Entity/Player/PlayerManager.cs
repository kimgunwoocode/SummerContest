using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerManager : MonoBehaviour {
    private PlayerInput_Action _inputActions;
    private Rigidbody2D _rb;

    [Header("Stats")]
    [SerializeField] internal ScriptablePlayerMovementStats playerMovementStats;
    [SerializeField] internal ScriptablePlayerAttackStats playerAttackStats;
    
    [Space(30)]
    [Header("camera")]
    [SerializeField] private Camera cam;
    private Vector3 _mousePosition;

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
        _attack = GetComponent<PlayerAttack>();
        _anima = GetComponent<PlayerAnimation>();
    }

    private void Start() {
        _data = Singleton.GameManager_Instance.Get<GameDataManager>();
        if (_data == null) Debug.LogError("Can't found GameDataManager at GameManager");
        if (_attack == null) Debug.LogError("PlayerAttack component must exist on this object");
        if (_movement == null) Debug.LogError("PlayerMovement component must exist on this object");
        if (_interaction == null) Debug.LogError("PlayerInteraction component must exist on this object");
        if (_anima == null) Debug.LogError("Missing required component: PlayerAnimation");
        if (playerMovementStats == null) Debug.LogError("Missing required component: PlayerMovementStats");
        if (playerAttackStats == null) Debug.LogError("Missing required component: PlayerAttackStats");


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

        _inputActions.Player.Attack.performed += Attack;

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

        _inputActions.Player.Attack.performed += Attack;

        _inputActions.Player.Disable();
    }

    public void Attack(InputAction.CallbackContext context) {
        _attack.MeleeAttack((_mousePosition - transform.position).normalized);
    }

    protected void Move() {
        //rb.linearVelocity = movement.ApplyMove();
    }

    
    public void TakeDamage(int damage, int hitDir){
        Debug.Log("Player has been damaged");
    }

    public void TakeDamage(int damage, Vector3 hitDir) {
        _currentHealth -= damage;

        if (_currentHealth <= 0) {
            Die();
            return;
        }

        Knockback(hitDir.x > 0 ? 1 : -1);
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
        if (id == 0) {
            playerMovementStats.IsDashUnlocked = true;
        }else if(id == 1) {

        }else if(id == 2) {
            playerMovementStats.IsDoubleJumpUnloceked = true;
        }else if(id == 3) {

        }else if(id == 4) {
            playerMovementStats.IsGlideUnlocked = true;
        } else if(id == 5) {

        }
    }

    private void Update() {
        _mousePosition = cam.WorldToScreenPoint(Mouse.current.position.ReadValue());
    }


    private void FixedUpdate() {
        Move();
    }
}
