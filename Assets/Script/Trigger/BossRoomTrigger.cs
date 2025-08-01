using UnityEngine;


/**
 *      Name                   : BossRoomTrigger
 *      Last Update         : 2025-08-01
 *      Description          : 보스전에 관한 특별한 이벤트 관리
 *      Todo                    : 보스 사망 이벤트가 필요하다
 */

public class BossRoomTrigger : MonoBehaviour
{
    [Tooltip("보스 오브젝트를 Inspector에 할당")]
    [SerializeField] private EnemyEntity boss;

    private bool hasTriggered = false;
    
    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered)
        {
            Debug.Log("OnTriggerEnter: Already hasTriggered is true");
            return;
        }
            
        Debug.Log("OnTriggerEnter: Enter the boss room");
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (boss != null)
            {
                string bossName = boss.gameObject.name;

                SoundManager.instance.StopCurrentBGM();
                SoundManager.instance.PlayBossBGM(bossName);
            }
            else
            {
                Debug.LogWarning("BossTriggerZone: No Boss Allocated");
            }
        }
    }
}