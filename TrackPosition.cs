using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPosition : MonoBehaviour
{
    public GameObject hamsterBall;

    private Animator animatorObj;
    private Rigidbody rigBody;

    // Start is called before the first frame update
    void Start()
    {
        rigBody = hamsterBall.GetComponent<Rigidbody>();
        animatorObj = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigBody.velocity != Vector3.zero)
        {
            animatorObj.SetBool("isMoving", true);
        }
        else
        {
            animatorObj.SetBool("isMoving", false);
        }
    }
}
