using UnityEngine;

public class ChadVision : MonoBehaviour
{
    public static Transform player;
    [SerializeField] Transform _player;

    private void Awake() {
        player = _player;
    }

}
