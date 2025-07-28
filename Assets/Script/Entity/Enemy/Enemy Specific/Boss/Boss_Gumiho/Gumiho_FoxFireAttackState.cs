using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.iOS;

public class Gumiho_FoxFireAttackState : AttackState
{
    [SerializeField] private GameObject foxOrbPrefab;
    [SerializeField] private float rotationSpeed = 2f;

    [SerializeField, Tooltip("반경 증가 속도")] 
    private float radiusIncreaseSpeed = 0.5f;

    [SerializeField, Tooltip("초기 반경")]
    private float circleRadius;

    [SerializeField, Tooltip("최대 반경")] 
    private float maxRadius = 10f;
    [SerializeField] private int numberOfOrbs = 5;

    private Boss_Gumiho gumiho;
    private List<GameObject> foxFires;  // 여우불 오브젝트를 저장할 리스트
    private float currentCircleRadius;

    public override void Initialize(EnemyEntity enemy, FiniteStateMachine stateMachine)
    {
        base.Initialize(enemy, stateMachine);

        animBoolName = "foxFireAttack";
        gumiho = enemy as Boss_Gumiho;
    }

    public override void Enter()
    {
        base.Enter();

        currentCircleRadius = circleRadius;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 공격 끝났으면 상태 전환
        if (isAnimationFinished)
        {
            stateMachine.ChangeState(gumiho.MoveState);
        }
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        foxFires = new List<GameObject>();

        // 여우불 5개 생성
        for (int i = 0; i < numberOfOrbs; i++)
        {
            // 초기 위치를 동심원 형태로 설정
            float angle = 360f / numberOfOrbs * i;
            Vector2 initialPosition = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle) * circleRadius, Mathf.Sin(Mathf.Deg2Rad * angle) * circleRadius);

            GameObject foxFire = Instantiate(foxOrbPrefab, attackPosition.position + (Vector3)initialPosition, Quaternion.identity);
            foxFires.Add(foxFire);
        }

        // 여우불의 회전 및 반경 증가를 코루틴으로 처리
        StartCoroutine(MoveFoxFires());
    }

    public override void FinishAttack()
    {
        for (int i = 0; i < foxFires.Count; i++)
        {
            Destroy(foxFires[i]);
        }

        base.FinishAttack();
    }

    private IEnumerator MoveFoxFires()
    {
        float deg = 0f;

        while (currentCircleRadius <= maxRadius) // 반경이 최대값에 도달할 때까지
        {
            deg += rotationSpeed * Time.deltaTime; // 회전

            if (deg >= 360f)
            {
                deg = 0f;
            }

            for (int i = 0; i < foxFires.Count; i++)
            {
                GameObject foxFire = foxFires[i];
                
                if(foxFire == null) continue;

                // 각 여우불의 위치를 동심원으로 회전
                float angle = deg + (i * (360f / numberOfOrbs)); // 각 여우불에 대한 회전 속도 조정
                float x = currentCircleRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
                float y = currentCircleRadius * Mathf.Sin(Mathf.Deg2Rad * angle);
                foxFire.transform.position = transform.position + new Vector3(x, y, 0);
            }

            // 반경 증가
            currentCircleRadius += radiusIncreaseSpeed * Time.deltaTime;

            yield return null;
        }

        // 최대 반경에 도달하면 여우불의 이동을 멈추고 공격 종료
        FinishAttack();
    }
}