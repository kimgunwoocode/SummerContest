using UnityEngine;
using UnityEngine.UI;
public class SavePointUI : MonoBehaviour
{
    public GameObject SavePointPanel;
    public Button breath_1;

    void Start()
    {

    }

    // 세이브 포인트에 상호작용시 세이브포인트판넬 활성화
    public void EnterSavePoint()
    {
        SavePointPanel.SetActive(true);
    }
}
