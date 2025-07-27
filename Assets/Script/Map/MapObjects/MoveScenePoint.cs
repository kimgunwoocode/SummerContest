using UnityEngine;
using UnityEngine.Events;

public class MoveScenePoint : MonoBehaviour
{
    public int PointID;
    [HideInInspector] public UnityEvent vertical_event;
    [HideInInspector] public GameObject Player;
    GameManager GameManager;

    private void Awake()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        //Debug.Log(Singleton.GameManager_Instance?.Get<GameManager>().CurrentScenePointID + "  " + PointID);
        if (Singleton.GameManager_Instance?.Get<GameManager>().CurrentScenePointID == PointID)
        {
            Player.transform.position = gameObject.transform.position;
            if (vertical_event != null)
            {
                vertical_event.Invoke();
            }
        }
    }
}
