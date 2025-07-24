using UnityEngine;

[CreateAssetMenu(menuName = "Item/BreathItem")]
public class BreathItemData : ItemData, IBreathItem
{
    [Header("Breath")]
    public float breathCost;
    public GameObject BreathPrefab;

    private BreathObject BreathObject;

    public void UseBreath(Vector2 ShootingDirection, Vector3 position = default(Vector3)) // 총알 소환 위치 (입), 발사 방향 (마우스)
    {
        GameObject Bullet = Instantiate(BreathPrefab);
        Bullet.transform.position = position;
        BreathObject = Bullet.GetComponent<BreathObject>();
        BreathObject.ShootingDirection = ShootingDirection.normalized;
    }

    private void OnValidate()
    {

    }
}

public interface IBreathItem
{
    void UseBreath(Vector2 ShootingDirection, Vector3 position);
}
