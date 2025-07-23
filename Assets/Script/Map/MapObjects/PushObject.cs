using UnityEngine;

public class PushObject : MonoBehaviour
{
    public int ID;
    public Vector2 position;

    private Rigidbody2D rb;
    private bool isPlayerTouching = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        gameObject.transform.position = Singleton.GameManager_Instance.Get<GameDataManager>().PushObjects[ID];
    }

    void FixedUpdate()
    {
        if (!isPlayerTouching)
        {
            rb.linearVelocity = Vector2.zero;
        }
        position = gameObject.transform.position;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal.normalized;

                if (Mathf.Abs(normal.x) > 0.5f)
                {
                    isPlayerTouching = true;
                    return;
                }
            }

            isPlayerTouching = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerTouching = false;
        }
    }
}
