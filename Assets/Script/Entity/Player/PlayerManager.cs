using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour {
    private PlayerInput_Action _inputActions;
    private Rigidbody2D _rb;
    internal bool IsInvincible;
    private bool isControllablePlayer = true;

    [Header("Stats")]
    [SerializeField] internal ScriptablePlayerMovementStats playerMovementStats;
    [SerializeField] internal ScriptablePlayerAttackStats playerAttackStats;
    [SerializeField] private float invinsibleTime = 0.1f;
    
    [Space(30)]
    [Header("camera")]
    [SerializeField] private Camera cam;
    private Vector3 _mousePosition;
    [Header("debug")]
    [Tooltip("DO NOT TURN ON IN BULID TEST VERSION.")][SerializeField] private bool isTestingEnvironment = false;

    private UIManager _ui;
    private PlayerMovement _movement;
    private PlayerAttack _attack;
    internal PlayerAnimation Anima;
    private PlayerInteraction _interaction;
    private GameDataManager _data;
    private GameManager _manager;


    internal List<bool> Abilitis;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Title")
            return;
        if (_ui == null) {
            _ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        }
    }

    private void Awake() {
        IsInvincible = false;
        _manager = Singleton.GameManager_Instance.Get<GameManager>();
        _data = Singleton.GameManager_Instance.Get<GameDataManager>();

        _rb = GetComponent<Rigidbody2D>();

        _movement = GetComponent<PlayerMovement>();
        _interaction = GetComponent<PlayerInteraction>();
        _attack = GetComponent<PlayerAttack>();
        Anima = GetComponent<PlayerAnimation>();

        if(_ui == null) _ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    private void Start() {
        if (_data == null) Debug.LogError("Can't found GameDataManager at GameManager");
        if (_attack == null) Debug.LogError("PlayerAttack component must exist on this object");
        if (_movement == null) Debug.LogError("PlayerMovement component must exist on this object");
        if (_interaction == null) Debug.LogError("PlayerInteraction component must exist on this object");
        if (Anima == null) Debug.LogError("Missing required component: PlayerAnimation");
        if (playerMovementStats == null) Debug.LogError("Missing required component: PlayerMovementStats");
        if (playerAttackStats == null) Debug.LogError("Missing required component: PlayerAttackStats");
        
        if (isTestingEnvironment) {
        }

        LoadData();
        _attack.InitiateBreath();

        Abilitis = _data.PlayerAbility;
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;

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

        _inputActions.Player.Pause.performed += OnPause;

        _inputActions.Player.Enable();
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;

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

        _inputActions.Player.Pause.performed -= OnPause;

        _inputActions.Player.Disable();
    }

    #region Util
    private void LoadData() {
        Abilitis = _data.PlayerAbility;
    }
    #endregion

    internal void SetControllable(bool value) {
        isControllablePlayer = value;
    }

    internal bool GetControllable() {
        return isControllablePlayer;
    }

    private void OnPause(InputAction.CallbackContext context) {
        _ui.Pausing();
    }

    private void Attack(InputAction.CallbackContext context) {
        if (!isControllablePlayer) return;
        if (context.action.name == "Attack")
            _attack.MeleeAttack((_mousePosition - transform.position).normalized);
        else if (context.action.name == "Breath") {
            if (!Abilitis[1]) return;
            _attack.FireBreath((_mousePosition - transform.position).normalized);
        }
    }

    public bool TakeDamage(int damage, Vector3 attackerPosition) {
        if (IsInvincible) return false;
        StartCoroutine(Invinsible());
        _data.CurrentHP -= damage;
        if (_data.CurrentHP <= 0) {
            Die();
            return true;
        }
        _movement.Knockback(attackerPosition);
        return true;
    }

    private IEnumerator Invinsible() {
        IsInvincible = true;
        yield return new WaitForSeconds(invinsibleTime);
        IsInvincible = false;
    }

    public void Knockback(Vector3 attackerPosition, int power = 4, float stunTime = 1f) {
        _movement.Knockback(attackerPosition, power, stunTime);
    }

    private void Die(){
        _manager.PlayerDie();
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
            LoadData();
            //SetData(1500);
        }else if(id == 1) {
            LoadData();
            //SetData(1501);
        } else if(id == 2) {
            LoadData();
            //SetData(1502);
        } else if (id == 3) {
            LoadData();
            //SetData(1503);
        } else if (id == 4) {
            LoadData();
            //SetData(1504);
        } else if (id == 5) {
            LoadData();
            //SetData(1505);
        }

    }




    private void Update() {
        _mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
