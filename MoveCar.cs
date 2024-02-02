using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCar : MonoBehaviour
{

    GameObject carObj = new GameObject();

    public bool move = false;
    private bool timerStart = false;
    private float speed;
    private float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        carObj = transform.GetChild(0).gameObject;
        timeLeft = Random.Range(0,2);
        speed = Random.Range(30, 40);
        // Debug.Log("NAME CHILD " + carObj.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStart)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                // Move vehicle
                move = true;
            }

            if (move)
            {
                carObj.transform.Translate(Vector3.back * speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ball" && move == false)
        {
            // Start Timer
            timerStart = true;
        }
    }
}
