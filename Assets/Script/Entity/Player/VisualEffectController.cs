using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using DG.Tweening;

public enum VFX_Type {
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
    ShakeCamera,
    ClearScreen,
    Delay,
    Callback
}

public class VFX_Info {
    public VFX_Info(VFX_Type _type, string _title = null, string _name = null, float _nameAppearanceDelay = 0.1f, float _duration = 1.0f, float _startDelay = 0, float _vibrato = 10f, Ease _ease = Ease.Linear, Action _callback = null) {
        type = _type;
        title = _title;
        name = _name;
        nameAppearanceDelay = _nameAppearanceDelay;
        duration = _duration;
        startDelay = _startDelay;
        vibrato = _vibrato;
        ease = _ease;
        callback = _callback;
    }
    public VFX_Type type;
    public string title;
    public string name;
    public float nameAppearanceDelay;
    public float duration;
    public float startDelay;
    public float vibrato;
    public Ease ease;
    public Action callback;
}

public class VFXBuilder {
    List<List<VFX_Info>> infos = new List<List<VFX_Info>>();

    public VFXBuilder AppendDelay(float _delay) {
        VFX_Info delay = new VFX_Info(VFX_Type.Delay, _startDelay: _delay);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(delay);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder JoinDelay(float _delay) {
        VFX_Info info = new VFX_Info(VFX_Type.Delay, _startDelay: _delay);
        infos[infos.Count - 1].Add(info);
        return this;
    }

    public VFXBuilder AppendBossNameAppearance(float duration, string title, string name, float nameAppearanceDelay) {
        VFX_Info bossNameAppearance = new VFX_Info(VFX_Type.BossNameAppearance, title, name, nameAppearanceDelay, duration);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(bossNameAppearance);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder JoinBossNameAppearance(float duration, string title, string name, float nameAppearanceDelay) {
        VFX_Info bossNameAppearance = new VFX_Info(VFX_Type.BossNameAppearance, title, name, nameAppearanceDelay, duration);
        infos[infos.Count - 1].Add(bossNameAppearance);
        return this;
    }

    public VFXBuilder AppendBossNameFadeOut(float duration) {
        VFX_Info bossNameFadeOut = new VFX_Info(VFX_Type.BossNameFadeOut, _duration: duration);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(bossNameFadeOut);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder JoinBossNameFadeOut(float duration) {
        VFX_Info info = new VFX_Info(VFX_Type.BossNameFadeOut, _duration : duration);
        infos[infos.Count - 1].Add(info);
        return this;
    }

    public VFXBuilder AppendBlackOut(float duration) {
        VFX_Info blackOut = new VFX_Info(VFX_Type.BlackOut, _duration: duration);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(blackOut);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder JoinBlackOut(float duration) {
        VFX_Info info = new VFX_Info(VFX_Type.BlackOut, _duration: duration);
        infos[infos.Count - 1].Add(info);
        return this;
    }

    public VFXBuilder AppendBlackIn(float duration) {
        VFX_Info blackIn = new VFX_Info(VFX_Type.BlackIn, _duration: duration);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(blackIn);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder JoinBlackIn(float duration) {
        VFX_Info info = new VFX_Info(VFX_Type.BlackIn, _duration: duration);
        infos[infos.Count - 1].Add(info);
        return this;
    }

    public VFXBuilder AppendBlackOutWithReversedAlpha(float duration) {
        VFX_Info blackOutWithReversedAlpha = new VFX_Info(VFX_Type.BlackOutWithReversedAlpha, _duration: duration);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(blackOutWithReversedAlpha);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder JoinBlackOutWithReversedAlpha(float duration) {
        VFX_Info info = new VFX_Info(VFX_Type.BlackOutWithReversedAlpha, _duration: duration);
        infos[infos.Count - 1].Add(info);
        return this;
    }

    public VFXBuilder AppendBlackInWithReversedAlpha(float duration) {
        VFX_Info blackInWithReversedAlpha = new VFX_Info(VFX_Type.BlackInWithReversedAlpha, _duration: duration);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(blackInWithReversedAlpha);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder JoinBlackInWithReversedAlpha(float duration) {
        VFX_Info info = new VFX_Info(VFX_Type.BlackInWithReversedAlpha, _duration: duration);
        infos[infos.Count - 1].Add(info);
        return this;
    }

    public VFXBuilder AppendWhiteOut(float duration) {
        VFX_Info whiteOut = new VFX_Info(VFX_Type.WhiteOut, _duration: duration);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(whiteOut);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder JoinWhiteOut(float duration) {
        VFX_Info info = new VFX_Info(VFX_Type.WhiteOut, _duration: duration);
        infos[infos.Count - 1].Add(info);
        return this;
    }

    public VFXBuilder AppendWhiteIn(float duration) {
        VFX_Info whiteIn = new VFX_Info(VFX_Type.WhiteIn, _duration: duration);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(whiteIn);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder JoinWhiteIn(float duration) {
        VFX_Info info = new VFX_Info(VFX_Type.WhiteIn, _duration: duration);
        infos[infos.Count - 1].Add(info);
        return this;
    }

    public VFXBuilder AppendWhiteOutWithReversedAlpha(float duration) {
        VFX_Info whiteOutWithReversedAlpha = new VFX_Info(VFX_Type.WhiteOutWithReversedAlpha, _duration: duration);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(whiteOutWithReversedAlpha);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder JoinWhiteOutWithReversedAlpha(float duration) {
        VFX_Info info = new VFX_Info(VFX_Type.WhiteOutWithReversedAlpha, _duration: duration);
        infos[infos.Count - 1].Add(info);
        return this;
    }

    public VFXBuilder AppendWhiteInWithReversedAlpha(float duration) {
        VFX_Info whiteInWithReversedAlpha = new VFX_Info(VFX_Type.WhiteInWithReversedAlpha, _duration: duration);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(whiteInWithReversedAlpha);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder JoinWhiteInWithReversedAlpha(float duration) {
        VFX_Info info = new VFX_Info(VFX_Type.WhiteInWithReversedAlpha, _duration: duration);
        infos[infos.Count - 1].Add(info);
        return this;
    }

    public VFXBuilder AppendClearScreen() {
        VFX_Info clearScreen = new VFX_Info(VFX_Type.ClearScreen);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(clearScreen);
        infos.Add(sequence);
        return this;
    }

    public VFXBuilder AppendShakeCamera(float duration, float vibrato) {
        VFX_Info info = new VFX_Info(VFX_Type.ShakeCamera, _duration : duration, _vibrato : vibrato);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(info);
        infos.Add(sequence);
        return this;
    }
    public VFXBuilder JoinShakeCamera(float duration, float vibrato, Ease ease = Ease.Linear) {
        VFX_Info info = new VFX_Info(VFX_Type.ShakeCamera, _duration: duration, _vibrato: vibrato, _ease : ease);
        infos[infos.Count - 1].Add(info);
        return this;
    }

    public VFXBuilder AppendCallBacks(Action callback) {
        VFX_Info info = new VFX_Info(VFX_Type.Callback, _callback : callback);
        List<VFX_Info> sequence = new List<VFX_Info>();
        sequence.Add(info);
        infos.Add(sequence);
        return this;
    }

    public VFXSequence Build() {
        return new VFXSequence(infos);
    }
}

public class VFXSequence {
    private List<List<VFX_Info>> Sequences;
    Coroutine SequenceCoroutine;
    bool isExcuting;
    public VFXSequence(List<List<VFX_Info>> Sequences) {
        this.Sequences = Sequences;
        isExcuting = true;
        SequenceCoroutine = VisualEffectController.Instance.StartCoroutine(ExcuteSequence());
    }

    public bool SequenceCompleted() {
        return isExcuting;
    }

    private IEnumerator ExcuteSequence() {
        int i = 0;
        while (i < Sequences.Count) {
            List<VFX_Info> currentSequence = Sequences[i];
            List<Coroutine> runningCoroutines = new List<Coroutine>();

            foreach (VFX_Info info in currentSequence) {
                Coroutine newCoroutine;
                switch (info.type) {
                    case VFX_Type.BossNameAppearance:
                        newCoroutine = VisualEffectController.Instance.BossNameAppearance(info);
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.BossNameFadeOut:
                        newCoroutine = VisualEffectController.Instance.BossNameFadeOut(info);
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.BlackOut:
                        newCoroutine = VisualEffectController.Instance.BlackOut(info);
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.BlackIn:
                        newCoroutine = VisualEffectController.Instance.BlackIn(info);
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.BlackOutWithReversedAlpha:
                        newCoroutine = VisualEffectController.Instance.BlackOutWithReversedAlpha(info);
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.BlackInWithReversedAlpha:
                        newCoroutine = VisualEffectController.Instance.BlackInWithReversedAlpha(info);
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.WhiteOut:
                        newCoroutine = VisualEffectController.Instance.WhiteOut(info);
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.WhiteIn:
                        newCoroutine = VisualEffectController.Instance.WhiteIn(info);
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.WhiteOutWithReversedAlpha:
                        newCoroutine = VisualEffectController.Instance.WhiteOutWithReversedAlpha(info);
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.WhiteInWithReversedAlpha:
                        newCoroutine = VisualEffectController.Instance.WhiteInWithReversedAlpha(info);
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.ShakeCamera:
                        newCoroutine = VisualEffectController.Instance.ShakeCamera(info);
                        break;
                    case VFX_Type.ClearScreen:
                        newCoroutine = VisualEffectController.Instance.ClearScreen();
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.Delay:
                        newCoroutine = VisualEffectController.Instance.Delay(info);
                        runningCoroutines.Add(newCoroutine);
                        break;
                    case VFX_Type.Callback:
                        info.callback.Invoke();
                        break;
                }
            }

            foreach (Coroutine coroutine in runningCoroutines) {
                yield return coroutine;
            }
            i++;
        }
        isExcuting = false;
    }
}

public class VisualEffectController : MonoBehaviour
{
    [SerializeField] GameObject blackOut;
    private Image blackOutRenderer;
    private Camera mainCam;
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
        mainCam = Camera.main;
    }

    private bool isRunningVFX = false;

    public Coroutine BossNameAppearance(VFX_Info info) {
        return StartCoroutine(IBossNameAppearance(info.title, info.name, info.nameAppearanceDelay, info.duration));
    }
    private IEnumerator IBossNameAppearance(string _title ,string _name, float nameAppearanceDelay ,float duration) {
        isRunningVFX = true;
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
    }

    public Coroutine BossNameFadeOut(VFX_Info info) {
        return StartCoroutine(IBossNameFadeOut(info.duration));
    }
    private IEnumerator IBossNameFadeOut(float duration) {
        isRunningVFX = true;
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
    }

    public Coroutine BlackOut(VFX_Info info) {
        return StartCoroutine(IBlackOut(info.duration));
    }
    private IEnumerator IBlackOut(float duration) {
        isRunningVFX = true;
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
    }

    public Coroutine BlackIn(VFX_Info info) {
        return StartCoroutine(IBlackIn(info.duration));
    }
    private IEnumerator IBlackIn(float duration) {
        isRunningVFX = true;
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
    }

    public Coroutine BlackOutWithReversedAlpha(VFX_Info info) {
        return StartCoroutine(IBlackOutWithReversedAlpha(info.duration));
    }
    private IEnumerator IBlackOutWithReversedAlpha(float duration) {
        isRunningVFX = true;
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
    }

    public Coroutine BlackInWithReversedAlpha(VFX_Info info) {
        return StartCoroutine(IBlackInWithReversedAlpha(info.duration));
    }
    private IEnumerator IBlackInWithReversedAlpha(float duration) {
        isRunningVFX = true;
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
    }

    public Coroutine WhiteOut(VFX_Info info) {
        return StartCoroutine(IWhiteOut(info.duration));
    }
    private IEnumerator IWhiteOut(float duration) {
        isRunningVFX = true;
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
    }

    public Coroutine WhiteIn(VFX_Info info) {
        return StartCoroutine(IWhiteIn(info.duration));
    }
    private IEnumerator IWhiteIn(float duration) {
        isRunningVFX = true;
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
    }

    public Coroutine WhiteOutWithReversedAlpha(VFX_Info info) {
        return StartCoroutine(IWhiteOutWithReversedAlpha(info.duration));
    }
    private IEnumerator IWhiteOutWithReversedAlpha(float duration) {
        isRunningVFX = true;
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
    }

    public Coroutine WhiteInWithReversedAlpha(VFX_Info info) {
        return StartCoroutine(IWhiteInWithReversedAlpha(info.duration));
    }
    private IEnumerator IWhiteInWithReversedAlpha(float duration) {
        isRunningVFX = true;
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
    }

    public Coroutine ClearScreen() {
        return StartCoroutine(IClearScreen());
    }
    private IEnumerator IClearScreen() {
        isRunningVFX = true;
        blackOutRenderer.material.SetFloat("_IsReversedAlpha", 0);
        blackOutRenderer.material.SetFloat("_IsReversedColor", 0);
        blackOutRenderer.material.SetFloat("_Radius", 0);
        title.text = null;
        bossName.text = null;
        title.alpha = 0;
        bossName.alpha = 0;
        isRunningVFX = false;
        yield return null;
    }

    public Coroutine Delay(VFX_Info info) {
        return StartCoroutine(IDelay(info.duration));
    }
    private IEnumerator IDelay(float duration) {
        yield return new WaitForSeconds(duration);
    }

    public Coroutine ShakeCamera(VFX_Info info) {
        return StartCoroutine(IShakeCamera(info.duration, info.vibrato, info.ease));
    }

    private IEnumerator IShakeCamera(float duration, float vibrato, Ease ease = Ease.Linear) {
        mainCam.transform.DOShakePosition(duration, vibrato).SetEase(ease);

        yield return new WaitForSeconds(duration);
        
    }
}
