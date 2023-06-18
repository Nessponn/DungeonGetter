using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoradAnimation : SingletonMonoBehaviourFast<BoradAnimation>
{
    private bool Board;//ボードを出しているとき

    public GameObject[] Life;
    public GameObject Gages;
    public GameObject BlackScreen;
    public GameObject BlackScreen2;

    private Color BoradColor;

    public GameObject Borad;//ボードオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        BoradColor = Borad.GetComponent<Image>().color;

        BlackScreen.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0f);
        BlackScreen2.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0f);
    }
    //ボードを出す
    public void Board_Pop()
    {
        //プレイ画面→ボード
        for (int i = 0; i < Life.Length; i++)
        {
            for (int j = 0; j < Life[i].transform.childCount; j++)
            {
                Life[i].transform.GetChild(j).transform.DOLocalMoveY(10, 0.5f);
                Life[i].transform.GetChild(j).GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 1), 0.5f);
            }

        }
        //ボード周りのアニメーション
        BlackScreen.GetComponent<RectTransform>().DOAnchorPosY(250, 0.5f);
        BlackScreen.GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 0.5f);

        BlackScreen2.GetComponent<RectTransform>().DOAnchorPosY(-250, 0.5f);
        BlackScreen2.GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 0.5f);

        Gages.GetComponent<RectTransform>().DOAnchorPosY(-320, 0.5f);
        Gages.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.5f);

        //ボードを出す


        Borad.GetComponent<RectTransform>().DOAnchorPosY(150, 0.5f);
        Borad.GetComponent<Image>().DOColor(BoradColor, 0.5f);
    }

    //ボードをしまう

    public void Board_Push()
    {
        for (int i = 0; i < Life.Length; i++)
        {
            for (int j = 0; j < Life[i].transform.childCount; j++)
            {
                Life[i].transform.GetChild(j).transform.DOLocalMoveY(7, 0.5f);
                Life[i].transform.GetChild(j).GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 0.5f);
            }

        }
        //ボード周りのアニメーション
        BlackScreen.GetComponent<RectTransform>().DOAnchorPosY(350, 0.5f);
        BlackScreen.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.5f);

        BlackScreen2.GetComponent<RectTransform>().DOAnchorPosY(-350, 0.5f);
        BlackScreen2.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.5f);

        Gages.GetComponent<RectTransform>().DOAnchorPosY(-270, 0.5f);
        Gages.GetComponent<Image>().DOColor(new Color(1, 1, 1, 1), 0.5f);

        //ボードをしまう
        Borad.GetComponent<RectTransform>().DOAnchorPosY(400, 0.5f);
        Borad.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.5f);
    }
}
