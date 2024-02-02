using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Vector3 myPos;
    public Transform myObj;

    public float xRotation;
    public float yRotation;
    public float zRotation;

    Vector3 newRotation;

    // Start is called before the first frame update
    void Start()
    {
        newRotation = new Vector3(xRotation, yRotation, zRotation);
    }

    // Update is called once per frame
    void Update()
    {
        newRotation = new Vector3(xRotation, yRotation, zRotation);
        transform.position = myObj.position + myPos;
        transform.eulerAngles = newRotation;
    }
}
