using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //部屋番号
    //public int RoomNumber;

    //抽選判断真偽変数
    private bool RoomLock = false;

    //部屋のゲート
    public TeleportLeft Left;
    public TeleportRight Right;
    public TeleportUp Up;
    public TeleportDown Down;

    [HideInInspector]public Vector2 CameraBottom;//エリアのカメラワーク制御
    public Vector2 CameraLimit;//エリアのカメラワーク制御


    public GameObject SignPoint_Vert;//通れないことを示す☓印
    public GameObject SignPoint_Hori;

    // Start is called before the first frame update
    void Start()
    {
        CameraBottom = transform.position;
    }

    //一度決められた部屋が二度抽選されないようにロックする
    public void RoomLocker()
    {
        RoomLock = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
