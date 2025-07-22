using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class ShopObject : MonoBehaviour
{
    GameManager GameManager;
    GameDataManager GameDataManager;
    [Header("판매하는 아이템 리스트")]
    public List<ItemData> SalesList = new();
    public Dictionary<int, bool> isSold = new();//판매여부 저장. 인스펙터창에서 확인 불가능. 아이템의 ID로 판매여부 검색하기

    [Space]
    [Header("상점 정보")]
    public int ID;
    public bool isOpened;

    private void Start()
    {
        GameManager = Singleton.GameManager_Instance.Get<GameManager>();
        GameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
    }
    public void Openshop()
    {

    }

    public void BuyItem(int ItemID)
    {
        ItemData data = SalesList.Find(item => item.itemID == ItemID);
        int price = data.price;
        ItemType type = data.itemType;
        if (type == ItemType.Breath)
            GameDataManager.Money -= price;
        GameManager.Get_Item(ItemID);
    }
}