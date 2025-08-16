using System;
using UnityEngine;

public class SystemDataManager : MonoBehaviour
{
    public SystemData SystemData;
}

[Serializable]
public class SystemData
{
    public float MouseSensitivity;

    public float MasterVolume;
    public float BGMVolume;
    public float SFXVolume;
}