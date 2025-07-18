using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private DollyCamera dollyCamera; // 인스펙터에서 연결하거나 자동 찾기
    [SerializeField] private int targetStageIndex = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어만 감지하고 싶다면 태그나 이름 조건 추가 가능
        if (other.CompareTag("Player"))
        {
            if (dollyCamera != null)
            {
                dollyCamera.SetStageIndex(targetStageIndex);
                // Debug.Log($"Stage index changed to {targetStageIndex}");
            }
        }
    }
}
