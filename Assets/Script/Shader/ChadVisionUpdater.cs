using UnityEngine;

public class ChadVisionUpdater : MonoBehaviour
{
    private Material fadeMaterial;

    private void Awake() {
        fadeMaterial = GetComponent<SpriteRenderer>().material;
    }

    void Update() {
        fadeMaterial.SetVector("_PlayerPos", ChadVision.player.position);
    }
}
