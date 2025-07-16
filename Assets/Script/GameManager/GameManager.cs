using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameDataManager GameDataManager;
    public GameObject Player;
    public string CurrentSceneName;

    private void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        if (GameDataManager == null)
        {
            GameDataManager = gameObject.GetComponent<GameDataManager>();
        }
    }
}
