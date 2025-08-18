using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class Lightning : MonoBehaviour
{
    [SerializeField] AnimationCurve myCurve;
    [SerializeField] float curveDuration = 5f;
    [SerializeField] Vector2 nextLightning_MinMax = new Vector2(4f, 6f);
    private float currentTime = 0f;

    [SerializeField] Light2D[] lightnings;

    private void Start() {
        StartCoroutine(RapidLightning());
        //Thunder();
    }

    private IEnumerator RapidLightning() {
        while (true) {
            float nextLightning = Random.Range(nextLightning_MinMax.x, nextLightning_MinMax.y);
            Thunder();
            yield return new WaitForSeconds(nextLightning);
        }
    }

    public void Thunder() {
        int index = Random.Range(0, lightnings.Length);
        Light2D current = lightnings[index];

        StartCoroutine(LightChange());
        StartCoroutine(ThunderSFX());
    }

    private IEnumerator ThunderSFX() {
        yield return new WaitForSeconds(Random.Range(0.7f, 1.5f));
        int ran = Random.Range(0, 3);
        if (ran == 0) SoundManager.instance.PlaySFX("map_Thunder1");
        else if (ran == 1) SoundManager.instance.PlaySFX("map_Thunder2");
        else if (ran == 2) SoundManager.instance.PlaySFX("map_Thunder3");
    }

    private IEnumerator LightChange() {
        currentTime = 0;
        float curveValue = 0f;
        while (currentTime < curveDuration) {
            currentTime += Time.deltaTime;
            curveValue = myCurve.Evaluate(currentTime / curveDuration);
            lightnings[0].intensity = curveValue * 10f;
            yield return null;
        }
    } 
}
