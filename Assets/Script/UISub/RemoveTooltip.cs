using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RemoveTooltip : MonoBehaviour
{
    [SerializeField] private float _seconds;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void RmAfter(float seconds)
    {
        StartCoroutine(discard(seconds));
    }

    IEnumerator discard(float seconds) {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
