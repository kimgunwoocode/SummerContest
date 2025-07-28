using UnityEngine;

public class Map_1_2 : MonoBehaviour
{
    public CameraManager CameraManager;

    private void Awake()
    {

    }

    public void MoveBound(int index)
    {
        CameraManager.SetStageIndex(index);
    }

    public void SetZoom(float size)
    {
        CameraManager.SetZoom(size, 5f);
    }
}
