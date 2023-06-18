using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAllower : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        string layerName = LayerMask.LayerToName(col.gameObject.layer);

        if (layerName == "Floor" || layerName == "FloatFloor")
        {
            Player.Instance.Jump = true;
        }

        //���蔲�����̂��蔲�����ʂ�����
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        string layerName = LayerMask.LayerToName(col.gameObject.layer);

        if (layerName == "Floor" || layerName == "FloatFloor")
        {
            Player.Instance.Jump = false;
        }
    }
}
