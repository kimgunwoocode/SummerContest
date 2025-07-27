using UnityEngine;

public class BurnWall : MonoBehaviour
{
    LayerMask mask;

    private void Awake()
    {
        mask = LayerMask.NameToLayer("Breath");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == mask)
        {
            Destroy(gameObject);
        }
    }

    public void init()
    {
        Destroy(gameObject);
    }
}
