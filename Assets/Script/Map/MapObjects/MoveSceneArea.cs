using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSceneArea : MonoBehaviour
{
    [Header("이동할 씬 정보")]
    public string SceneName;
    [Tooltip("다음 씬으로 넘어갔을 때 어느 지점으로 플레이어를 소환할지를 ID로 정함.\n ID가 알맞는지는 알아서 확인해주세요... 수동임.")]
    public int NextPointID;
    GameManager gamemanager;

    private void Start()
    {
        gamemanager = Singleton.GameManager_Instance.Get<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MoveScene();
    }

    void MoveScene()
    {
        // 맵 이동 애니메이션
        gamemanager.CurrentScenePointID = NextPointID;
        gamemanager.CurrentSceneName = SceneName;
        SceneManager.LoadScene(SceneName);
    }
}
