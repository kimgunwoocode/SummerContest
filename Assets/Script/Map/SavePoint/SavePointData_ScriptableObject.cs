using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Map/SavePoint_list")]
public class SavePoint_list : ScriptableObject
{
    //ID 0���� ���� ����Ʈ
    //���� ����Ʈ�� 1001����~ ���� ����Ʈ�� 2001����~
    Dictionary<int, SavePoint> SavePoint_IDlist = new();
}
