using UnityEngine;
using System.Collections.Generic;

public class ShopObject : MonoBehaviour
{

    [Header("판매하는 아이템 리스트")]
    public List<ItemData> SalesList = new();
    public Dictionary<int, bool> isSold = new();//판매여부 저장. 인스펙터창에서 확인 불가능. 아이템의 ID로 판매여부 검색하기

    [Space]
    [Header("상점 정보")]
    public int ID;
    public bool isOpened;


    public void Openshop()
    {

    }


}