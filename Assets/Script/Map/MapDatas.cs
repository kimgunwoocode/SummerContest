﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public Dictionary<int, bool> InteractionObjects = new();// 상호작용 가능한 오브젝트들 <ID, 상호작용 여부>
    public Dictionary<int, Vector2> PushObjects = new();
    public List<ShopData> Shops = new();
    public Dictionary<int, bool> SpawnPoints = new();// 활성화된 스폰포인트 <ID, 활성화 여부>
    public int SpawnPoint = -1;// 부활or시작할 스폰포인트 ID
}
[System.Serializable]
public class ShopData
{
    public int ID;
    public bool isOpened;
    public Dictionary<int, bool> Items = new();// 아이템 ID, 판매 여부
}
