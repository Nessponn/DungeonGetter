using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    //public GameObject Bullet;

    private Rigidbody2D rbody;
    public GameObject Explosion;

    [HideInInspector]public Vector3 velocity_Dir;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += velocity_Dir;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layerName = LayerMask.LayerToName(col.gameObject.layer);

        if(layerName == "Floor")
        {
            Instantiate(Explosion, transform.localPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
