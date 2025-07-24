using UnityEngine;

public abstract class BreathObject : MonoBehaviour
{
    protected Vector2 shootingDirection;  // 내부 사용 전용

    public Vector2 ShootingDirection      // 외부에서 방향 설정용
    {
        private get => shootingDirection;
        set => shootingDirection = value.normalized;
    }
}