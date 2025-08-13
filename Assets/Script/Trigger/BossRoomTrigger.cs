using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VisualEffectController.Instance.BlackOut(1.2f);
            VisualEffectController.Instance.BossNameAppearance("the ultimate byoung sin", "HYO SUCK", 0.2f, 1f);
            VisualEffectController.Instance.BossNameFadeOut(startDelay : 0.7f);
            VisualEffectController.Instance.BlackIn(0.5f);
        }
    }
}