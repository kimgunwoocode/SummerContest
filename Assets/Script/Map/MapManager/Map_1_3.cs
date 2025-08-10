using UnityEngine;

public class Map_1_3 : MonoBehaviour
{
    public CameraManager CameraManager;
    GameDataManager GameDataManager;
    /*
    [Header("Boss Item ID")]
    public int BossItemID;
    [Header("Map Objects")]
    public GameObject BossWall;
    */

    private void Awake()
    {
        GameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
    }

    /*
    public void SetBossWall(bool active)
    {
        if (active && GameDataManager.GettedItems[BossItemID] == 0) //벽 생성 (보스 클리어시 벽 생성 안하게 하기 위함)
        {
            BossWall.SetActive(true);
        }
        else if (!active)
        {
            // 벽 제거 + 보스 클리어시 호출할거기 때문에, 이펙트 있으면 좋음
            // 보스 클리어시 얻는 아이템 보스쪽에서 주기
            BossWall.SetActive(false);
        }
    }
    */

    public void MoveBound(int index)
    {
        CameraManager.SetStageIndex(index);
    }

    public void SetZoom(float size)
    {
        CameraManager.SetZoom(size, 4f);
    }
}
