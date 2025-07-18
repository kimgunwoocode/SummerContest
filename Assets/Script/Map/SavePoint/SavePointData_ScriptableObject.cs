using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Map/SavePoint_list")]
public class SavePoint_list : ScriptableObject
{
    //ID 0번은 시작 포인트
    //세미 포인트는 1001부터~ 메인 포인트는 2001부터~
    Dictionary<int, SavePoint> SavePoint_IDlist = new();
}
