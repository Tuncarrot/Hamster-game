using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCoins : MonoBehaviour
{
    GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        obj = this.gameObject;       
    }

    // Update is called once per frame
    void Update()
    {
        obj.transform.Rotate(Vector3.up * 150.0f * Time.deltaTime);
    }
}
