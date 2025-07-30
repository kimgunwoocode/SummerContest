using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class PlayerUtilState {
    private List<bool> PlayerUtilList;

    public void Setter(int index, bool value) {
        if (index < 0 || index > PlayerUtilList.Count) { 
            Debug.LogError("The index must be between 0 and 5 (inclusive).");
            return;
        }
        PlayerUtilList[index] = value;

    }

    public bool Getter(int index) {
        if (index < 0 || index > PlayerUtilList.Count) {
            Debug.LogError("The index must be between 0 and 5 (inclusive).");
            return false;
        }
        return PlayerUtilList[index];
    }
}

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
    private GameManager _manager;

    private int _maxHealth;
    private int _currentHealth;

    internal List<bool> Abilitis;


    private void Awake() {
        _manager = Singleton.GameManager_Instance.Get<GameManager>();
        _data = Singleton.GameManager_Instance.Get<GameDataManager>();

        _rb = GetComponent<Rigidbody2D>();

        _movement = GetComponent<PlayerMovement>();
        _interaction = GetComponent<PlayerInteraction>();
        _attack = GetComponent<PlayerAttack>();
        _anima = GetComponent<PlayerAnimation>();
    }

    private void Start() {
        if (_data == null) Debug.LogError("Can't found GameDataManager at GameManager");
        if (_attack == null) Debug.LogError("PlayerAttack component must exist on this object");
        if (_movement == null) Debug.LogError("PlayerMovement component must exist on this object");
        if (_interaction == null) Debug.LogError("PlayerInteraction component must exist on this object");
        if (_anima == null) Debug.LogError("Missing required component: PlayerAnimation");
        if (playerMovementStats == null) Debug.LogError("Missing required component: PlayerMovementStats");
        if (playerAttackStats == null) Debug.LogError("Missing required component: PlayerAttackStats");
        SaveFileManager.Load(0);

        LoadData(-1);
        _attack.InitiateBreath();

        _maxHealth = _data.MaxHP;
        _currentHealth = _data.CurrentHP;
        Abilitis = _data.PlayerAbility;
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
        _inputActions.Player.Breath.performed += Attack;

        _inputActions.Player.Down.performed += _movement.OnPlatformDown;

        _inputActions.Player.Enable();
    }

    private void OnDisable() {
        _inputActions.Player.Jump.performed -= _movement.OnJumpPerformed;
        _inputActions.Player.Jump.canceled -= _movement.OnJumpCanceled;

        _inputActions.Player.Move.performed -= _movement.OnMovePerformed;
        _inputActions.Player.Move.canceled -= _movement.OnMoveCanceled;

        _inputActions.Player.Crouch.performed -= _movement.OnCrouchPerformed;
        _inputActions.Player.Crouch.canceled -= _movement.OnCrouchCanceled;

        _inputActions.Player.Glide.performed -= _movement.OnGlidePerformed;
        _inputActions.Player.Glide.canceled -= _movement.OnGlideCanceled;

        _inputActions.Player.Dash.performed -= _movement.OnDashPerformed;

        _inputActions.Player.Interact.performed -= _interaction.OnInteraction;

        _inputActions.Player.Attack.performed -= Attack;
        _inputActions.Player.Breath.performed -= Attack;

        _inputActions.Player.Down.performed -= _movement.OnPlatformDown;

        _inputActions.Player.Disable();
    }

    #region Util


    private void LoadData(int id) {
        Abilitis = _data.PlayerAbility;
    }

    #endregion

    private void Attack(InputAction.CallbackContext context) {
        if (context.action.name == "Attack")
            _attack.MeleeAttack((_mousePosition - transform.position).normalized);
        else if (context.action.name == "Breath") {
            if (!Abilitis[1]) return;
            _attack.FireBreath((_mousePosition - transform.position).normalized);
        }
    }

    
    public void TakeDamage(int damage, int hitDir){
        Debug.Log("Player has been damaged");
    }

    public void TakeDamage(int damage, Vector3 hitDir) {
        _currentHealth -= damage;
        _data.CurrentHP = _currentHealth;
        if (_currentHealth <= 0) {
            Die();
            return;
        }

        Knockback(hitDir.x > 0 ? 1 : -1);
    }

    private void Die(){
        _manager.PlayerDie();
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
            LoadData(0);
            //SetData(1500);
        }else if(id == 1) {
            LoadData(1);
            //SetData(1501);
        } else if(id == 2) {
            LoadData(2);
            //SetData(1502);
        } else if (id == 3) {
            LoadData(3);
            //SetData(1503);
        } else if (id == 4) {
            LoadData(4);
            //SetData(1504);
        } else if (id == 5) {
            LoadData(5);
            //SetData(1505);
        }

    }

    private void Update() {
        _mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
