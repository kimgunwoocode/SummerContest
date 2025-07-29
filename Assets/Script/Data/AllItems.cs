using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Init/all items")]
public class AllItems : ScriptableObject
{
    public List<ItemData> allitems = new();
}
