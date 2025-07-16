using UnityEngine;

public class Camera_for_test : MonoBehaviour
{
    GameManager gameManager;
    private void Awake()
    {
        gameManager = Singleton.GameManager_Instance.Get<GameManager>();
    }
    void Update()
    {
        
    }
}
