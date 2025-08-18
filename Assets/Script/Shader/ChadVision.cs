using UnityEngine;

public class ChadVision : MonoBehaviour
{
    public Transform player;
    private Material fadeMaterial;

    private void Awake() {
        fadeMaterial = GetComponent<SpriteRenderer>().material;
    }

    void Update() {
        fadeMaterial.SetVector("_PlayerPos", player.position);
        Debug.Log(fadeMaterial.GetVector("_PlayerPos"));
    }
}
