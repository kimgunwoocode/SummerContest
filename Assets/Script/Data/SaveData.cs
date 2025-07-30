[System.Serializable]
public class SaveData
{
    public int Slot;
    public string Name;
    public string Day;
    public MapData MapData = new();
    public PlayerData PlayerData = new();
}