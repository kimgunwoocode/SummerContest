using UnityEngine;
using System.Collections;

public class MeleeEffect : Effect {
    private void Start() {
        StartCoroutine(counter());
    }

    private IEnumerator counter() {
        yield return new WaitForSeconds(duration);
        kill();
    }
}
