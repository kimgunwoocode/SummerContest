using UnityEngine;

public class BurnWall : EnemyEntity
{
    GameDataManager gameDataManager;
    Interaction interaction;

    public override void TakeDamage(int damageAmount, Vector2 attackerPosition)
    {
        // 애니메이션, 사운드 추가
        gameDataManager.InteractionObjects[interaction.ID] = false;
        gameObject.SetActive(false);
    }


    #region overide
    protected override void Awake()
    {
        gameDataManager = Singleton.GameManager_Instance.Get<GameDataManager>();
        interaction = gameObject.GetComponent<Interaction>();
    }
    protected override void Start() { }
    protected override void Update() { }
    protected override void FixedUpdate() { }
    public override void OnDrawGizmos() { }
    #endregion
    public void init()
    {
        Destroy(gameObject);
    }
}
