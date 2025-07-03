using UnityEngine;

public abstract class AbstractEntity : MonoBehaviour {
    [Header("Entity's life info")]
    [SerializeField] protected float HP;
    [SerializeField] protected float attackRate;

    [Header("Movement System")]
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;

    [Header("Ground Check")]
    [SerializeField] protected float groundCheckDistance = 0.1f;
    [SerializeField] protected Transform groundCheckerTransform;
    [SerializeField] protected LayerMask groundLayer;
    protected bool isGrounded;

    public abstract void Attack();
    protected abstract void Move();

}
