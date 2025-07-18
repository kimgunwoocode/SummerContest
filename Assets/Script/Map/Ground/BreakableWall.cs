using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private DollyCamera dollyCamera; // �ν����Ϳ��� �����ϰų� �ڵ� ã��
    [SerializeField] private int targetStageIndex = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾ �����ϰ� �ʹٸ� �±׳� �̸� ���� �߰� ����
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
