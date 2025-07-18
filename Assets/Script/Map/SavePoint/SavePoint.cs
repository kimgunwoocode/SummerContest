using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public enum SP_type { Main, Semi };
    [Header("type option")]
    public SP_type SavePoint_type;
    [Header("ID")]
    public int SavePoint_ID;
    [Space]
    [Header("is activate")]
    public bool SavePointEnabled;
    
    public void InteractSavePoint()
    {
        print("SavePoint_"+SavePoint_type+" ID:"+SavePoint_ID);
        Singleton.GameManager_Instance.Get<GameManager>().SaveData__SavePoint();
    }
}