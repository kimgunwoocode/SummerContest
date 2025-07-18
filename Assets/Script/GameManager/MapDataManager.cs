using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapData
{
    public Dictionary<int,bool> InteractionObjects = new();// 상호작용 가능한 오브젝트들 <ID, 상호작용 여부>
    public List<ShopData> Shops = new();
    public Dictionary<int,bool> SpawnPoints = new();// 활성화된 스폰포인트 <ID, 활성화 여부>
    public int SpawnPoint = -1;// 부활or시작할 스폰포인트 ID
}
[System.Serializable]
public class ShopData
{
    public bool isOpen;
    public Dictionary<int,bool> Items = new();
}

public class MapDataManager : MonoBehaviour
{
    public MapData GameManager_MapData { get; private set; }

    //실제 게임 플레이 중에 접근할 변수들 :
    public Dictionary<int, bool> InteractionObjects = new();
    public List<ShopData> Shops = new();
    public Dictionary<int, bool> SpawnPoints = new();
    public int SpawnPoint = -1;
}
