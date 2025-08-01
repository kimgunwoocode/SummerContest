using UnityEngine;
using GameAudio;
using UnityEngine.SceneManagement;


/**
 *      Name                   : GameAudioData
 *      Last Update         : 2025-07-30
 *      Description          : Manage playing sounds
 *      Todo                    : Implement playing boss thema in runtime
 */
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    [SerializeField] private GameAudioData gameAudioData;
    private AudioSource currentBGMSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SetAudioSource();
        string sceneName = SceneManager.GetActiveScene().name;
        PlayMapBGM(sceneName);
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

    public void StopCurrentBGM()
    {
        currentBGMSource.Stop();
    }

    /* 맵에 따른 BGM 출력 */
    // 주의 : clip, currentMapBGMSource, currentMapBGMSource.clip 할당 확인
    private void PlayMapBGM(string sceneName)
    {
        if (!HasAudioSource())
            return;

        AudioClip clip = gameAudioData.GetMapBGMClip(sceneName);

        if (clip != null)
        {   
            if (currentBGMSource.clip != null)
            {
                if (currentBGMSource.clip != clip && currentBGMSource.isPlaying)
                {
                    currentBGMSource.Stop();
                //    Debug.Log("PlayMapBGM: Stop BGM -" + currentMapBGMSource.clip.name);
                }
                else
                {
                 //   Debug.Log("PlayMapBGM: Same Scene, continue play bgm - " + currentMapBGMSource.clip.name);
                    return;
                }
            }
            currentBGMSource.clip = clip;
        //   Debug.Log("PlayMapBGM: currentMapBGMSource: " + currentMapBGMSource.clip);

            currentBGMSource.Play();
            currentBGMSource.loop = true;
        //    Debug.Log("PlayMapBGM: Play" + currentMapBGMSource.clip.name);
        }
        else
        {
         //   Debug.LogWarning("PlayMapBGM: Map BGM is null");
        }
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
