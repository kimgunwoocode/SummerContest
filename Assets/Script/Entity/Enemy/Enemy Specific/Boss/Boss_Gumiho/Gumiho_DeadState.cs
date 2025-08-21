using UnityEngine.Events;
using UnityEngine;
using Unity.VisualScripting;

public class Gumiho_DeadState : DeadState
{
    public UnityEvent DieEvent;

    Boss_Gumiho gumiho;

    [SerializeField] private GameObject _dropTem;
    [SerializeField] private Transform _child;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);
        gumiho = enemy as Boss_Gumiho;
    }
    
    public override void Enter()
    {
        base.Enter();

        Debug.Log($"facingDIr: {enemy.facingDir}");
        _dropTem.SetActive(true);
        _dropTem.transform.position = _child.position; //new Vector2(gameObject.transform.position.x-0.45f,gameObject.transform.position.y);
        Debug.Log($"pos: {_dropTem.transform.position}");
        Vector2 _direction = new Vector2(enemy.facingDir, 1).normalized;
        _dropTem.GetComponent<Rigidbody2D>().AddForce(_direction * 5f, ForceMode2D.Impulse);


        // 여기에 아이템 획득 등의 함수 추가
        DieEvent?.Invoke();// 맵에서 등록한 구미호 처치와 관련된 메서드 실행
        //Singleton.GameManager_Instance.Get<GameManager>().Get_Item(1501);// 브레스 아이템 획득
    }
}