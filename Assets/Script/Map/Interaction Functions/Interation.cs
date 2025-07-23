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
    public UnityEvent InteractEvent;

    [Header("Interaction 고유 정보")]
    public int ID;
    public bool isInteracted = true; //상호작용 여부

    [Space(30)]
    public GameObject InteractionGuide;

    private bool isPlayerNearby = false;

    private UnityEvent _playerInteraction;
    private GameObject _player;
    private GameManager _manager;



    private void Start()
    {
        _manager = Singleton.GameManager_Instance.Get<GameManager>();
        _player = _manager.Player;

        InteractionGuide.SetActive(false);
        SetPlayerInteraction();
    }

    private void SetPlayerInteraction() {
        _playerInteraction = _player.GetComponent<PlayerInteraction>().interactionEvent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            InteractionGuide.SetActive(true);
            _playerInteraction.AddListener(OnInteractPerformed);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            InteractionGuide.SetActive(false);
            _playerInteraction.RemoveListener(OnInteractPerformed);
        }
    }

    private void OnInteractPerformed()
    {
        if (isPlayerNearby)
        {
            InteractEvent.Invoke();
        }
    }
}
