using UnityEngine;
using UnityEngine.InputSystem;

public class BreakableWall : EnemyEntity
{
    // 인스펙터창에 변수 보이게 하려면 BreakableWallEditor 스크립트에서 설정하기


    public void init()
    {
        Destroy(gameObject);
    }
    public override void TakeDamage(int damageAmount, Vector2 attackerPosition)
    {
        // 애니메이션, 사운드 추가
        Destroy(gameObject);
    }


    #region overide
    protected override void Awake() { }
    protected override void Start() { }
    protected override void Update() { }
    protected override void FixedUpdate() { }
    public override void OnDrawGizmos() { }
    #endregion
}