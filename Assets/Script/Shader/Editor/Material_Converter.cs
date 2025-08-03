using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class Material_Converter : EditorWindow
{
    [MenuItem("Tools/Material Converter")]
    public static void ShowWindow() {
        GetWindow<Material_Converter>("Material_Converter");
    }

    private void OnGUI() {
        GUILayout.Label("머테리얼 일괄 변환기", EditorStyles.boldLabel);

        if (GUILayout.Button("현재 씬의 모든 머테리얼 검사 후 변환")) {
            ConvertToUnlit();
        }
    }

    private static void ConvertToUnlit() {
        int convertedCount = 0;

        // 현재 씬의 모든 SpriteRenderer 검색
        foreach (GameObject rootObj in SceneManager.GetActiveScene().GetRootGameObjects()) {
            SpriteRenderer[] renderers = rootObj.GetComponentsInChildren<SpriteRenderer>(true);

            foreach (SpriteRenderer sr in renderers) {
                if (sr.sharedMaterial != null && sr.sharedMaterial.shader != null) {
                    string shaderName = sr.sharedMaterial.shader.name;
                    //if (shaderName == "Universal Render Pipeline/2D/Sprite-Lit-Default") {
                        Shader target = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");
                        if (target == null) {
                            Debug.LogError("Target Shader not found.");
                            return;
                        }

                        Material newMat = new(target);
                        newMat.mainTexture = sr.sprite.texture;
                      
                        Undo.RecordObject(sr, "Convert to target Sprite");
                        sr.sharedMaterial = newMat;
                        EditorUtility.SetDirty(sr);

                        convertedCount++;
                    //}
                }
            }
        }

        Debug.Log($"Converted {convertedCount} SpriteRenderer(s) to material.");
    }
}