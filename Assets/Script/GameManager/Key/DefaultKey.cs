using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Input/DefaultKeyBindings")]
public class DefaultKeyBindings : ScriptableObject
{
    public string inputActionAssetPath; // InputActionAsset ���
    public List<BindingEntry> bindings = new();

    [System.Serializable]
    public class BindingEntry
    {
        public string actionName;
        public string bindingPath;
        public int bindingIndex; // ���ε� �ε��� ����
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
            Debug.LogError("InputActionAsset ��ΰ� ��� �ֽ��ϴ�.");
            return;
        }

        InputActionAsset inputActions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(inputActionAssetPath);
        if (inputActions == null)
        {
            Debug.LogError($"InputActionAsset�� ã�� �� �����ϴ�:\n{inputActionAssetPath}");
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
                        continue; // ���� �Է� ���� (�ʿ��ϸ� ���� ����)

                    bindings.Add(new BindingEntry
                    {
                        actionName = action.name,
                        bindingPath = binding.path,
                        bindingIndex = i
                    });
                }
            }
        }

        Debug.Log($"[{name}] InputActionAsset���� ���ε� {bindings.Count}�� �ҷ���.");
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
