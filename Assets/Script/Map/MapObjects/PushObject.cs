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

    void FixedUpdate()
    {
        if (!isPlayerTouching)
        {
            rb.linearVelocity = Vector2.zero;
        }
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
