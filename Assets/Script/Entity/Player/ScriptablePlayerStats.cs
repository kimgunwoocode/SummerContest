using UnityEngine;

[CreateAssetMenu]
public class ScriptablePlayerStats : ScriptableObject {
    [Header("Layer")]
    public LayerMask PlayerLayer;

    [Header("Movement")]
    public float WalkSpeed = 250f;
    public float MaxSpeed = 800f;

    [Header("Jump")]
    public float JumpForce = 10f;
    public float GravityByNormalForce = -1f;
    public float MidAirGravity = -9.81f;
    public float MaxFallingSpeed = -40f;
    public float GravityModifierWhenJumpEndedEarly = 3f;
    public float GravityModifierWhenFalling = 2.5f;
    public float CoyoteTime = 0.14f;
    public float JumpBufferTime = 0.24f;

    public float maxJumpTime;

    [Header("Collision Check")]
    public float wallCheckDistance = 0.6f;
    //public GameObject[] wallRaycastPoints;

    public float groundCheckDistance = 0.1f;
    //public GameObject groundCheckerTransform;
    public LayerMask groundLayer;


}
