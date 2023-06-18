using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonoBehaviourFast<Player>
{
    private Rigidbody2D rbody;
    private float Speed;
    private float SpeedY;

    public float MaxSpeed = 10;
    public float Jump_Power = 7;

    [HideInInspector] public bool Jump;


    public GameObject BulletPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //������
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (Jump) Speed -= 0.12f;
            else Speed -= 0.02f;
            if (Speed <= -MaxSpeed) Speed = -MaxSpeed;

            //rbody.velocity = new Vector2(Speed, rbody.velocity.y);
            if(rbody.velocity.x >= -MaxSpeed) rbody.AddForce(new Vector2(-3, 0));
        }

        //�E����
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (Jump) Speed += 0.12f;
            else Speed += 0.02f;

            if (Speed >= MaxSpeed) Speed = MaxSpeed;

            //rbody.velocity = new Vector2(Speed, rbody.velocity.y);
            if (rbody.velocity.x <= MaxSpeed) rbody.AddForce(new Vector2(3, 0));
        }

        //���E�ǂ�������͂��Ȃ�
        else
        {
            if (Speed > 0) Speed -= 0.1f;
            else if (Speed < 0) Speed += 0.1f;


            if (Speed <= 0.12f && Speed >= -0.12f) Speed = 0;
            
            rbody.velocity = new Vector2(Speed, rbody.velocity.y);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && Jump)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, Jump_Power);
        }

        if(rbody.velocity.y >= 1f && (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)))
        {
            rbody.velocity = new Vector2(rbody.velocity.x, -2);
        }

        if (rbody.velocity.y <= -30f)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, -30f);
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (PlayerStatus.Instance.CanIUseGun(20))
            {
                PlayerBullet();
                PlayerStatus.Instance.Use_Stamina(20,0);

                //�e��ł��������Ƃ͋t��
                // �v���C���[�̃X�N���[�����W���v�Z����
                var screenPos = Camera.main.WorldToScreenPoint(transform.position);

                // �v���C���[���猩���}�E�X�J�[�\���̕������v�Z����
                var direction = Input.mousePosition - screenPos;

                // �}�E�X�J�[�\�������݂�������̊p�x���擾����
                var angle = GetAngle(Vector3.zero, direction);

                // �e�̔��ˊp�x���x�N�g���ɕϊ�����
                var direction_angle = GetDirection(angle);

                // ���ˊp�x�Ƒ������瑬�x�����߂�
                var velocity_Dir = -direction_angle * 30f;
                /*
                                // ���@���i�s�����Ɣ��Ε����Ɍ����悤�ɂ���
                                var angles = gameObject.transform.localEulerAngles;
                                angles.z = angle + 90;
                                gameObject.transform.localEulerAngles = angles;*/

                rbody.velocity = velocity_Dir;
            }

        }
    }

    private void PlayerBullet()
    {
        // �v���C���[�̃X�N���[�����W���v�Z����
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // �v���C���[���猩���}�E�X�J�[�\���̕������v�Z����
        var direction = Input.mousePosition - screenPos;

        // �}�E�X�J�[�\�������݂�������̊p�x���擾����
        var angle = GetAngle(Vector3.zero, direction);
/*
        // �v���C���[���}�E�X�J�[�\���̕���������悤�ɂ���
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;*/

        //�e�e�̐���
        GameObject obj = Instantiate(BulletPrefabs,transform.position,Quaternion.Euler(0,0, angle));
        Init(0.35f, obj.GetComponent<PlayerBullet>());
    }
    Vector3 velocity_Dir;
    public void Init(float speed, PlayerBullet obj)
    {
        // �v���C���[�̃X�N���[�����W���v�Z����
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // �v���C���[���猩���}�E�X�J�[�\���̕������v�Z����
        var direction = Input.mousePosition - screenPos;

        // �}�E�X�J�[�\�������݂�������̊p�x���擾����
        var angle = GetAngle(Vector3.zero, direction);

        // �e�̔��ˊp�x���x�N�g���ɕϊ�����
        var direction_angle = GetDirection(angle);

        // ���ˊp�x�Ƒ������瑬�x�����߂�
        obj.velocity_Dir = direction_angle * speed;

        // �e���i�s�����������悤�ɂ���
        var angles = obj.gameObject.transform.localEulerAngles;
        angles.z = angle - 90;
        obj.gameObject.transform.localEulerAngles = angles;

        // 1 �b��ɍ폜����
        Destroy(obj.gameObject, 0.45f);
    }

    // �w�肳�ꂽ 2 �̈ʒu����p�x�����߂ĕԂ�
    public static float GetAngle(Vector2 from, Vector2 to)
    {
        var dx = to.x - from.x;
        var dy = to.y - from.y;
        var rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }
    public static Vector3 GetDirection(float angle)
    {
        return new Vector3
        (
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0
        );
    }
}
