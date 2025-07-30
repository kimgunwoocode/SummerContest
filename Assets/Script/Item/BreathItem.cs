using UnityEngine;

[CreateAssetMenu(menuName = "Item/BreathItem")]
public class BreathItemData : ItemData, IBreathItem
{
    [Header("Breath")]
    public float breathCost;
    public GameObject BreathPrefab;

    //이잉,,,이거 여기에서 필요해잉,,, 에러가 뜬다면 암쏘쏘리 죄송죄송 고멘네데스용
    public float breathCoolDown;

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
