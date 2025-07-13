using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;

    private void Awake()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
