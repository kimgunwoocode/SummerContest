using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CollectionButton : MonoBehaviour
{
    GameDataManager data;

    [Header("해당 버튼의 정보")]
    public int MyId;                      // 해당 버튼에 할당된 장비의 아이디 번호
    public Text L_ItemName;               // 아이템 이름 출력 텍스트박스 - 버튼쪽

    [Header("텍스트 및 이미지 출력 창")]
    public Text ItemName;               // 아이템 이름 출력 텍스트박스
    public Text ItemInfor;              // 아이템 정보 출력 텍스트박스
    public Image ItemImage;             // 아이템 이미지 출력 이미지창


    void Start()
    {
        data = Singleton.GameManager_Instance.Get<GameDataManager>();

        if (data.PlayerSkill.TryGetValue(MyId, out bool isUnlocked) && isUnlocked)
        {
            // 획득
            L_ItemName.text = data.allitems[MyId].itemName;
        }
        else
        {
            // 미획득
            L_ItemName.text = "???";
        }
    }

    public void ClickButton()
    {

        if (data.PlayerSkill.TryGetValue(MyId, out bool isUnlocked) && isUnlocked)
        {
            // 획득
            ItemName.text = data.allitems[MyId].itemName;
            ItemInfor.text = data.allitems[MyId].description;
            ItemImage.color = new Color(1f, 1f, 1f, 1f);
            ItemImage.sprite = data.allitems[MyId].icon;
        }
        else
        {
            // 미획득
            ItemName.text = "???";
            ItemInfor.text = "???";
            ItemImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            ItemImage.sprite = data.allitems[MyId].icon;
        }
    }

}
