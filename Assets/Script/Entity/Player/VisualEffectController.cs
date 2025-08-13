using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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

    private enum VFX_Type {
        BossNameAppearance,
        BossNameFadeOut,
        BlackOut,
        BlackIn,
        BlackOutWithReversedAlpha,
        BlackInWithReversedAlpha,
        WhiteOut,
        WhiteIn,
        WhiteOutWithReversedAlpha,
        WhiteInWithReversedAlpha,
        ClearScreen
    }

    private class VFX_Info {
        public VFX_Info(VFX_Type _type, string _title = null, string _name = null, float _nameAppearanceDelay = 0.1f, float _duration = 1.0f, float _startDelay = 0) {
            type = _type;
            title = _title;
            name = _name;
            nameAppearanceDelay = _nameAppearanceDelay;
            duration = _duration;
            startDelay = _startDelay;

        }
        public VFX_Type type;
        public string title;
        public string name;
        public float nameAppearanceDelay;
        public float duration;
        public float startDelay;

    }

    private Queue<VFX_Info> RequestQueue = new Queue<VFX_Info>();
    private bool isRunningVFX = false;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        blackOutRenderer = blackOut.GetComponent<Image>();
        blackOutRenderer.material.SetFloat("_Radius", 0f);
        title.text = null;
        bossName.text = null;
    }
    
    private void ExcuteRequest() {
        if (isRunningVFX) {
            return;
        } else {
            VFX_Info current = RequestQueue.Dequeue();

            switch (current.type) {
                case VFX_Type.BossNameAppearance :
                    StartCoroutine(IBossNameAppearance(current.title, current.name, current.nameAppearanceDelay, current.duration, current.startDelay));
                    break;

                case VFX_Type.BossNameFadeOut:
                    StartCoroutine(IBossNameFadeOut(current.duration, current.startDelay));
                    break;

                case VFX_Type.BlackOut:
                    StartCoroutine(IBlackOut(current.duration, current.startDelay));
                    break;

                case VFX_Type.BlackIn:
                    StartCoroutine(IBlackIn(current.duration, current.startDelay));
                    break;

                case VFX_Type.BlackOutWithReversedAlpha:
                    StartCoroutine(IBlackOutWithReversedAlpha(current.duration, current.startDelay));
                    break;

                case VFX_Type.BlackInWithReversedAlpha:
                    StartCoroutine(IBlackInWithReversedAlpha(current.duration, current.startDelay));
                    break;

                case VFX_Type.WhiteOut:
                    StartCoroutine(IWhiteOut(current.duration, current.startDelay));
                    break;

                case VFX_Type.WhiteIn:
                    StartCoroutine(IWhiteIn(current.duration, current.startDelay));
                    break;

                case VFX_Type.WhiteOutWithReversedAlpha:
                    StartCoroutine(IWhiteOutWithReversedAlpha(current.duration, current.startDelay));
                    break;

                case VFX_Type.WhiteInWithReversedAlpha:
                    StartCoroutine(IWhiteInWithReversedAlpha(current.duration, current.startDelay));
                    break;

                case VFX_Type.ClearScreen:
                    StartCoroutine(IClearScreen(current.startDelay));
                    break;
            }

        }
    }

    public void BossNameAppearance(string title, string name, float nameAppearanceDelay = 0.1f, float duration = 1.0f, float startDelay = 0) {
        VFX_Info info = new VFX_Info(VFX_Type.BossNameAppearance, _title: title, _name: name, _nameAppearanceDelay: nameAppearanceDelay, _duration: duration, _startDelay: startDelay);
        RequestQueue.Enqueue(info);
        ExcuteRequest();
    }

    private IEnumerator IBossNameAppearance(string _title ,string _name, float nameAppearanceDelay ,float duration, float startDelay) {
        isRunningVFX = true;
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
        isRunningVFX = false;

        if (RequestQueue.Count != 0) ExcuteRequest();
    }

    public void BossNameFadeOut(float duration = 0.2f, float startDelay = 0) {
        VFX_Info info = new VFX_Info(VFX_Type.BossNameFadeOut, _duration: duration, _startDelay: startDelay);
        RequestQueue.Enqueue(info);
        ExcuteRequest();
    }

    private IEnumerator IBossNameFadeOut(float duration ,float startDelay) {
        isRunningVFX = true;
        yield return new WaitForSeconds(startDelay);
        float startX = 1f;
        float endX = 0;
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            float newAlpha = Mathf.Lerp(startX, endX, elapsedTime/duration);
            title.alpha = newAlpha;
            bossName.alpha = newAlpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        title.alpha = 0f;
        bossName.alpha = 0f;
        isRunningVFX = false;
        if (RequestQueue.Count != 0) ExcuteRequest();
    }

    public void BlackOut(float duration, float startDelay = 0) {
        VFX_Info info = new VFX_Info(VFX_Type.BlackOut, _duration: duration, _startDelay: startDelay);
        RequestQueue.Enqueue(info);
        ExcuteRequest();
    }

    private IEnumerator IBlackOut(float duration, float startDelay = 0) {
        isRunningVFX = true;
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 0;
        float endX = 1.5f;
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
        isRunningVFX = false;
        if (RequestQueue.Count != 0) ExcuteRequest();
    }

    public void BlackIn(float duration, float startDelay = 0) {
        VFX_Info info = new VFX_Info(VFX_Type.BlackIn, _duration: duration, _startDelay: startDelay);
        RequestQueue.Enqueue(info);
        ExcuteRequest();
    }

    private IEnumerator IBlackIn(float duration, float startDelay) {
        isRunningVFX = true;
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
        isRunningVFX = false;
        if (RequestQueue.Count != 0) ExcuteRequest();
    }

    public void BlackOutWithReversedAlpha(float duration, float startDelay = 0) {
        VFX_Info info = new VFX_Info(VFX_Type.BlackOutWithReversedAlpha, _duration: duration, _startDelay: startDelay);
        RequestQueue.Enqueue(info);
        ExcuteRequest();
    }

    private IEnumerator IBlackOutWithReversedAlpha(float duration, float startDelay) {
        isRunningVFX = true;
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 0;
        float endX = 1.5f;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 1);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 0);
        blackOutRenderer.material.SetFloat("_Radius", startX);
        while (elapsedTime < duration) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / duration);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
        isRunningVFX = false;
        if (RequestQueue.Count != 0) ExcuteRequest();
    }

    public void BlackInWithReversedAlpha(float duration, float startDelay = 0) {
        VFX_Info info = new VFX_Info(VFX_Type.BlackInWithReversedAlpha, _duration: duration, _startDelay: startDelay);
        RequestQueue.Enqueue(info);
        ExcuteRequest();
    }

    private IEnumerator IBlackInWithReversedAlpha(float duration, float startDelay) {
        isRunningVFX = true;
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 1.5f;
        float endX = 0;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 1);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 0);
        blackOutRenderer.material.SetFloat("_Radius", startX);
        while (elapsedTime < duration) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / duration);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
        isRunningVFX = false;
        if (RequestQueue.Count != 0) ExcuteRequest();
    }

    public void WhiteOut(float duration, float startDelay = 0) {
        VFX_Info info = new VFX_Info(VFX_Type.WhiteOut, _duration: duration, _startDelay: startDelay);
        RequestQueue.Enqueue(info);
        ExcuteRequest();
    }

    private IEnumerator IWhiteOut(float duration, float startDelay = 0) {
        isRunningVFX = true;
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 0;
        float endX = 1.5f;
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
        isRunningVFX = false;
        if (RequestQueue.Count != 0) ExcuteRequest();
    }

    public void WhiteIn(float duration, float startDelay = 0) {
        VFX_Info info = new VFX_Info(VFX_Type.WhiteIn, _duration: duration, _startDelay: startDelay);
        RequestQueue.Enqueue(info);
        ExcuteRequest();
    }

    private IEnumerator IWhiteIn(float duration, float startDelay) {
        isRunningVFX = true;
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
        isRunningVFX = false;
        if (RequestQueue.Count != 0) ExcuteRequest();
    }

    public void WhiteOutWithReversedAlpha(float duration, float startDelay = 0) {
        VFX_Info info = new VFX_Info(VFX_Type.WhiteOutWithReversedAlpha, _duration: duration, _startDelay: startDelay);
        RequestQueue.Enqueue(info);
        ExcuteRequest();
    }

    private IEnumerator IWhiteOutWithReversedAlpha(float duration, float startDelay) {
        isRunningVFX = true;
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 0;
        float endX = 1.5f;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 1);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 1);
        blackOutRenderer.material.SetFloat("_Radius", startX);
        while (elapsedTime < duration) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / duration);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
        isRunningVFX = false;
        if (RequestQueue.Count != 0) ExcuteRequest();
    }

    public void WhiteInWithReversedAlpha(float duration, float startDelay = 0) {
        VFX_Info info = new VFX_Info(VFX_Type.WhiteInWithReversedAlpha, _duration: duration, _startDelay: startDelay);
        RequestQueue.Enqueue(info);
        ExcuteRequest();
    }

    private IEnumerator IWhiteInWithReversedAlpha(float duration, float startDelay) {
        isRunningVFX = true;
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;
        float startX = 1.5f;
        float endX = 0;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 1);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 1);
        blackOutRenderer.material.SetFloat("_Radius", startX);
        while (elapsedTime < duration) {
            float newRadius = Mathf.Lerp(startX, endX, elapsedTime / duration);
            blackOutRenderer.material.SetFloat("_Radius", newRadius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackOutRenderer.material.SetFloat("_Radius", endX);
        isRunningVFX = false;
        if (RequestQueue.Count != 0) ExcuteRequest();
    }

    public void ClearScreen(float startDelay = 0) {
        VFX_Info info = new VFX_Info(VFX_Type.ClearScreen, _startDelay: startDelay);
        RequestQueue.Enqueue(info);
        ExcuteRequest();
    }

    private IEnumerator IClearScreen(float startDelay = 0) {
        isRunningVFX = true;
        yield return new WaitForSeconds(startDelay);
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 0);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 0);
        blackOutRenderer.material.SetFloat("_Radius", 0);
        title.text = null;
        bossName.text = null;
        title.alpha = 0;
        bossName.alpha = 0;
        isRunningVFX = false;
        if (RequestQueue.Count != 0) ExcuteRequest();
    }
}
