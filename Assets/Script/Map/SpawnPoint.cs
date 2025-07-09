using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public enum SpawnPoint_type { Main, Semi };
    [Header("type option")]
    public SpawnPoint_type spawnpoint_type;
    [Header("ID")]
    public int SpawnPoint_ID;
    
    public void InteractSpawnPoint()
    {
        print("SpawnPoint_"+spawnpoint_type+" ID:"+SpawnPoint_ID);
        Singleton.GameManager_Instance.GameDataManager.SaveData_to_SpawnPoint();
    }
}
