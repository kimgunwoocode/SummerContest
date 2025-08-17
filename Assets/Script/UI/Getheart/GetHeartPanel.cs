using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetHeartPanel : MonoBehaviour
{
    public EventTrigger clossTrigger;
    public Image[] Heart;

    private float duration = 2f;

    Color emptyColor = new Color(1, 1, 1, 0);
    Color fullColor = new Color(1, 1, 1, 1);

    public void GetHeart(int step)// step : 1 ~ 3
    {
        for (int i=0;i<3;i++)
        {
            if (i < step - 1)
                Heart[i].color = fullColor;
            else
                Heart[i].color = emptyColor;
        }

        StartCoroutine(GetHeartPiece(step));
    }

    IEnumerator GetHeartPiece(int step)
    {
        clossTrigger.enabled = false;

        float startX = 0f;
        float endX = 1;
        float elapsedTime = 0f;
        Color newAlpha = emptyColor;

        while (elapsedTime < 3f)
        {
            newAlpha.a = Mathf.Lerp(startX, endX, elapsedTime / duration);
            Heart[step-1].color = newAlpha;
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }
        Heart[step-1].color = fullColor;

        clossTrigger.enabled = true;
        yield break;
    }
}
