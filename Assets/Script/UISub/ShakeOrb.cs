using Unity.VisualScripting;
using UnityEngine;

public class ShakeOrb : ShakeItem
{
    private bool _hasLanded = false;
    [SerializeField]private Rigidbody2D _rb;

    private new void Start() { }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & 1 << 3) != 0) {
            if (_hasLanded) return;
            _rb.simulated = false;
            Hover();
        }
    }
}