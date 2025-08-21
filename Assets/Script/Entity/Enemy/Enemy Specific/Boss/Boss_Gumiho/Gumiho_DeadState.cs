using UnityEngine.Events;

public class Gumiho_DeadState : DeadState
{
    public UnityEvent DieEvent;

    Boss_Gumiho gumiho;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        gumiho = enemy as Boss_Gumiho;
    }
    
    public override void Enter()
    {
        base.Enter();

        // 여기에 아이템 획득 등의 함수 추가
        DieEvent?.Invoke();// 맵에서 등록한 구미호 처치와 관련된 메서드 실행
        //Singleton.GameManager_Instance.Get<GameManager>().Get_Item(1501);// 브레스 아이템 획득
    }
}
