using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportLeft : MonoBehaviour
{
    //自分の情報
    public Room room;　　　//部屋情報
    public GameObject Warppoint;//自分のゲート

    //移動先の情報
    [HideInInspector] public TeleportRight Warp;//担当するワープ先のTeleportDown
    [HideInInspector] public GameObject SignPoint;

    private void Awake()
    {
        SignPoint = Instantiate(room.SignPoint_Hori);
        SignPoint.transform.position = new Vector2(this.transform.position.x + 1.5f, this.transform.position.y);
        SignPoint.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<BoxCollider2D>().isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        string layerName = LayerMask.LayerToName(col.gameObject.layer);
        GameObject Player = col.gameObject.transform.root.gameObject;

        if (layerName == "Player")
        {
            if (Player.GetComponent<Rigidbody2D>().velocity.x <= 0f)
            {
                //プレイヤーを移動させる
                Player.transform.position = Warp.Warppoint.transform.position;

                //カメラの追従値を変更する
                CameraRangeManager.Instance.CameraRangeSetter(Warp.room.CameraBottom, Warp.room.CameraLimit);
            }
        }

        /*if (col.gameObject.CompareTag("Player"))
        {
            //左へ移動中であれば
            if (col.gameObject.GetComponent<Rigidbody2D>().velocity.x <= 0f)
            {
                //プレイヤーを移動させる
                col.gameObject.transform.position = Warp.Warppoint.transform.position;

                //カメラの追従値を変更する
                CameraRangeManager.Instance.CameraRangeSetter(Warp.room.CameraBottom, Warp.room.CameraLimit);
            }
        }*/
    }
    //☓印を消す
    //判定もコライダーからトリガーにして、プレイヤーが通れるように
    public void UnLock()
    {
        SignPoint.SetActive(false);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
