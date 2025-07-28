using UnityEngine;

public class PushObject : MonoBehaviour
{
    public int ID;

    GameDataManager gameDataManager;
    private Rigidbody2D rb;
    private bool isPlayerTouching = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        gameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
        if (gameDataManager.PushObjects != null && gameDataManager.PushObjects.ContainsKey(ID))
            gameObject.transform.position = gameDataManager.PushObjects[ID];
        else
        {
            Debug.LogError("PushObject가 등록되지 않음 (ID : " + ID + " )");
        }
    }

    void FixedUpdate()
    {
        if (!isPlayerTouching)
        {
            rb.linearVelocity = Vector2.zero;
        }
        gameDataManager.PushObjects[ID] = gameObject.transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPlayerTouching)
        {
            isPlayerTouching = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerTouching = false;
        }
    }
}
