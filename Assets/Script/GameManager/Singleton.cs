using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton GameManager_Instance { get; private set; }
    public GameDataManager GameDataManager;
    public MapDataManager MapDataManager;

    private void Awake()
    {
        if (GameManager_Instance == null)
        {
            GameManager_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (GameManager_Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
