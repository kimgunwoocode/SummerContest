using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/**
 *      Name                   : GameAudioData
 *      Last Update         : 2025-07-30
 *      Description          : Allocate Audio Sources and return to "SoundManager"
 *      Todo                    : Implement other return functions
 */

namespace GameAudio
{
    [CreateAssetMenu(fileName = "GameAudioData", menuName = "Audio/Game Audio Data")]
    public class GameAudioData : ScriptableObject
    {
        public ThemaBGMGroup themaBGM;
        public EffectSoundGroup effectSound;

        // 현재 씬에 따라 적절한 bgm을 리턴한다
        public AudioClip GetMapBGMClip(string sceneName)
        {
           //  Debug.Log("Scene Name: " + sceneName);
            switch (sceneName)
            {
                case "1-1_ForgottenNest":
                    return themaBGM.MapBGM.forgottenNest;
                case "1-2_FoxForest":
                    return themaBGM.MapBGM.foxForest;
                case "1-3_FoxForest":
                    return themaBGM.MapBGM.burnedFoxForest;
                case "1-4_FoxHole":
                    return themaBGM.MapBGM.foxHole;
                case "1-5_ThousandTree":
                    return themaBGM.MapBGM.thousandTree;
                default:
                    return null;
            }
        }
        public AudioClip GetBossBGMClip(string bossName)
        {
            switch (bossName)
            {
                case "Gumiho":
                    return themaBGM.BossBGM.Gumiho;
                default:
                    return null;
            }
        }
    }

    /* All BGMs */
    [System.Serializable]
    public class ThemaBGMGroup
    {
        public AudioClip lobbyBGM;
        public MapBGMGroup MapBGM;
        public BossBGMGroup BossBGM;
    }

    /* Thema BGMs */
    [System.Serializable]
    public class MapBGMGroup
    {
        public AudioClip forgottenNest;
        public AudioClip foxForest;
        public AudioClip burnedFoxForest;
        public AudioClip foxHole;
        public AudioClip thousandTree;
    }

    /* Boss Stage BGMs */
    [System.Serializable]
    public class BossBGMGroup
    {
        public AudioClip Gumiho;
    }

    /* All Effect Sounds */
    [System.Serializable]
    public class EffectSoundGroup
    {
        public CharacterSoundGroup characterSound;
        public EnemySoundGroup enemySound;
        public UISoundGroup uiSound;
    }

    /* Character Effect Sounds */
    [System.Serializable]
    public class CharacterSoundGroup
    {
        public AudioClip attack;
        public AudioClip jump;
        public AudioClip breath;
    }

    /* Enemy Effect Sounds */
    [System.Serializable]
    public class EnemySoundGroup
    {
        public AudioClip attack;
        public AudioClip death;
    }

    /* UI Effect Sounds */
    [System.Serializable]
    public class UISoundGroup
    {
        public AudioClip click;
        public AudioClip confirm;
        public AudioClip cancel;
    }
}