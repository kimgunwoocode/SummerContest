using UnityEngine;

public class BlindArea : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public GameObject[] obj;

    private void Awake()
    {
        if (obj == null)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                obj[i] = gameObject.transform.GetChild(i).gameObject;
            }
        }
    }

    public void SetBlind(bool enabled)
    {
        SpriteRenderer.enabled = enabled;
        foreach (GameObject obj in obj)
        {
            obj.SetActive(enabled);
        }
    }
}
