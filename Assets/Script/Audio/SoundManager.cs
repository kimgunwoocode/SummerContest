using UnityEngine;
using GameAudio;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


/**
 *      Name                   : GameAudioData
 *      Last Update         : 2025-08-01
 *      Description          : Manage playing sounds
 *      Todo                    : Implement playing boss thema in runtime
 */
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    [SerializeField] private GameAudioData gameAudioData;       // 음원 데이터셋
    private AudioSource currentBGMSource;
    public AudioMixer mainMixer;
    public Slider bgmSlider;

    public enum VolumeName { BGM_Volume, SFX_Volume, Master_Volume }

    public void SetVolume(VolumeName VolumeName, float volume) {
        if (volume == 0) {
            mainMixer.SetFloat(VolumeName.ToString(), -80f);
        } else {
            mainMixer.SetFloat(VolumeName.ToString(), Mathf.Log10(volume) * 20);
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            SetAudioSource();
            string sceneName = SceneManager.GetActiveScene().name;
            PlayMapBGM(sceneName);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMapBGM(scene.name);
    }

    /* AudioSource Null Check */
    private bool HasAudioSource()
    {
        if (currentBGMSource == null)
        {
            Debug.LogError("Failed to Get AudioSources");
            return false;
        }
        else
            return true;
    }

    /* AudioSource에 컴포넌트 가져오기  */
    private void SetAudioSource()
    {
        if (currentBGMSource == null)
        {
            // 현재 gameObject에 붙어있는 컴포넌트 가져온다
            currentBGMSource = gameObject.GetComponent<AudioSource>();
            // Debug.Log("GetComponent Done");
            if (currentBGMSource == null)
            {
                // 직접 새 컴포넌트를 추가한다
                currentBGMSource = gameObject.AddComponent<AudioSource>();
                //   Debug.Log("AddComponent Done");
            }
        }
    }

    /* BGM 종료 */
    public void StopCurrentBGM()
    {
        currentBGMSource.Stop();
    }

    /* 맵에 따른 BGM 출력 */
    private void PlayMapBGM(string sceneName)
    {
        // Null Check
        if (!HasAudioSource())
            return;
        AudioClip clip = gameAudioData.GetMapBGMClip(sceneName);

        if (clip != null) {   
            if (currentBGMSource.clip != null) {
                if (currentBGMSource.clip != clip) {
                    currentBGMSource.Stop();
                }
            }
            currentBGMSource.clip = clip;
            currentBGMSource.Play();
            currentBGMSource.loop = true; 
            
            //    Debug.Log("PlayMapBGM: Play" + currentMapBGMSource.clip.name);
        }
        /*
        else if(sceneName == "Title")
        {
            StopCurrentBGM();
            
        }
        */
    }


    public void PlayBossBGM(string bossName)
    {
        if (!HasAudioSource()) return;

        currentBGMSource.clip = gameAudioData.GetBossBGMClip(bossName);
        Debug.Log("PlayBossBGM: bossName -" +  currentBGMSource.clip);
        if (currentBGMSource.clip != null)
        {
            currentBGMSource.Play();
            currentBGMSource.loop = true;
            Debug.Log("PlayBossBGM: Play Boss BGM");
        }
        else
        {
            Debug.LogWarning("PlayBossBGM: Boss BGM is null");
        }

    }
}
