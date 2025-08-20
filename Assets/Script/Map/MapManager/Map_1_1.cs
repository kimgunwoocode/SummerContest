using System.Collections;
using UnityEngine;

public class Map_1_1 : MonoBehaviour
{
    public CameraManager CameraManager;
    [Space]
    public GameObject Intro_obj;
    public GameObject Egg_obj;
    public GameObject Player_obj;

    private Vector2 PlayerInBoxPosition = new Vector2(-22.5f, 0.5f);
    private Vector2 PlayerInitPosition = new Vector2(-5.177f, -0.4f);

    GameDataManager GameDataManager;

    private void Awake()
    {
        GameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
        // 게임을 처음 시작했을 때
        if (GameDataManager.SpawnPoint == -1)
        {
            //Debug.Log("게임 첫 시작 애니메이션");

            GameDataManager.SpawnPoint = 0;
            
            MoveBound(5);
            Intro_obj.SetActive(true);
            Egg_obj.SetActive(false);
            Player_obj.transform.position = PlayerInBoxPosition;
            StartCoroutine(SetIntro_middle());
        }
        else
        {
            Intro_obj.SetActive(false);
            Egg_obj.SetActive(true);
        }
    }

    public void MoveBound(int index)
    {
        CameraManager.SetStageIndex(index);
    }
    public void SetZoom(float size)
    {
        CameraManager.SetZoom(size, 3f);
    }

    public void SetIntro()
    {
        //Debug.Log("게임 시작");

        MoveBound(0);
        SetZoom(4.4f);

        Intro_obj.SetActive(false);
        Egg_obj.SetActive(true);
        Player_obj.SetActive(true);
        Player_obj.transform.position = PlayerInitPosition;
    }

    IEnumerator SetIntro_middle()
    {
        yield return null;
        MoveBound(5);
        CameraManager._cam.orthographicSize = 2;
        CameraManager._targetZoom = 2;

        Player_obj.transform.position = PlayerInitPosition;
        Player_obj.SetActive(false);
        yield break;
    }
}
