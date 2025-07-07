using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    [Header("상호작용시 실행할 이벤트 (메서드)")]
    public UnityEvent InteractEvent;

    [Space(30)]
    public GameObject InteractionGuide;

    private PlayerInput_Action inputActions;
    private bool isPlayerNearby = false;

    private void Awake()
    {
        inputActions = new PlayerInput_Action();
    }
    private void Start()
    {
        InteractionGuide.SetActive(false);
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteractPerformed;
        inputActions.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            InteractionGuide.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            InteractionGuide.SetActive(false);
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (isPlayerNearby)
        {
            InteractEvent.Invoke();
        }
    }
}
