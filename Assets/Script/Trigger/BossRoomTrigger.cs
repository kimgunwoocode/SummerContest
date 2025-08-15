using UnityEngine;
using DG.Tweening;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] GameObject gumiho;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            VFXSequence sequence = new VFXBuilder()
            .AppendDelay(1.7f)
            .AppendBlackOut(1.2f)
            .AppendBossNameAppearance(0.7f, "\"진실의 방으로.\"", "구미호", 0.40f)
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