using UnityEngine;
using UnityEngine.InputSystem;

public class BreakableWall : EnemyEntity
{
    // 인스펙터창에 변수 보이게 하려면 BreakableWallEditor 스크립트에서 설정하기

    public Animator animator;

    GameDataManager gameDataManager;
    Interaction interaction;

    public void init()
    {
        if (animator == null)
            gameObject.SetActive(false);
        else
        {
            animator.SetTrigger("breaked");

            int childCount = gameObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public override void TakeDamage(int damageAmount, Vector2 attackerPosition)
    {
        // 애니메이션, 사운드 추가
        gameDataManager.InteractionObjects[interaction.ID] = false;

        if (animator == null)
            gameObject.SetActive(false);
        else
        {
            animator.SetTrigger("break");

            int childCount = gameObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void gameObject_SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    #region overide
    protected override void Awake() {
        gameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
        interaction = gameObject.GetComponent<Interaction>();
    }
    protected override void Start() { }
    protected override void Update() { }
    protected override void FixedUpdate() { }
    public override void OnDrawGizmos() { }
    #endregion
}