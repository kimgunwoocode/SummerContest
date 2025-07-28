using UnityEngine;

public class ShotBreath : BreathObject
{
    [Header("탄환 설정")]
    public GameObject bulletPrefab; // NomalBreath 프리팹
    public float spreadAngle = 30f; // 전체 퍼짐 각도
    public int bulletCount = 4;
    public float shotSpeed = 5f;

    public void Fire(Vector2 direction)
    {
        float halfSpread = spreadAngle * 0.5f;

        for (int i = 0; i < bulletCount; i++)
        {
            // 퍼짐 각도 계산
            float angleOffset = Mathf.Lerp(-halfSpread, halfSpread, bulletCount == 1 ? 0.5f : (float)i / (bulletCount - 1));
            float angleInRad = angleOffset * Mathf.Deg2Rad;

            // 회전된 방향 계산
            Vector2 shotDir = Quaternion.Euler(0, 0, angleOffset) * direction.normalized;

            // 탄환 생성
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            NomalBreath breath = bullet.GetComponent<NomalBreath>();

            if (breath != null)
            {
                breath.ShootingDirection = shotDir;
                breath.speed = shotSpeed;
            }
        }

        // 발사 후 ShotBreath 본체는 제거
        Destroy(gameObject);
    }
}
