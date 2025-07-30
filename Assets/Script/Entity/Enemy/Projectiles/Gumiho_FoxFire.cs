using UnityEngine;

public class Gumiho_FoxFire : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 플레이어와 충돌 시 공격 처리
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("여우불 공격");
            other.GetComponent<PlayerManager>().TakeDamage(1, transform.position);
            Destroy(gameObject);
        }
    }
}