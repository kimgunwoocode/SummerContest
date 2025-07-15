using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSceneArea : MonoBehaviour
{
    public string SceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MoveScene();
    }

    void MoveScene()
    {
        // 맵 이동 애니메이션
        SceneManager.LoadScene(SceneName);
    }
}
