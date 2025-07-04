using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ItemData.cs
//public enum ItemType { Skill, Breath, Nomal }

[CreateAssetMenu(menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite icon;
    public string description;
    //public ItemType itemType;
}


public class MapData
{
    public List<GameObject> InteractionObjects = new();
    public int SpawnPoint = -1;
}

public class PlayerData
{
    public Dictionary<string, bool> PlayerSkill = new();
}

public class SaveData
{
    public List<bool> isObjectsInteracted = new();
    public int SpawnPoint = -1;
}
