using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class KeyManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    [SerializeField] private DefaultKeyBindings defaultKeyBindings;
    [SerializeField] private Dictionary<string, bool> defaultToggle;
    [SerializeField] private float defaultMouse;

    // 설정 변수
    public Dictionary<string, string> userBindings = new(); //키 설정
    public Dictionary<string, bool> userToggles = new(); //토글 설정
    public float userMouse;//마우스 감도 설정


    #region Init Bindings
    private void Awake()
    {
        InitializeUserBindingsFromInput();
    }
    private Dictionary<string, string> DefaultBindings => defaultKeyBindings.ToDictionary();
    public void InitializeUserBindingsFromInput()
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
    #endregion


    // action에 할당된 키를 문자열로 반환해줌 (현재 키 설정값 출력할 때 사용하기!)
    public string GetCurrentKeyDisplay(string actionName)
    {
        if (userBindings.TryGetValue(actionName, out string path))
        {
            return InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        return "Not Set";
    }

    #region Key Change
    public void StartListeningForKey(string actionName)
    {
        InputAction action = inputActions.FindAction(actionName);
        if (action == null)
        {
            Debug.LogWarning($"[KeyManager] Action '{actionName}' not found.");
            return;
        }

        action.Disable();

        int bindingIndex = GetKeyboardBindingIndex(action);
        if (bindingIndex == -1)
        {
            Debug.LogError($"[KeyManager] No keyboard binding found for '{actionName}'");
            return;
        }

        RebindingOperation operation = new();
        operation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(ctx =>
            {
                string newPath = action.bindings[bindingIndex].effectivePath;
                userBindings[actionName] = newPath;

                ApplyRebinding(actionName, newPath, bindingIndex);

                Debug.Log($"[KeyManager] '{actionName}' is now bound to {newPath}");

                action.Enable();
                operation.Dispose();
            })
            .Start();
    }
    private int GetKeyboardBindingIndex(InputAction action) // 현재 디바이스에 맞는 바인딩 인덱스에 접근
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (!binding.isComposite && !binding.isPartOfComposite && binding.path.Contains("<Keyboard>"))
            {
                return i;
            }
        }

        return -1;
    }



    // 유저가 키를 바꿨을 때 갱신
    public void ApplyRebinding(string actionName, string newPath, int bindingIndex)
    {
        InputAction action = inputActions.FindAction(actionName);
        action.ApplyBindingOverride(bindingIndex, newPath);
        userBindings[actionName] = newPath;
    }
    #endregion

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