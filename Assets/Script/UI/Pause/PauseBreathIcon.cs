using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseBreathIcon : MonoBehaviour
{
    GameDataManager data;
    public int MyId;
    public Image BreathImage;
    public Sprite NullSprite;

    void Start()
    {
        data = Singleton.GameManager_Instance.Get<GameDataManager>();

        // 해당 아이콘의 정보 보이기
        // 장비 얻었는지 여부에 따라 표기가 다름
        if (data.PlayerSkill.TryGetValue(MyId, out bool isUnlocked) && isUnlocked)
        {
            BreathImage.sprite = data.allitems[MyId].icon;
        }
        else
        {
            BreathImage.sprite = NullSprite;
        }
    }

}
