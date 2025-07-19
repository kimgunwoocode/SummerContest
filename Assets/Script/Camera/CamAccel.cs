using UnityEngine;

public class CamAccel : MonoBehaviour
{
    [SerializeField] private DollyCamera _cam;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            _cam.CamAccel();
        }
    }
}
