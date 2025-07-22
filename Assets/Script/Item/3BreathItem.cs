using UnityEngine;

[CreateAssetMenu(menuName = "Item/BreathItem")]
public class BreathItemData : ItemData, IBreathItem
{
    public float breathCost;
    public GameObject BreathPrefab;
    public void UseBreath(GameObject BreathPrefab, Vector2 ShootingDirection)
    {

    }
}

public interface IBreathItem
{
    void UseBreath(GameObject BreathPrefab, Vector2 ShootingDirection);
}
