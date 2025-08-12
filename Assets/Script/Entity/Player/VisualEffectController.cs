using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class VisualEffectController : MonoBehaviour
{
    [SerializeField] GameObject blackOut;
    private Image blackOutRenderer;
    [Header("Text")]
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text bossName;

    private static VisualEffectController instance;
    public static VisualEffectController Instance {
        get {
            if (instance == null) instance = new VisualEffectController();
            return instance;
        }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        blackOutRenderer = blackOut.GetComponent<Image>();
        blackOutRenderer.material.SetFloat("_Radius", 0f);
        title.text = null;
        bossName.text = null;
    }
    

    public void BossNameAppearance(string title, string name, float nameAppearanceDelay = 0.1f, float duration = 1.0f, float startDelay = 0) {
        StartCoroutine(IBossNameAppearance(title, name, nameAppearanceDelay, duration, startDelay));
    }

    private IEnumerator IBossNameAppearance(string _title ,string _name, float nameAppearanceDelay ,float duration, float startDelay) {
        yield return new WaitForSeconds(startDelay);
        string curretentText = null;
        int index = 0;
        title.text = null;
        title.alpha = 1f;

        float textAppearanceTime = duration / _title.Length;
        while (curretentText != _title) {
            curretentText += _title[index];
            title.text = curretentText;
            index++;
            yield return new WaitForSeconds(textAppearanceTime);
        }
        yield return new WaitForSeconds(nameAppearanceDelay);
        bossName.text = _name;
    }

    public void BossNameFadeOut(float startDelay = 0) {
        StartCoroutine(IBossNameFadeOut(startDelay));
    }

    private IEnumerator IBossNameFadeOut(float startDelay) {
        yield return new WaitForSeconds(startDelay);
        title.text = null;
        bossName.text = null;
    }

    public void BlackOut(float time, float startDelay = 0) {
        StartCoroutine(IBlackOut(time, startDelay));
    }

    private IEnumerator IBlackOut(float time, float startDelay = 0) {
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 0;
        float endX = 1.5f;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 0);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 0);
        blackOutRenderer.material.SetFloat("_Radius", startX);
        while (elapsedTime < time) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / time);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
    }

    public void BlackIn(float duration, float startDelay = 0) {
        StartCoroutine(IBlackIn(duration, startDelay));
    }

    private IEnumerator IBlackIn(float duration, float startDelay) {
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 1.5f;
        float endX = 0;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 0);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 0);
        blackOutRenderer.material.SetFloat("_Radius", startX);

        while (elapsedTime < duration) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / duration);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
    }

    public void BlackOutWithReversedAlpha(float time, float startDelay = 0) {
        StartCoroutine(IBlackOutWithReversedAlpha(time, startDelay));
    }

    private IEnumerator IBlackOutWithReversedAlpha(float time, float startDelay) {
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 0;
        float endX = 1.5f;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 1);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 0);
        blackOutRenderer.material.SetFloat("_Radius", startX);
        while (elapsedTime < time) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / time);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
    }

    public void BlackInWithReversedAlpha(float time, float startDelay = 0) {
        StartCoroutine(IBlackInWithReversedAlpha(time, startDelay));
    }

    private IEnumerator IBlackInWithReversedAlpha(float time, float startDelay) {
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 1.5f;
        float endX = 0;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 1);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 0);
        blackOutRenderer.material.SetFloat("_Radius", startX);
        while (elapsedTime < time) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / time);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
    }

    public void WhiteOut(float time, float startDelay = 0) {
        StartCoroutine(IWhiteOut(time, startDelay));
    }

    private IEnumerator IWhiteOut(float time, float startDelay = 0) {
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 0;
        float endX = 1.5f;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 0);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 1);
        blackOutRenderer.material.SetFloat("_Radius", startX);
        while (elapsedTime < time) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / time);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
    }

    public void WhiteIn(float duration, float startDelay = 0) {
        StartCoroutine(IWhiteIn(duration, startDelay));
    }

    private IEnumerator IWhiteIn(float duration, float startDelay) {
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 1.5f;
        float endX = 0;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 0);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 1);
        blackOutRenderer.material.SetFloat("_Radius", startX);

        while (elapsedTime < duration) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / duration);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
    }

    public void WhiteOutWithReversedAlpha(float time, float startDelay = 0) {
        StartCoroutine(IWhiteOutWithReversedAlpha(time, startDelay));
    }

    private IEnumerator IWhiteOutWithReversedAlpha(float time, float startDelay) {
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 0;
        float endX = 1.5f;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 1);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 1);
        blackOutRenderer.material.SetFloat("_Radius", startX);
        while (elapsedTime < time) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / time);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
    }

    public void WhiteInWithReversedAlpha(float time, float startDelay = 0) {
        StartCoroutine(IWhiteInWithReversedAlpha(time, startDelay));
    }

    private IEnumerator IWhiteInWithReversedAlpha(float time, float startDelay) {
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 1.5f;
        float endX = 0;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 1);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 1);
        blackOutRenderer.material.SetFloat("_Radius", startX);
        while (elapsedTime < time) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / time);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
    }

    public void ClearScreen(float startDelay = 0) {
        StartCoroutine(IClearScreen(startDelay));
    }

    public void ControllDisplayDirectly() {

    }

    private IEnumerator IControllDisplayDirectly(Color color,bool reversedAlpha, bool reversedColor ,float startDelay) {
        yield return new WaitForSeconds(startDelay);
    }

    private IEnumerator IClearScreen(float startDelay = 0) {
        yield return new WaitForSeconds(startDelay);
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 0);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 0);
        blackOutRenderer.material.SetFloat("_Radius", 0);
        title.text = null;
        bossName.text = null;
        title.alpha = 0;
        bossName.alpha = 0;
    }
}
