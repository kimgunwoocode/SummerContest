using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;     

    public void flip(bool value) {
        sprite.flipX = value;
    }


    public void EnterJump() {
        animator.SetTrigger("JumpStart");
    }

    public void EnterDoubleJump() {
        animator.SetTrigger("doubleJump");
    }
    
    public void SetDash(bool value) {
        animator.SetBool("Dash", value);
    }

    public void SetStun(bool value) {
        animator.SetBool("isStun", value);
    }

    public void SetSpeed(int speed) {
        if (!(0 <= speed || speed <= 2)) {
            Debug.LogError("the \"speed\" must be between 0 and 2 (inclusive).");
            return;
        }

        animator.SetInteger("Speed", speed);
    }



    public void SetGrounded(bool value) {
        animator.SetBool("isGrounded", value);
    }

    public void SetGlide(bool value) {
        animator.SetBool("isGlide", value);
    }

    public void SetClimb(bool value) {
        animator.SetBool("isClimb", value);
    }
}
