using UnityEngine;

public abstract class AbstractEntity : MonoBehaviour {
    [SerializeField] protected float HP;
    [SerializeField] protected float attackRate;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float groundCheckDistance = 0.1f;
    [SerializeField] protected Transform groundCheckerTransform;
    [SerializeField] protected LayerMask groundLayer;
    protected bool isGrounded;

    public abstract void Attack();
    protected abstract void Move();

}
