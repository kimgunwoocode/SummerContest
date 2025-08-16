using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static SoundManager;

public class UISoundManamger : MonoBehaviour
{
    SoundManager _SoundManager;
    SystemDataManager _SystemDataManager;

    public Slider MasterVolume_Slider;
    public Slider BGMVolume_Slider;
    public Slider SFXVolume_Slider;


    private void Start()
    {
        _SoundManager = SoundManager.instance;
        _SystemDataManager = Singleton.GameManager_Instance.Get<SystemDataManager>();
        SetSliderValue();
    }

    public void SetSliderValue()
    {
        MasterVolume_Slider.value = _SystemDataManager.SystemData.MasterVolume;
        BGMVolume_Slider.value = _SystemDataManager.SystemData.BGMVolume;
        SFXVolume_Slider.value = _SystemDataManager.SystemData.SFXVolume;
        SetVolume(VolumeName.Master_Volume, _SystemDataManager.SystemData.MasterVolume);
        SetVolume(VolumeName.BGM_Volume, _SystemDataManager.SystemData.BGMVolume);
        SetVolume(VolumeName.SFX_Volume, _SystemDataManager.SystemData.SFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        //MasterVolume_Slider.value = _SystemDataManager.SystemData.MasterVolume;
        SetVolume(VolumeName.Master_Volume, volume);
        _SystemDataManager.SystemData.MasterVolume = volume;
    }
    public void SetBGMVolume(float volume)
    {
        //BGMVolume_Slider.value = _SystemDataManager.SystemData.BGMVolume;
        SetVolume(VolumeName.BGM_Volume, volume);
        _SystemDataManager.SystemData.BGMVolume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        //SFXVolume_Slider.value = _SystemDataManager.SystemData.SFXVolume;
        SetVolume(VolumeName.SFX_Volume, volume);
        _SystemDataManager.SystemData.SFXVolume = volume;
    }

    private void SetVolume(VolumeName VolumeName, float Volume)
    {
        _SoundManager.SetVolume(VolumeName, Volume);
    }
}
