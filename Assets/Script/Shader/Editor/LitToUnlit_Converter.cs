using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class LitToUnlit_Converter : EditorWindow
{
    [MenuItem("Tools/Convert SpriteRenderer to Unlit")]
    public static void ShowWindow() {
        GetWindow<LitToUnlit_Converter>("Lit to Unlit Converter");
    }

    private void OnGUI() {
        GUILayout.Label("Lit → Unlit 머테리얼 일괄 변환기", EditorStyles.boldLabel);

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
                    Debug.Log(sr.gameObject.name + ", " + sr.material.name);
                    if (shaderName == "Universal Render Pipeline/2D/Sprite-Lit-Default") {
                        // Unlit용 기본 쉐이더
                        Shader unlitShader = Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default");
                        if (unlitShader == null) {
                            Debug.LogError("Unlit Shader not found.");
                            return;
                        }

                        Material newMat = new Material(unlitShader);
                        newMat.mainTexture = sr.sprite?.texture;
                      
                        Undo.RecordObject(sr, "Convert to Unlit Sprite");
                        sr.sharedMaterial = newMat;
                        EditorUtility.SetDirty(sr);

                        convertedCount++;
                    }
                }
            }
        }

        Debug.Log($"Converted {convertedCount} SpriteRenderer(s) to Unlit.");
    }
}