using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class SavePointUI : MonoBehaviour
{
    GameDataManager data;

    public Image[] BreathIconButton;
    public Sprite nullSprite;

    void Start()
    {
        data = Singleton.GameManager_Instance.Get<GameDataManager>();
        for (int i = 0; i < BreathIconButton.Length; i++)
        {
            // 장비 얻었는지 여부에 따라 표기가 다름
            if (!data.PlayerSkill.TryGetValue(i, out bool isUnlocked) || !isUnlocked)
            {
                BreathIconButton[i].sprite = nullSprite;
            }
        }

    }


}
