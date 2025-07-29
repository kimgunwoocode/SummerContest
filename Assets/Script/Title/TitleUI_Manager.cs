using UnityEngine;

public class TitleUI_Manager : MonoBehaviour
{
    public GameObject SelectPanel_Screen;



    private void Start()
    {
        SelectPanel_Screen.SetActive(false);
    }


    public void Open_SelectPanel_Screen()
    {
        SelectPanel_Screen.SetActive(true);
    }

    public void Closs_SelectPanel_Screen()
    {
        SelectPanel_Screen.SetActive(false);
    }
}
