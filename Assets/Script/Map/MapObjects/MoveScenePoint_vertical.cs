using UnityEngine;

public class MoveScenePoint_vertical : MonoBehaviour
{
    public MoveScenePoint MoveScenePoint;
    public Vector2 MoveSceneJump = new Vector2(-1f, 15f);

    private void Awake()
    {
        if (MoveScenePoint == null)
            MoveScenePoint = GetComponent<MoveScenePoint>();

        MoveScenePoint.vertical_event.AddListener(vertical_event);
    }

    void vertical_event()
    {

    }
}
