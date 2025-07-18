using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "InitData")]
public class InitSaveData : ScriptableObject
{
    public MapData MapData = new();
    public PlayerData PlayerData = new();
}
