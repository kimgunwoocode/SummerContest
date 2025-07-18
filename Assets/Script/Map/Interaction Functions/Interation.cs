using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;

public class Interaction : MonoBehaviour
{
    [Header("상호작용 기능 설명")]
    [TextArea]
    public string description;

    [Header("상호작용시 실행할 이벤트 (메서드)")]
    public List<UnityEvent> InteractEvent;

    [Header("Interaction 고유 정보")]
    public int ID;
    public int isInteracted = 1; // 상호작용 여부,
    // 그냥 bool로 하지 않은 이유는 한 번 상호작용한 뒤에 다른 종류의 상호작용이 가능하도록, 또는 다른 이벤트 발생 이후 상호작용 내용이 바뀔 수 있도록 하기 위함
    // -1 이면 상호작용 자체가 안됨. 0부터 양의 정수는 interactEvent의 인덱스 순서를 기준으로 상호작용시 실행할 이벤트 선택


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
        if (other.CompareTag("Player") && isInteracted != -1)
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
            InteractEvent[isInteracted].Invoke();
        }
    }
}
