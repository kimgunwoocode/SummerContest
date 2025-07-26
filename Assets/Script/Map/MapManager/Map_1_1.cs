using UnityEngine;

public class Map_1_1 : MonoBehaviour
{
    public CameraManager CameraManager;

    private void Awake()
    {

    }

    public void MoveBound(int index)
    {
        CameraManager.SetStageIndex(index);
    }
}
