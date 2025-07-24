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
    public UnityEvent InitEvent;// 이미 상호작용한 오브젝트 상테로 바꾸도록 하는 이벤트 (연출 X)
    public UnityEvent InteractEvent;// 상호작용시 실행될 이벤트

    [Header("Interaction 고유 정보")]
    public int ID;
    public bool isInteracted = true; //상호 가능작용 여부

    [Space(30)]
    public GameObject InteractionGuide;

    private bool isPlayerNearby = false;

    private UnityEvent _playerInteraction;
    private GameObject _player;
    private GameManager _manager;
    private GameDataManager _gameDataManager;


    private void Start()
    {
        _manager = Singleton.GameManager_Instance.Get<GameManager>();
        _gameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
        _player = _manager.Player;

        if (InteractionGuide != null)
            InteractionGuide.SetActive(false);

        SetPlayerInteraction();

        if (_gameDataManager.InteractionObjects != null && _gameDataManager.InteractionObjects.Count > 0)
            isInteracted = _gameDataManager.InteractionObjects[ID];
        
        if (!isInteracted)
            InitEvent.Invoke();
    }

    private void SetPlayerInteraction() {
        _playerInteraction = _player.GetComponent<PlayerInteraction>().interactionEvent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isInteracted)
        {
            isPlayerNearby = true;
            _playerInteraction.AddListener(OnInteractPerformed);
            if (InteractionGuide != null)
                InteractionGuide.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isInteracted)
        {
            isPlayerNearby = false;
            _playerInteraction.RemoveListener(OnInteractPerformed);
            if (InteractionGuide != null)
                InteractionGuide.SetActive(false);
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
