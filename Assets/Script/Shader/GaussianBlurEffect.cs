using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GaussianBlurSprite : MonoBehaviour {
    public Shader blurShader;
    public float blurRadius = 3.0f;

    private SpriteRenderer spriteRenderer;
    private Material blurMaterial;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (blurShader == null) return;

        blurMaterial = new Material(blurShader);

        // 원본 스프라이트의 rect, pivot, originalTexture 정보를 가져옵니다.
        Rect originalRect = spriteRenderer.sprite.rect;
        Vector2 originalPivot = spriteRenderer.sprite.pivot;
        Texture2D originalTex = spriteRenderer.sprite.texture;

        // 임시 RenderTexture를 생성합니다. (전체 텍스처 크기로 생성)
        RenderTexture tempRT1 = RenderTexture.GetTemporary(originalTex.width, originalTex.height);
        RenderTexture tempRT2 = RenderTexture.GetTemporary(originalTex.width, originalTex.height);

        // 원본 텍스처를 첫 번째 RenderTexture로 복사합니다.
        Graphics.Blit(originalTex, tempRT1);

        // 블러 강도를 설정합니다.
        blurMaterial.SetFloat("_BlurRadius", blurRadius);

        // 가로 블러 Pass
        Graphics.Blit(tempRT1, tempRT2, blurMaterial, 0);

        // 세로 블러 Pass
        Graphics.Blit(tempRT2, tempRT1, blurMaterial, 1);

        // 블러가 적용된 결과(tempRT1)를 SpriteRenderer에 적용합니다.
        // 이때, 원본 스프라이트의 rect와 pivot 정보를 사용합니다.
        Sprite newSprite = Sprite.Create(
            texture: GetTexture2D(tempRT1),
            rect: originalRect,  // <--- 수정된 부분: 원본 rect 사용
            pivot: originalPivot // <--- 수정된 부분: 원본 pivot 사용
        );
        spriteRenderer.sprite = newSprite;

        // 임시 RenderTexture 해제
        RenderTexture.ReleaseTemporary(tempRT1);
        RenderTexture.ReleaseTemporary(tempRT2);
    }

    // RenderTexture의 내용을 Texture2D로 변환하는 함수
    private Texture2D GetTexture2D(RenderTexture rt) {
        RenderTexture.active = rt;
        Texture2D texture2D = new Texture2D(rt.width, rt.height);
        texture2D.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;
        return texture2D;
    }
}