using UnityEngine;

public class Yoko_DeadState : DeadState
{
    YoKo yoKo;

    public Yoko_DeadState(EnemyEntity enemy, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, YoKo yoKo) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.yoKo = yoKo;
    }
}
