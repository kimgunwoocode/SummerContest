using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSceneArea : MonoBehaviour
{
    [Header("�̵��� �� ����")]
    public string SceneName;
    [Tooltip("���� ������ �Ѿ�� �� ��� �������� �÷��̾ ��ȯ������ ID�� ����.\n ID�� �˸´����� �˾Ƽ� Ȯ�����ּ���... ������.")]
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
        // �� �̵� �ִϸ��̼�
        gamemanager.CurrentScenePointID = NextPointID;
        gamemanager.CurrentSceneName = SceneName;
        SceneManager.LoadScene(SceneName);
    }
}
