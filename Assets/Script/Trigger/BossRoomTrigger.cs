using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] GameObject gumiho;
    [SerializeField] PlayerManager Player;
    [SerializeField] GameObject gumihoIntro;
    [SerializeField] ParticleSystem intro;
    [SerializeField] ParticleSystem[] rains;
    [SerializeField] Lightning lightning;
    [SerializeField] Light2D global;
    BoxCollider2D PlayerCollider;
    CameraManager cameraManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VFXSequence sequence = new VFXBuilder()
            .AppendCallBacks(() => {
                Player.SetControllable(false);
                PlayerCollider = Player.GetComponent<BoxCollider2D>();
                PlayerCollider.enabled = false;

                SoundManager.instance.StopCurrentBGM();
                foreach (var r in rains) {
                    r.Stop();
                }

                cameraManager = Camera.main.GetComponent<CameraManager>();
                cameraManager.CanCameraMove = false;

                lightning.StopAllCoroutines();
            })
            .AppendMoveCamera(Camera.main.transform.position + new Vector3(6, 0, 0), 1f)
            .AppendDelay(0.3f)

            .AppendShakeCamera(3f, new Vector3(0.1f, 0.1f, 0), 10)

            .AppendCallBacks(() => {
                gumihoIntro.SetActive(true);
                cameraManager.CanCameraMove = true;
                gumihoIntro.transform.DOMoveY(gumihoIntro.transform.position.y - 3f, 4f);
                //intro.Play();
            })

            .AppendDelay(0.2f)
            .AppendBlackOut(2f)

            .AppendBossNameAppearance(1f, "헛된 기다림", "구미호", 0.5f)
            .AppendCallBacks(() => {
                gumihoIntro.SetActive(false);
                gumiho.SetActive(true);
                global.intensity = 1;
                
            })

            .AppendDelay(0.5f)

            .AppendBossNameFadeOut(0.3f, true)
            .JoinBlackIn(0.7f)

            .AppendCallBacks(() => {
                PlayerCollider.enabled = true;
                Player.SetControllable(true);
                SoundManager.instance.PlayBossBGM(BossBgm.Gumiho);
            })
            .Build();
        }
    }
}