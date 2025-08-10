using UnityEngine;

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
}
