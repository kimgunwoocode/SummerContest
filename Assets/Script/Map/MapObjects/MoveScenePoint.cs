using UnityEngine;

public class MoveScenePoint : MonoBehaviour
{
    public int PointID;

    GameManager manager;

    private void Start()
    {
        manager = Singleton.GameManager_Instance.Get<GameManager>();
        if (manager.CurrentScenePointID == PointID)
        {
            manager.Player.transform.position = gameObject.transform.position;
        }
    }
}
