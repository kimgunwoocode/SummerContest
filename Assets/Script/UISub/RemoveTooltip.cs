using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class RemoveTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _selfUGUI;
    private void Awake()
    {
        _selfUGUI.text = " ";
    }
    public void RmAfter(float seconds)
    {
        StartCoroutine(discard(seconds));
    }

    IEnumerator discard(float seconds) {
        _selfUGUI.text = "Esc키로 도감 확인";
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
