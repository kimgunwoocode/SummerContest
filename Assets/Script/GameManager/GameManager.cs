using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public string CurrentSceneName;

    private void Awake()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
