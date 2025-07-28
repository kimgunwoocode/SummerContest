using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Init/InitData")]
public class InitSaveData : ScriptableObject
{
    public SaveData InitData = new();
}
