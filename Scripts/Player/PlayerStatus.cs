using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : SingletonMonoBehaviourFast<PlayerStatus>
{
    //�v���C���[�̌��݂̏�ԓ����R���g���[���A���f����X�N���v�g

    public int HP = 5;//�̗�

    public float Low_Gun_Gage;//����K���Q�[�W

    private float Gun_Gage;//�K���Q�[�W
    public float Gun_GageMAX;//�ő�K���Q�[�W


    //�I�u�W�F�N�g

    public Image GunGage_Object;

    private float t;//�f�����񕜂���܂ł̃��~�b�g����

    public int Tresure_Count;
    public Text Tresure_CountTx;//��̎c�萔

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

        //�K���Q�[�W�̉�
        if (t <=�@1.4f) Gun_Gage += 0.03f;
        else Gun_Gage += 0.7f;

        if (Gun_Gage >= Gun_GageMAX) Gun_Gage = Gun_GageMAX;

        //�K���Q�[�W�̉�ʔ��f

        GunGage_Object.fillAmount = Gun_Gage / Gun_GageMAX;
    }

    //�e���g�����Ƃ��̏���
    public void Use_Stamina(float value , float Handycap)
    {
        Gun_Gage -= value;
        t = 0 + Handycap;
    }

    //�X�^�~�i������ł��邩
    public bool CanIUseGun(float value)
    {
        return Gun_Gage >= value;
    }

    //Hp����
    public void Hp_Recover()
    {
        HP++;
    }

    //HP������
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
