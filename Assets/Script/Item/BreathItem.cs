using UnityEngine;

[CreateAssetMenu(menuName = "Item/BreathItem")]
public class BreathItemData : ItemData, IBreathItem
{
    [Space]
    public GameObject BreathPrefab;
    [Header("Breath")]
    public int breathDamage;//브레스 피해량 개수 (플레이어 ATK*breathDamage = 최종 피해량)
    public int breathAttackCount;// 브레스가 공격할 수 있는 적 개수
    public float breathCoolDown;// 브레스 사용시 다음 브레스 사용까지 걸리는 시간
    public float breathCost;// 브레스 사용시 소모되는 브레스게이지
    public float breathRange;// 브레스가 날아갈 수 있는 최대 사거리
    public float breathSpeed;// 브레스의 이동 속도
    public bool breathMapPassable;// 맵 통과 가능 여부 (벽, 땅 플랫폼 등...)
    public bool breathWaterPassable;// 물에서 사용 가능 여부


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
