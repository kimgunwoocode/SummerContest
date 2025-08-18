using System.Collections;
using UnityEngine;

public class BurnWall : EnemyEntity
{
    GameDataManager gameDataManager;
    Interaction interaction;
    Material burnWallMat;

    public override void TakeDamage(int damageAmount, Vector2 attackerPosition, AttackType attackType)
    {
        if (attackType == AttackType.Breath)
        {
            // 애니메이션, 사운드 추가
            gameDataManager.InteractionObjects[interaction.ID] = false;
            StartCoroutine(Burn(1.2f));
        }
    }
    private IEnumerator Burn(float duration) {
        float ET = 0;
        float startX = 0;
        float endX = 1.1f;
        while (ET < duration) {
            ET += Time.deltaTime;
            float newDissolve = Mathf.Lerp(startX, endX, ET/duration);
            burnWallMat.SetFloat("_Dissolve", newDissolve);
            yield return null;
        }

        burnWallMat.SetFloat("_Dissolve", endX);
        gameObject.SetActive(false);
    }


    #region overide
    protected override void Awake()
    {
        gameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
        interaction = gameObject.GetComponent<Interaction>();
        burnWallMat = gameObject.GetComponent<SpriteRenderer>().material;
    }
    protected override void Start() { }
    protected override void Update() { }
    protected override void FixedUpdate() { }
    public override void OnDrawGizmos() { }
    #endregion
    public void Init()
    {
        Destroy(gameObject);
    }
}
