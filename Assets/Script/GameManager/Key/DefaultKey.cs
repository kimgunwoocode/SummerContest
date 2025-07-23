using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Input/DefaultKeyBindings")]
public class DefaultKeyBindings : ScriptableObject
{
    public string inputActionAssetPath; // InputActionAsset 경로
    public List<BindingEntry> bindings = new();

    [System.Serializable]
    public class BindingEntry
    {
        public string actionName;
        public string bindingPath;
        public int bindingIndex; // 바인딩 인덱스 포함
    }
    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> dict = new();
        foreach (var entry in bindings)
        {
            dict[entry.actionName] = entry.bindingPath;
        }
        return dict;
    }

    public Dictionary<string, List<BindingEntry>> ToDictionaryByAction()
    {
        Dictionary<string, List<BindingEntry>> dict = new();

        foreach (var entry in bindings)
        {
            if (!dict.ContainsKey(entry.actionName))
                dict[entry.actionName] = new List<BindingEntry>();

            dict[entry.actionName].Add(entry);
        }

        return dict;
    }

#if UNITY_EDITOR
    public void LoadFromInputActions()
    {
        if (string.IsNullOrEmpty(inputActionAssetPath))
        {
            Debug.LogError("InputActionAsset 경로가 비어 있습니다.");
            return;
        }

        InputActionAsset inputActions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(inputActionAssetPath);
        if (inputActions == null)
        {
            Debug.LogError($"InputActionAsset을 찾을 수 없습니다:\n{inputActionAssetPath}");
            return;
        }

        bindings.Clear();

        foreach (var map in inputActions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];
                    if (binding.isComposite || binding.isPartOfComposite)
                        continue; // 복합 입력 제외 (필요하면 포함 가능)

                    bindings.Add(new BindingEntry
                    {
                        actionName = action.name,
                        bindingPath = binding.path,
                        bindingIndex = i
                    });
                }
            }
        }

        Debug.Log($"[{name}] InputActionAsset에서 바인딩 {bindings.Count}개 불러옴.");
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
