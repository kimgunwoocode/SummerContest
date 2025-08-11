using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class VisualEffectController : MonoBehaviour
{
    [SerializeField] GameObject blackOut;
    private Image blackOutRenderer;
    [SerializeField] TMP_Text text;
    [SerializeField] float textAppearanceTime;

    private void Awake() {
        blackOutRenderer = blackOut.GetComponent<Image>();
        blackOutRenderer.material.SetFloat("_Radius", 0f);
    }
    

    public void BlackOut(float time) {
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 0);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 0);
        StartCoroutine(IBlackOut(time));
    }

    private IEnumerator IBlackOut(float time) {
        float elapsedTime = 0f;
        float startX = 0;
        float endX = 1.5f;
        while (elapsedTime < time) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / time);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void BossNameAppearance(string name) {
        StartCoroutine(IBossNameAppearance(name));
    }

    private IEnumerator IBossNameAppearance(string name) {
        string curretentText = null;
        int index = 0;
        while(curretentText != name) {
            curretentText += name[index];
            text.text = curretentText;
            index++;
            yield return new WaitForSeconds(textAppearanceTime);
        }
    }
}
