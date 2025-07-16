using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSceneArea : MonoBehaviour
{
    public string SceneName;
    GameManager gamemanager;

    private void Awake()
    {
        gamemanager = Singleton.Get<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MoveScene();
    }

    void MoveScene()
    {
        // �� �̵� �ִϸ��̼�
        gamemanager.CurrentSceneName = SceneName;
        SceneManager.LoadScene(SceneName);
    }
}
