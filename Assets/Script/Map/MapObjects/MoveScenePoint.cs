using UnityEngine;

public class MoveScenePoint : MonoBehaviour
{
    public int PointID;

    GameObject Player;

    private void Awake()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        if (Singleton.GameManager_Instance?.Get<GameManager>().CurrentScenePointID == PointID)
        {
            Player.transform.position = gameObject.transform.position;
        }
    }
}
