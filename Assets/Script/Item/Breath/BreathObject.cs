using UnityEngine;

public abstract class BreathObject : MonoBehaviour
{
    public BreathItemData BreathItemData_SO;
    public BreathLayerOption BreathLayerOption_SO;
    protected Vector2 shootingDirection;  // 내부 사용 전용

    [SerializeField] protected LayerMask hitLayers;

    public Vector2 ShootingDirection      // 외부에서 방향 설정용
    {
        private get => shootingDirection;
        set => shootingDirection = value.normalized;
    }

    private void OnValidate()
    {
        if (BreathItemData_SO != null)
        {
            if(BreathItemData_SO.breathMapPassable)
            {
                hitLayers = 0;
            }
            else
            {
                hitLayers = BreathLayerOption_SO.MapLayer;
            }
        }
    }
}