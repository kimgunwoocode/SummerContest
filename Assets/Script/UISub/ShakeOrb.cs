using Unity.VisualScripting;
using UnityEngine;

public class ShakeOrb : ShakeItem
{
    private bool _hasLanded = false;
    [SerializeField]private Rigidbody2D _rb;
    [SerializeField] private BoxCollider2D _collider;

    private new void Start() { }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_hasLanded) return;
        if ((1 << collision.gameObject.layer) == (1 << 3)) {
            _hasLanded = true;
            Destroy(_rb);
            _collider.isTrigger = true;
            Hover();
        }
    }
}