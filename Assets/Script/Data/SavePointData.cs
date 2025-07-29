using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Init/all SavePoint")]
public class SavePointData : ScriptableObject
{
    public List<SavePoint> allsavepoint = new();// 인스펙터창에서 확인하기위한 용도. 따라서 아이템 등록은 드래그가 아니라 버튼으로 하자!
    public Dictionary<int, SavePoint> allsavepoint_dic = new();// 실제 접근할 데이터
}
