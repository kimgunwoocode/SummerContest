using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapData
{
    public Dictionary<int,bool> InteractionObjects = new();// 상호작용 가능한 오브젝트들 <ID, 상호작용 여부>
    public Dictionary<int,bool> SpawnPoints = new();// 활성화된 스폰포인트 <ID, 활성화 여부>
    public int SpawnPoint = -1;// 부활or시작할 스폰포인트 ID
}

public class MapDataManager : MonoBehaviour
{
    public MapData GameManager_MapData { get; private set; }

    public void StartGame_LoadData(MapData MapData)
    {

    }
}
