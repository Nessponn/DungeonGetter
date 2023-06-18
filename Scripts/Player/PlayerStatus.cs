using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : SingletonMonoBehaviourFast<PlayerStatus>
{
    //プレイヤーの現在の状態等をコントロール、反映するスクリプト

    public int HP = 5;//体力

    public float Low_Gun_Gage;//消費ガンゲージ

    private float Gun_Gage;//ガンゲージ
    public float Gun_GageMAX;//最大ガンゲージ


    //オブジェクト

    public Image GunGage_Object;

    private float t;//素早く回復するまでのリミット時間

    public int Tresure_Count;
    public Text Tresure_CountTx;//宝の残り数

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int Tresure_value)
    {
        Tresure_Count = Tresure_value;
        Tresure_CountTx.text = "" + Tresure_value;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        //ガンゲージの回復
        if (t <=　1.4f) Gun_Gage += 0.03f;
        else Gun_Gage += 0.7f;

        if (Gun_Gage >= Gun_GageMAX) Gun_Gage = Gun_GageMAX;

        //ガンゲージの画面反映

        GunGage_Object.fillAmount = Gun_Gage / Gun_GageMAX;
    }

    //銃を使ったときの処理
    public void Use_Stamina(float value , float Handycap)
    {
        Gun_Gage -= value;
        t = 0 + Handycap;
    }

    //スタミナを消費できるか
    public bool CanIUseGun(float value)
    {
        return Gun_Gage >= value;
    }

    //Hpを回復
    public void Hp_Recover()
    {
        HP++;
    }

    //HPを消費
    public void Hp_Decrease()
    {
        HP--;
    }

    public void Get_Tresure()
    {
        Tresure_Count--;
        Tresure_CountTx.text = "" + Tresure_Count;
    }
}
