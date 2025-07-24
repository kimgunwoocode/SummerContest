using UnityEngine;

public class ShotBreath : BreathObject
{
    [Header("źȯ ����")]
    public GameObject bulletPrefab; // NomalBreath ������
    public float spreadAngle = 30f; // ��ü ���� ����
    public int bulletCount = 4;
    public float shotSpeed = 5f;

    public void Fire(Vector2 direction)
    {
        float halfSpread = spreadAngle * 0.5f;

        for (int i = 0; i < bulletCount; i++)
        {
            // ���� ���� ���
            float angleOffset = Mathf.Lerp(-halfSpread, halfSpread, bulletCount == 1 ? 0.5f : (float)i / (bulletCount - 1));
            float angleInRad = angleOffset * Mathf.Deg2Rad;

            // ȸ���� ���� ���
            Vector2 shotDir = Quaternion.Euler(0, 0, angleOffset) * direction.normalized;

            // źȯ ����
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            NomalBreath breath = bullet.GetComponent<NomalBreath>();

            if (breath != null)
            {
                breath.ShootingDirection = shotDir;
                breath.speed = shotSpeed;
            }
        }

        // �߻� �� ShotBreath ��ü�� ����
        Destroy(gameObject);
    }
}
