using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] protected float duration;

    protected void kill() {
        Destroy(gameObject);
    }
}
