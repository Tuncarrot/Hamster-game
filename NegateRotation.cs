using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegateRotation : MonoBehaviour
{
    public GameObject ball;

    private GameObject hamster;
    private Vector3 objPos;

    // Start is called before the first frame update
    void Start()
    {
        objPos = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        // Negate Rotation
        transform.rotation = Quaternion.Euler(0, 0, 0);
        objPos = ball.transform.position;
        objPos.y -= Constants.HamsterInBallAdj;
        transform.position = objPos;
    }
}
