using UnityEngine;
using UnityEngine.Events;

public class Function : MonoBehaviour
{
    public UnityEvent function;
    public UnityEvent function2;

    public void InvokeFunction()
    {
        function.Invoke();
    }

    public void InvokeFunction2()
    {
        function2.Invoke();
    }
}
