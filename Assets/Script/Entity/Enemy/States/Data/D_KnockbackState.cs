using UnityEngine;

[CreateAssetMenu(fileName = "newKnockbackStateData", menuName = "Data/State Data/Knockback State")]
public class D_KnockbackState : ScriptableObject
{
    public float knockbackTime = 0.2f;
    public float knockbackSpeed = 3f;
    public Vector2 knockbackAngle = new Vector2(2f, 4f);
}
