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
    private AudioSource currentMapBGMSource;

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

    /* 맵에 따른 BGM 출력 */
    // 주의 : clip, currentMapBGMSource, currentMapBGMSource.clip 할당 확인
    private void PlayMapBGM(string sceneName)
    {
        AudioClip clip = gameAudioData.GetMapBGMClip(sceneName);

        if (currentMapBGMSource == null)
        {
            // 현재 gameObject에 붙어있는 컴포넌트 가져온다
            currentMapBGMSource = gameObject.GetComponent<AudioSource>();
            // Debug.Log("GetComponent Done");
            if (currentMapBGMSource == null)
            {
                // 직접 새 컴포넌트를 추가한다
                currentMapBGMSource = gameObject.AddComponent<AudioSource>();
             //   Debug.Log("AddComponent Done");
            }
        }

        if (clip != null)
        {   
            if (currentMapBGMSource.clip != null)
            {
                if (currentMapBGMSource.clip != clip && currentMapBGMSource.isPlaying)
                {
                    currentMapBGMSource.Stop();
                //    Debug.Log("Stop BGM" + currentMapBGMSource.clip.name);
                }
                else
                {
                 //   Debug.Log("Same Scene, continue play bgm: " + currentMapBGMSource.clip.name);
                    return;
                }
            }
            currentMapBGMSource.clip = clip;
        //   Debug.Log("currentMapBGMSource: " + currentMapBGMSource.clip);

            currentMapBGMSource.Play();
            currentMapBGMSource.loop = true;
        //    Debug.Log("Play BGM: " + currentMapBGMSource.clip.name);
        }
        else
        {
         //   Debug.LogWarning("No map BGM!!");
        }
    }
}
