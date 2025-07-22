using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class KeyManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private DefaultKeyBindings defaultKeyBindings;


    public Dictionary<string, string> userBindings = new();



    private void Awake()
    {
        InitializeUserBindingsFromInput();
    }
    private Dictionary<string, string> DefaultBindings => defaultKeyBindings.ToDictionary();
    private void InitializeUserBindingsFromInput()
    {
        userBindings.Clear();

        foreach (var map in inputActions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                if (action.bindings.Count > 0)
                {
                    userBindings[action.name] = action.bindings[0].effectivePath;
                }
            }
        }
    }


    // action에 할당된 키를 문자열로 반환해줌 (현재 키 설정값 출력할 때 사용하기!)
    public string GetCurrentKeyDisplay(string actionName)
    {
        if (userBindings.TryGetValue(actionName, out string path))
        {
            return InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        return "Not Set";
    }


    public void StartListeningForKey(string actionName)
    {
        InputAction action = inputActions.FindAction(actionName);
        if (action == null)
        {
            Debug.LogWarning($"[KeyManager] Action '{actionName}' not found.");
            return;
        }

        action.Disable();
        RebindingOperation operation = new();
        operation = action.PerformInteractiveRebinding()
            .OnComplete(ctx =>
            {
                string newPath = action.bindings[0].effectivePath;
                userBindings[actionName] = newPath;

                ApplyRebinding(actionName, newPath);

                Debug.Log($"[KeyManager] '{actionName}' is now bound to {newPath}");

                action.Enable();
                operation.Dispose();
            })
            .Start();
    }




    // 유저가 키를 바꿨을 때 갱신
    public void ApplyRebinding(string actionName, string newPath)
    {
        InputAction action = inputActions.FindAction(actionName);

        action.ApplyBindingOverride(0, newPath);
        userBindings[actionName] = newPath;
    }

    // 모든 키 설정을 기본값으로 되돌리기
    public void ResetToDefault()
    {
        foreach (var map in inputActions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                action.RemoveAllBindingOverrides();
                if (DefaultBindings.TryGetValue(action.name, out string path))
                {
                    action.ApplyBindingOverride(0, path);
                    userBindings[action.name] = path;
                }
            }
        }
    }
}
