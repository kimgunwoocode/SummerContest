/*using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Tooltip("0에 가까울수록 멀리 있는 배경 (느리게 움직임)")]
    [Range(0f, 1f)]
    public float parallaxFactor;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private float minX;
    private float maxX;

    [SerializeField]GameObject worldStartMarker;
    [SerializeField] GameObject worldEndMarker;

    void Start() {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        float spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        if (worldStartMarker == null || worldEndMarker == null) {
            this.enabled = false;
            return;
        }

        float worldStartX = worldStartMarker.transform.position.x;
        float worldEndX = worldEndMarker.transform.position.x;

        minX = worldStartX + (spriteWidth / 2f);
        maxX = worldEndX - (spriteWidth / 2f);
    }

    void LateUpdate() {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        Vector3 newPosition = transform.position + new Vector3(deltaMovement.x * parallaxFactor, 0, 0); 

        if (minX < maxX) {
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        }

        transform.position = newPosition;

        lastCameraPosition = cameraTransform.position;
    }
}*/

using UnityEngine;

public class ParallaxBackground : MonoBehaviour {
    public float parallaxEffectMultiplier;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    // 추가된 변수들
    private float cameraHeight;
    private float cameraWidth;

    void Start() {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        // 카메라의 높이와 너비를 계산합니다.
        cameraHeight = Camera.main.orthographicSize * 2;
        cameraWidth = cameraHeight * Camera.main.aspect;
    }

    void LateUpdate() {
        // 현재 오브젝트의 위치와 카메라의 위치를 기준으로,
        // 오브젝트가 카메라 시야 범위 내에 있는지 확인합니다.
        float distanceToCameraX = Mathf.Abs(transform.position.x - cameraTransform.position.x);
        float distanceToCameraY = Mathf.Abs(transform.position.y - cameraTransform.position.y);

        // 카메라 시야 범위 밖에 있으면 함수를 종료합니다.
        // 여기서는 카메라 너비/높이의 절반을 기준으로 여유를 줍니다.
        if (distanceToCameraX > cameraWidth / 2f + 5f || distanceToCameraY > cameraHeight / 2f + 5f) {
            lastCameraPosition = cameraTransform.position;
            return;
        }

        Vector3 cameraMovementDelta = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(cameraMovementDelta.x * parallaxEffectMultiplier, cameraMovementDelta.y * parallaxEffectMultiplier, 0);

        lastCameraPosition = cameraTransform.position;
    }
}
