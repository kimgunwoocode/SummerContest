using UnityEngine;

public class PushObject : MonoBehaviour
{
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
            // ������ ���� ����
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // �÷��̾ ������ ��Ҵ��� Ȯ��
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal.normalized;

                // normal�� ��(0,1) �Ǵ� �Ʒ�(0,-1)�� �ƴϸ� ������ ���� ��
                if (Mathf.Abs(normal.x) > 0.5f)
                {
                    isPlayerTouching = true;
                    return;
                }
            }

            // ��/�Ʒ������� ���� ���
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
