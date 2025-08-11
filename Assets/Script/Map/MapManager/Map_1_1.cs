using UnityEngine;

public class Map_1_1 : MonoBehaviour
{
    public CameraManager CameraManager;
    [Space]
    public GameObject Intro_obj;
    public GameObject Egg_obj;
    public GameObject Player_obj;

    private void Awake()
    {
        // 게임을 처음 시작했을 때
        if (Singleton.GameManager_Instance.Get<GameDataManager>().SpawnPoint == -1)
        {
            Intro_obj.SetActive(false);
            Egg_obj.SetActive(false);
            Player_obj.SetActive(false);
            SetIntro();
        }
    }

    public void MoveBound(int index)
    {
        CameraManager.SetStageIndex(index);
    }

    public void SetIntro()
    {
        Egg_obj.SetActive(true);
        Player_obj.SetActive(true);
    }
}
