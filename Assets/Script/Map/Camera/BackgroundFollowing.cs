using UnityEngine;

public class BackgroundFollowing : MonoBehaviour
{
    [SerializeField] private GameObject[] backgrounds;
    private GameObject currentBG;

    private void Awake() {
        currentBG = backgrounds[0];
    }

}
