using UnityEngine;

public class BossFightMaterialController : MonoBehaviour
{
    private Material bossFightMat;
    private SpriteRenderer renderer;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
        bossFightMat = renderer.material;
    }

    private void Start() {
        bossFightMat.SetColor("_BackgroundCol", new Color(0f, 0f, 0f, 1f));  // 검은 배경
        bossFightMat.SetColor("_Color", new Color(1f, 0.3f, 0.3f, 1f));  // 불꽃 색상
        bossFightMat.SetFloat("_Smoothness", 1f);
        bossFightMat.SetFloat("_MovementRadius", 0.5f);
        bossFightMat.SetFloat("_Radius", 0.1f);
        bossFightMat.SetInt("_FlameCount", 3);
        bossFightMat.SetVector("_Center", new Vector4(0.5f, 0.5f, 0f, 0f));  // Vector4로 설정
        bossFightMat.SetFloat("_Seed", 1.44563f);  // 시드 값
    }

    private void Update() {
        bossFightMat.SetFloat("_MoveTime", Time.time);
    }
}
