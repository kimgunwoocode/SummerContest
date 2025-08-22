using UnityEngine;
using TMPro;
using Mono.Cecil;

public class WOORUNG : MonoBehaviour
{
    [SerializeField] TMP_Text woorung;
    [SerializeField] PlayerManager woorungwoorungwoowoorungrung;

    GameDataManager woowoorung;
    GameManager woowoorungrung;
    int woorungwoorung = 0;
    int woorungwoorungwoowoo = 0;

    private void Start() {
        woowoorung = Singleton.GameManager_Instance.Get<GameDataManager>();
        woowoorungrung = Singleton.GameManager_Instance.Get<GameManager>();
        
    }

    private int WOO() {
        return Random.Range(1, 101);
    }

    public void Rung() {
        if(woowoorung.Money < 10) {
            switch (woorungwoorung) {
                case 0:
                    woorung.text = "꿈틀거린다...";
                    break;
                case 1:
                    woorung.text = "더욱 격렬히 꿈틀거리기 시작한다...";
                    break;
                case 2:
                    woorung.text = "더더욱 격렬히 꿈틀거리기 시작했다...";
                    break;
                case 3:
                    woorung.text = "꿈틀꿈틀꿈틀꿈틀...";
                    break;
                case 4:
                    woorung.text = "무엇인가 표현하고 싶은 것 같다...";
                    break;
                case 5:
                    woorung.text = "...?";
                    break;
                case 6:
                    woorung.text = "나한테 있는 금빛 반짝이를 원하는 것 같다...";
                    break;
                default:
                    woorung.text = "이걸 최대한 많이 들고 와보자.";
                    break;
            }
        woorungwoorung++;
        } else {
            woowoorung.Money -= 10;
            int woorungwoorungwoo = WOO();
            if (woorungwoorungwoowoo == 0) {
                woowoorung.Money += 30;
                woorung.text = "초심자의 행운!";
                woorungwoorungwoowoo++;
            } else {
                if (woorungwoorungwoo <= 50) {
                    woorung.text = "다음 기회에...";
                    return;
                } else if (50 < woorungwoorungwoo && woorungwoorungwoo <= 55) {
                    //이단
                    woowoorungrung.Get_Item(1502);
                    woorung.text = "하늘을 밟을 수 있을것만 같다...!";
                    return;
                } else if (55 < woorungwoorungwoo && woorungwoorungwoo <= 60) {
                    //활강
                    woowoorungrung.Get_Item(1504);
                    woorung.text = "몸이 가벼워졌다...!";
                    return;
                } else if (60 < woorungwoorungwoo && woorungwoorungwoo <= 65) {
                    //스킨변경
                    woowoorung.jangdok = true;
                    woowoorung.woorung = false;
                    woorungwoorungwoowoorungrung.LETSCHANGE();
                    woorung.text = "???????";
                    return;
                } else if (65 < woorungwoorungwoo && woorungwoorungwoo <= 70) {
                    //스킨 변경
                    woowoorung.woorung = true;
                    woorungwoorungwoowoorungrung.LETSCHANGE();
                    woorung.text = "????";
                    return;
                } else if (70 < woorungwoorungwoo && woorungwoorungwoo <= 80) {
                    //돈 +10
                    woowoorung.Money += 10;
                    woorung.text = "본전이 어디랴...";
                    return;
                } else if (80 < woorungwoorungwoo && woorungwoorungwoo <= 90) {
                    //돈 +20
                    woowoorung.Money += 20;
                    woorung.text = "성공!";
                    return;
                } else if (90 < woorungwoorungwoo && woorungwoorungwoo <= 100) {
                    //돈 +30
                    woowoorung.Money += 30;
                    woorung.text = "대성공!";
                    return;
                }
            }
        }
    }
}
