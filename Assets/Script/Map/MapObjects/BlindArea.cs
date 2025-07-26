using UnityEngine;

public class BlindArea : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;

    public void SetBlind(bool enabled)
    {
        SpriteRenderer.enabled = enabled;
    }
}
