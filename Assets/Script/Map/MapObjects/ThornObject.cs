using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThornObject : MonoBehaviour
{
    GameManager GameManager;

    public List<GameObject> ThornSpawnPoints;// 함정에 걸린 이후에 다시 스폰할 위치
    public Vector2 SpawnPoint;// 플레이어가 함정에 걸린 후 되돌아갈 위치

    private void Awake()
    {
        GameManager = Singleton.GameManager_Instance.Get<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attack(collision);
        //애니메이션

    }


    // 플레이어 공격 함수 가져다 쓰기 (엔티티 그거)
    void Attack(Collider2D collision)
    {
        //함정 걸린 위치 저장하기
        //AttackPosition = ;
    }


    void SpawnPlayer_to_SpawnPoint()
    {
        GameManager.Player.transform.position = SpawnPoint;
    }


    /*
    // 그 다음 가장 가까운 ThornSpawnPoint 위치로 이동시키기
    void SpawnPlayer_to_NearSpawnPoint()
    {
        if (ThornSpawnPoint_Position == null)
        {
            //예외처리 (Die로 처리할까...?)
        }

        Transform nearest = ThornSpawnPoint_Position[0];
        float minDistance = Vector2.Distance(GameManager.Player.transform.position, nearest.position);

        foreach (Transform spawnPoint in ThornSpawnPoint_Position)
        {
            float dist = Vector2.Distance(GameManager.Player.transform.position, spawnPoint.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = spawnPoint;
            }
        }

        GameManager.Player.transform.position = nearest.position;
    }
    */
}
