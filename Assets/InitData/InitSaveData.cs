using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "InitData")]
public class InitSaveData : ScriptableObject
{
    public SaveData InitData = new();
}
