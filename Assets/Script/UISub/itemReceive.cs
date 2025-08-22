using UnityEngine;
using TMPro;

public class itemReceive : MonoBehaviour
{

    [SerializeField] private TMP_InputField _inp;
    [SerializeField] private ItemHandler _itemHandler;

    public void Clicked() {
        _itemHandler.CallGetItem(int.Parse(_inp.text));
    }
}
