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
            switch (sceneName)
            {
                case "Title":
                    return themaBGM.lobbyBGM;
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
                case "1-X_Gumiho":
                    return themaBGM.MapBGM.heavyRain;
                default:
                    return null;
            }
        }
        public AudioClip GetBossBGMClip(BossBgm bossBgm)
        {
            switch (bossBgm)
            {
                case BossBgm.Gumiho:
                    return themaBGM.BossBGM.Gumiho;
                default:
                    return null;
            }
        }

        public AudioClip GetSfxClip(string sfxName)
        {
            switch (sfxName)
            {
                /* player */
                case "player_attack":
                    return effectSound.characterSound.attack;
                case "player_run":
                    return effectSound.characterSound.run;
                case "player_jump":
                    return effectSound.characterSound.jump;
                case "player_dash":
                    return effectSound.characterSound.dash;
                case "player_damage":
                    return effectSound.characterSound.damage;
                case "player_eggCrack":
                    return effectSound.characterSound.eggCrack;
                /* enemy */
                case "enemy_damage":
                    return effectSound.enemySound.damage;
                case "enemy_death":
                    return effectSound.enemySound.death;
                case "yoko_jump":
                    return effectSound.enemySound.yoko_jump;
                case "yoko_attack":
                    return effectSound.enemySound.yoko_attack;
                case "fenFire_attack":
                    return effectSound.enemySound.fenFire_attack;
                case "jaii_charge":
                    return effectSound.enemySound.jaii_charge;
                case "jaii_attack":
                    return effectSound.enemySound.jaii_attack;
                    /* gumiho */
                case "gumiho_clawAttack":
                    return effectSound.enemySound.clawAttack;
                case "gumiho_tailAttack":
                    return effectSound.enemySound.tailAttack;
                case "gumiho_foxOrb":
                    return effectSound.enemySound.foxOrb;
                case "gumiho_foxFire":
                    return effectSound.enemySound.foxFire;
                case "gumiho_jump":
                    return effectSound.enemySound.jump;
                case "gumiho_spiritLeap":
                    return effectSound.enemySound.spiritLeap;
                case "gumiho_phase2":
                    return effectSound.enemySound.phase2;
                /* UI */
                case "ui_heartUp":
                    return effectSound.uiSound.heartUp;
                case "ui_click":
                    return effectSound.uiSound.click;
                case "ui_paper":
                    return effectSound.uiSound.paper;
                /* map */
                case "map_save":
                    return effectSound.mapSound.save;
                case "map_Thunder1":
                    return effectSound.mapSound.Lightning1;
                case "map_Thunder2":
                    return effectSound.mapSound.Lightning2;
                case "map_Thunder3":
                    return effectSound.mapSound.Lightning3;
                case "map_break1":
                    return effectSound.mapSound.break1;
                case "map_break2":
                    return effectSound.mapSound.break2;
                case "map_break3":
                    return effectSound.mapSound.break3;
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
        public AudioClip heavyRain;
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
        public MapSoundGroup mapSound;
    }

    /* Character Effect Sounds */
    [System.Serializable]
    public class CharacterSoundGroup
    {
        public AudioClip run;
        public AudioClip attack;
        public AudioClip jump;
        public AudioClip dash;
        public AudioClip breath;
        public AudioClip damage;
        public AudioClip eggCrack;
    }

    /* Enemy Effect Sounds */
    [System.Serializable]
    public class EnemySoundGroup
    {
        public AudioClip damage;
        public AudioClip death;

        public AudioClip yoko_jump;
        public AudioClip yoko_attack;
        public AudioClip fenFire_attack;
        public AudioClip jaii_charge;
        public AudioClip jaii_attack;

        /* Boss Gumiho */
        public AudioClip clawAttack;
        public AudioClip tailAttack;
        public AudioClip foxOrb;
        public AudioClip foxFire;
        public AudioClip jump;
        public AudioClip spiritLeap;
        public AudioClip phase2;
    }

    /* UI Effect Sounds */
    [System.Serializable]
    public class UISoundGroup
    {
        public AudioClip heartUp;
        public AudioClip click;
        public AudioClip paper;
        public AudioClip confirm;
        public AudioClip cancel;
    }

    /* Map Effect Sounds */
    [System.Serializable]
    public class MapSoundGroup
    {
        public AudioClip save;
        public AudioClip Lightning1;
        public AudioClip Lightning2;
        public AudioClip Lightning3;
        public AudioClip break1;
        public AudioClip break2;
        public AudioClip break3;
    }
}