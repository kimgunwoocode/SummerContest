using UnityEngine;

public class WellVisible : MonoBehaviour
{
    [SerializeField] SpriteRenderer cover;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        cover.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cover.enabled = false;
    }
}
