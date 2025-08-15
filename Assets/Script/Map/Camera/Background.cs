using UnityEngine;

public class Background : MonoBehaviour
{
    void Start() {
        Camera mainCamera = Camera.main;

        if (mainCamera == null || !mainCamera.orthographic) {
            Debug.LogError("메인 카메라가 Orthographic 모드가 아닙니다. 2D 게임에서는 Orthographic 모드를 사용해야 합니다.");
            return;
        }

        float cameraHeight = mainCamera.orthographicSize * 2;

        float cameraWidth = cameraHeight * mainCamera.aspect;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null) {
            float spriteWidth = spriteRenderer.sprite.bounds.size.x;
            float spriteHeight = spriteRenderer.sprite.bounds.size.y;

            float scaleX = cameraWidth / spriteWidth;
            float scaleY = cameraHeight / spriteHeight;

            float scale = Mathf.Max(scaleX, scaleY);
            transform.localScale = new Vector3(scale * 1f, scale * 1f, 1);
        } else {
            Debug.LogError("SpriteRenderer didn't found");
        }
    }
}
