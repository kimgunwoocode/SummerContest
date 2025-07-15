using UnityEngine;

public class ThornSpawnPoint : MonoBehaviour
{
    public ThornObject ThornObject;
    public Transform SpawnPoint;

    private void Awake()
    {
        if (SpawnPoint == null && transform.childCount > 0)
            SpawnPoint = transform.GetChild(0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ThornObject.SpawnPoint = SpawnPoint.position;
    }
}
