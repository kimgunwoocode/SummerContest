using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void EnterJump() {
        animator.SetTrigger("JumpStart");

       /* SetPeakValue(false);
        SetFalling(false);
        SetGrounded(false);*/
    }

    public void SetSpeed(int speed) {
        if (!(0 <= speed || speed <= 2)) {
            Debug.LogError("the \"speed\" must be between 0 and 2 (inclusive).");
            return;
        }

        animator.SetInteger("Speed", speed);
    }

    public void SetPeakValue(bool value) {
        animator.SetBool("isPeak", value);

        /*animator.SetBool("isFalling", !value);
        animator.SetBool("isGrounded", !value);*/
    }

    public void SetFalling(bool value) {
        animator.SetBool("isFalling", value);

        /*animator.SetBool("isPeak", !value);
        animator.SetBool("isGrounded", !value);*/
    }

    public void SetGrounded(bool value) {
        animator.SetBool("isGrounded", value);

        /*animator.SetBool("isPeak", !value);
        animator.SetBool("isFalling", !value);*/
    }
}
