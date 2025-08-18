using UnityEngine;
using UnityEngine.SceneManagement;

public class Map_1_X_GameClearScreen : MonoBehaviour
{
    GameManager _gameManager;
    private void Start()
    {
       _gameManager = Singleton.GameManager_Instance.Get<GameManager>(); 
    }

    public void GameClear()// 25.8.23 최종발표 기준
    {
        _gameManager.RequestTogglePause(false);

        VFXSequence sequence = new VFXBuilder()
            .AppendBlackOut(1.5f, true)
            .AppendBossNameAppearance(5f, "To Be Continued.....", "[드래곤전]", 1f, true)
            .AppendCallBacks(() => {
                SceneManager.LoadScene("Title");
            })
            .Build();
    }
}
