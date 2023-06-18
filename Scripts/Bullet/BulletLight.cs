using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLight : MonoBehaviour
{
    HardLight2D Lightning;

    // Start is called before the first frame update
    void Start()
    {
        Lightning = GetComponent<HardLight2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Lightning.Intensity -= 0.1f;
        if (Lightning.Intensity <= 0) Destroy(gameObject.transform.root.gameObject);
    }
}
