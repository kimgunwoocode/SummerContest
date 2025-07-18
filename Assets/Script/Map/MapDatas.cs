using System.Collections.Generic;

[System.Serializable]
public class MapData
{
    public Dictionary<int, int> InteractionObjects = new();// 상호작용 가능한 오브젝트들 <ID, 상호작용 여부>  --> 상호작용여부가 int인 이유는 interation스크립트 참고 (노션에도 기재함)
    public List<ShopData> Shops = new();
    public Dictionary<int, bool> SpawnPoints = new();// 활성화된 스폰포인트 <ID, 활성화 여부>
    public int SpawnPoint = -1;// 부활or시작할 스폰포인트 ID
}
[System.Serializable]
public class ShopData
{
    public int ID;
    public bool isOpened;
    public Dictionary<int, bool> Items = new();
}
