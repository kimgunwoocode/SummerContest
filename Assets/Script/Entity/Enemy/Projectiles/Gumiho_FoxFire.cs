using UnityEngine;

public class Gumiho_FoxFire : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌 시 공격 처리
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (!playerManager.IsInvincible)
            {
                Debug.Log("여우불에 공격 당함");
                playerManager.TakeDamage(1, transform.position);
                gameObject.SetActive(false);
            }
        }
    }
}