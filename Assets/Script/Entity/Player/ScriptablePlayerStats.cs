using UnityEngine;

[CreateAssetMenu]
public class ScriptablePlayerStats : ScriptableObject {
    [Header("Layer")]
    public LayerMask PlayerLayer;

    [Header("Movement")]
    public float CrounchSpeed = 250f;
    public float WalkSpeed = 300f;
    public float MaxSpeed = 800f;
    public bool isCrounchActionByToggle = true;
    public bool isGlideActionByToggle = false;

    [Header("Jump")]
    [Tooltip("the basic jump force")]public float JumpForce = 10f;
    [Tooltip("the gravity that is apllied when player is on the ground")]public float GravityByNormalForce = -1f;
    [Tooltip("the basic gravity")]public float MidAirGravity = -9.81f;
    [Tooltip("the max y-axis speed")]public float MaxFallingSpeed = -40f;
    [Tooltip("the apex range, if player's y-axis speed is included in the apex range, it judged as a apex")]public float ApexThreadHold = 0.1f;
    [Tooltip("allowed jump time while the player is in the air when doesn't jump")]public float CoyoteTime = 0.14f;
    [Tooltip("allowed jump time while the player is in the air when does jump")]public float JumpBufferTime = 0.24f;
    [Tooltip("allowed ")] public int bonusJump = 1;

    [Header("Gravity")]
    [Tooltip("the gravity multipler that is apllied after when player release the button")] public float GravityModifierWhenJumpEndedEarly = 3f;
    [Tooltip("the gravity multipler that is apllied after when player is falling down")] public float GravityModifierWhenFalling = 2.5f;
    [Tooltip("the gravity multipler that is applied when player reached about on the apex")] public float ApexModifier = 0.7f;
    public float GlideGravity = 0.5f;

    [Header("Dash")]
    public float DashCooldown;
    public float DashTime;
    public float DashSpeed;

    [Header("Available Abilities")]
    public bool IsDashUnlocked = false;
    public bool IsDoubleJumpUnloceked = false;
    public bool IsGlideUnlocked = false;

    [Header("Collision Check")]
    public float wallCheckDistance = 0.6f;
    public float groundCheckDistance = 0.1f;
    public float ceilingCheckDistance = 0.1f;
    
    public LayerMask groundLayer;
}
