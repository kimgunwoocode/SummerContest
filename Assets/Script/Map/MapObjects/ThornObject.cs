using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThornObject : MonoBehaviour
{
    GameManager GameManager;

    public List<GameObject> ThornSpawnPoints;// 스폰할 위치 목록
    public Vector2 SpawnPoint;// 플레이어가 함정에 걸린 후 되돌아갈 위치

    private void Start()
    {
        GameManager = Singleton.GameManager_Instance.Get<GameManager>();
        if (GameManager == null )
            Debug.Log("error gameobject null : " + GameManager);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Attack(collision);
            //애니메이션 및 효과
            SpawnPlayer_to_SpawnPoint(collision.gameObject);
        }
    }


    // 플레이어 공격 함수 가져다 쓰기 (엔티티 그거)
    void Attack(Collider2D collision)
    {
        //함정 걸린 위치 저장하기
        //AttackPosition = ;
    }


    void SpawnPlayer_to_SpawnPoint(GameObject Player)
    {
        if (SpawnPoint != null)
        {
            Player.transform.position = SpawnPoint;
        }
        else if (SpawnPoint == null)
        {
            SpawnPlayer_to_NearSpawnPoint(Player);
        }
    }



    // 그 다음 가장 가까운 ThornSpawnPoint 위치로 이동시키기
    void SpawnPlayer_to_NearSpawnPoint(GameObject Player)
    {
        if (ThornSpawnPoints == null)
        {
            //error : 스폰할 장소 지정 안함
        }

        GameObject nearest = ThornSpawnPoints[0];
        float minDistance = Vector2.Distance(Player.transform.position, nearest.transform.position);

        foreach (GameObject spawnPoint in ThornSpawnPoints)
        {
            float dist = Vector2.Distance(Player.transform.position, spawnPoint.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = spawnPoint;
            }
        }

        Player.transform.position = nearest.transform.position;
    }

}
