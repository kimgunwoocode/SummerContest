using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapDataManager : MonoBehaviour
{
    public MapData GameManager_MapData { get; private set; }

    //실제 게임 플레이 중에 접근할 변수들 :
    public Dictionary<int, bool> InteractionObjects = new();
    public List<ShopData> Shops = new();
    public Dictionary<int, bool> SpawnPoints = new();
    public int SpawnPoint = -1;
}
