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
        //左入力
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (Jump) Speed -= 0.12f;
            else Speed -= 0.02f;
            if (Speed <= -MaxSpeed) Speed = -MaxSpeed;

            //rbody.velocity = new Vector2(Speed, rbody.velocity.y);
            if(rbody.velocity.x >= -MaxSpeed) rbody.AddForce(new Vector2(-3, 0));
        }

        //右入力
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (Jump) Speed += 0.12f;
            else Speed += 0.02f;

            if (Speed >= MaxSpeed) Speed = MaxSpeed;

            //rbody.velocity = new Vector2(Speed, rbody.velocity.y);
            if (rbody.velocity.x <= MaxSpeed) rbody.AddForce(new Vector2(3, 0));
        }

        //左右どちらも入力しない
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

                //弾を打った方向とは逆に
                // プレイヤーのスクリーン座標を計算する
                var screenPos = Camera.main.WorldToScreenPoint(transform.position);

                // プレイヤーから見たマウスカーソルの方向を計算する
                var direction = Input.mousePosition - screenPos;

                // マウスカーソルが存在する方向の角度を取得する
                var angle = GetAngle(Vector3.zero, direction);

                // 弾の発射角度をベクトルに変換する
                var direction_angle = GetDirection(angle);

                // 発射角度と速さから速度を求める
                var velocity_Dir = -direction_angle * 30f;
                /*
                                // 自機が進行方向と反対方向に向くようにする
                                var angles = gameObject.transform.localEulerAngles;
                                angles.z = angle + 90;
                                gameObject.transform.localEulerAngles = angles;*/

                rbody.velocity = velocity_Dir;
            }

        }
    }

    private void PlayerBullet()
    {
        // プレイヤーのスクリーン座標を計算する
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // プレイヤーから見たマウスカーソルの方向を計算する
        var direction = Input.mousePosition - screenPos;

        // マウスカーソルが存在する方向の角度を取得する
        var angle = GetAngle(Vector3.zero, direction);
/*
        // プレイヤーがマウスカーソルの方向を見るようにする
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;*/

        //銃弾の生成
        GameObject obj = Instantiate(BulletPrefabs,transform.position,Quaternion.Euler(0,0, angle));
        Init(0.35f, obj.GetComponent<PlayerBullet>());
    }
    Vector3 velocity_Dir;
    public void Init(float speed, PlayerBullet obj)
    {
        // プレイヤーのスクリーン座標を計算する
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // プレイヤーから見たマウスカーソルの方向を計算する
        var direction = Input.mousePosition - screenPos;

        // マウスカーソルが存在する方向の角度を取得する
        var angle = GetAngle(Vector3.zero, direction);

        // 弾の発射角度をベクトルに変換する
        var direction_angle = GetDirection(angle);

        // 発射角度と速さから速度を求める
        obj.velocity_Dir = direction_angle * speed;

        // 弾が進行方向を向くようにする
        var angles = obj.gameObject.transform.localEulerAngles;
        angles.z = angle - 90;
        obj.gameObject.transform.localEulerAngles = angles;

        // 1 秒後に削除する
        Destroy(obj.gameObject, 0.45f);
    }

    // 指定された 2 つの位置から角度を求めて返す
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
