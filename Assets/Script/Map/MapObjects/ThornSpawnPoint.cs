using UnityEngine;

public class ThornSpawnPoint : MonoBehaviour
{
    public ThornObject ThornObject;
    public Transform SpawnPoint;

    private void Awake()
    {
        if (SpawnPoint == null && transform.childCount > 0)
            SpawnPoint = transform.GetChild(0);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.enabled)
            spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ThornObject != null && SpawnPoint != null)
            ThornObject.SpawnPoint = SpawnPoint.position;
    }

    // 선택된 경우에만 Gizmo 표시
    private void OnDrawGizmosSelected()
    {
        if (SpawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(SpawnPoint.position, 0.2f);
            Gizmos.DrawLine(transform.position, SpawnPoint.position);
        }

        if (ThornObject != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(ThornObject.transform.position, 0.25f);
            Gizmos.DrawLine(transform.position, ThornObject.transform.position);
        }
    }
}
