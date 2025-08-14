using UnityEngine;
using DG.Tweening;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] GameObject gumiho;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VFXSequence sequence = new VFXBuilder()
            .AppendDelay(1.7f)
            .JoinShakeCamera(1.5f, 1, Ease.OutSine)
            .AppendBlackOut(1.2f)
            .AppendBossNameAppearance(1f, "the ultimate byoung sin", "HYO SUCK", 0.2f)
            .AppendDelay(2f)
            .AppendBossNameFadeOut(0.7f)
            .AppendBlackIn(0.5f)
            .AppendCallBacks(() => { 
                gumiho.SetActive(true);
                SoundManager.instance.PlayBossBGM("Gumiho");
            })
            .Build();
        }
    }
}