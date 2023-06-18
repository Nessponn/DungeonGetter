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

        //Ç∑ÇËî≤ÇØè∞ÇÃÇ∑ÇËî≤ÇØå¯â Çé¿ëï
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
