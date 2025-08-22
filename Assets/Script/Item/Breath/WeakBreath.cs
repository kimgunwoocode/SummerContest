using System.Collections.Generic;
using UnityEngine;

public class WeakBreath : BreathObject
{
    public float speed;

    [Space]
    public Rigidbody2D rb;

    List<EnemyEntity> enemys = new();

    private void Awake() {
        if (rb == null) {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + shootingDirection * BreathItemData_SO.breathSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            EnemyEntity enemy = collision.GetComponent<EnemyEntity>();
            if (!enemys.Contains(enemy)) {
                enemy?.TakeDamage(BreathItemData_SO.breathDamage, transform.position);
                enemys.Add(enemy);
            }
        } else if (((1 << collision.gameObject.layer) & hitLayers) != 0) {
            Destroy(gameObject);
        }
    }
}
