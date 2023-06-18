using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRangeManager : SingletonMonoBehaviourFast<CameraRangeManager>
{
    private Vector2 CameraBottom;
    private Vector2 CameraLimit;
    private Camera Cam;

    // Start is called before the first frame update
    void Start()
    {
        Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの追従
        Cam.transform.position = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y + 1, -10);

        //CameraBottomの適用
        if (Cam.transform.position.x <= CameraBottom.x) Cam.transform.position = new Vector3(CameraBottom.x, Cam.transform.position.y, -10);
        if (Cam.transform.position.y <= CameraBottom.y) Cam.transform.position = new Vector3(Cam.transform.position.x, CameraBottom.y, -10);

        //CameraLimitの適用
        if (Cam.transform.position.x >= CameraLimit.x) Cam.transform.position = new Vector3(CameraLimit.x, Cam.transform.position.y, -10);
        if (Cam.transform.position.y >= CameraLimit.y) Cam.transform.position = new Vector3(Cam.transform.position.x, CameraLimit.y, -10);
    }

    public void CameraRangeSetter(Vector2 Bottom, Vector2 Limit)
    {
        CameraBottom = Bottom;
        CameraLimit = Limit;
    }
}
