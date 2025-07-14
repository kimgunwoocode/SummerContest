using UnityEngine;

[CreateAssetMenu(fileName = "newJumpStateData", menuName = "Data/State Data/Jump State")]
public class D_JumpState : ScriptableObject
{
   public float jumpHeight = 3f;
   public float jumpOffset = .5f;
}
