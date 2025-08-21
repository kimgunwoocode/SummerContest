using UnityEngine;
using UnityEngine.U2D;

public class ParallaxBackground : MonoBehaviour {
    [Tooltip("0에 가까울수록 멀리 있는 배경 (느리게 움직임)")]
    [Range(-1f, 1f)]
    public float parallaxEffectMultiplier;
    public bool isActive = true;
    private Transform cameraTransform;
    private Vector3 initialPosition;

    void Start() {
        cameraTransform = Camera.main.transform;
        initialPosition = transform.position;
    }

    void LateUpdate() {
        if (!isActive) return;
        // 카메라의 X좌표를 기준으로 배경의 새로운 X좌표를 계산한다.
        // 이게 핵심이다.
        float newX = initialPosition.x + (cameraTransform.position.x * parallaxEffectMultiplier);

        // Z축은 원래 위치 그대로 유지한다.
        transform.position = new Vector3(newX, transform.position.y, initialPosition.z);
    }
}

/*using UnityEngine;

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
*/