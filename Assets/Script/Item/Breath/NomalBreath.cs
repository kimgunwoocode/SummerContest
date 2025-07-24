using UnityEngine;

public class NomalBreath : BreathObject
{
    public float speed;


    [Space]
    public Rigidbody2D rb;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + shootingDirection * speed * Time.fixedDeltaTime);
    }
}
