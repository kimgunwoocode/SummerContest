using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VisualEffectController.Instance.BlackOut(1.2f);
            VisualEffectController.Instance.BossNameAppearance("the ultimate byoung sin", "HYO SUCK", 0.2f, 1f, 1.25f);
            VisualEffectController.Instance.BossNameFadeOut(3f);
            VisualEffectController.Instance.BlackIn(1f, 3f);
        }
    }
}