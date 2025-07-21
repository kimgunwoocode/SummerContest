using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public GameObject ShopPanel;
    public ShopItem shopItem;
    void Start()
    {

    }

    // 상점에 상호작용시 상점판넬 활성화, 각 버튼의 활성화 상태 초기화
    public void EnterShop()
    {
        ShopPanel.SetActive(true);
    }
}
