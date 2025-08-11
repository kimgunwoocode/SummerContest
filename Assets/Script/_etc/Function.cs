using UnityEngine;
using UnityEngine.Events;

public class Function : MonoBehaviour
{
    public UnityEvent function;

    public void InvokeFunction()
    {
        function.Invoke();
    }
}
