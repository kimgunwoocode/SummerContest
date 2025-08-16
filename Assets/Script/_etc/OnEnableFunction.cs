using UnityEngine;
using UnityEngine.Events;

public class OnEnableFunction : MonoBehaviour
{
    public UnityEvent function;

    private void OnEnable()
    {
        function.Invoke();
    }
}
