using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveCarCenterPattern : MonoBehaviour
{
    float speed;
    float oscSpeed;

    int oscCounter;

    Animator driftAnim;

    public int oscPattern;

    void Start()
    {
        oscCounter = 0;
        oscSpeed = 5.0f;

        speed = UnityEngine.Random.Range(30, 40);

        driftAnim = GetComponentInChildren<Animator>();

        if (driftAnim == null)
        {
            Console.WriteLine("DriftAnim is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (oscPattern)
        {
            case 0:
                if (oscCounter > 0 && oscCounter < 240)
                {
                    transform.Translate(Vector3.right * oscSpeed * Time.deltaTime);
                }
                else if (oscCounter > 240 && oscCounter < 480)
                {
                    transform.Translate(Vector3.left * oscSpeed * Time.deltaTime);
                }
                break;
            case 1:
                // Travel Straight on the left
                break;
            case 2:
                // Drift
                driftAnim.enabled = true;

                Vector3 relPos = transform.position;
                relPos.x = 0.0f; // was 15 
                transform.position = relPos;
                break;
            default:
                // Do nothing, patten will be set shortly after initilziation
                break;
        }

        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (oscCounter > 480)
        {
            oscCounter = 0;
        }

        oscCounter++;
    }

    public void SetOscPattern(int oscCount)
    {
        oscPattern = oscCount;
    }

}
