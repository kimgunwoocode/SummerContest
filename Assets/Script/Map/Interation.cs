using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Interation : MonoBehaviour
{
    [Header("상호작용시 실행할 이벤트 (메서드)")]
    public UnityEvent InterationEvent;
    [Space(30)]
    public GameObject InteractionGuide;

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }
}
