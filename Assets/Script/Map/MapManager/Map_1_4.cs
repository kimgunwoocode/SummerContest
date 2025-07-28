using UnityEngine;

public class Map_1_4 : MonoBehaviour
{
    public CameraManager CameraManager;
    GameDataManager GameDataManager;


    private void Awake()
    {
        GameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
    }

    public void MoveBound(int index)
    {
        CameraManager.SetStageIndex(index);
    }

    public void SetZoom(float size)
    {
        CameraManager.SetZoom(size, 4f);
    }
}
