using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TouchControl : MonoBehaviour
{
    Vector3 centerOfScreen;
    Vector3 startingPosition;
    Vector3 currentPosition;

    public TMP_Text score;
    public TMP_Text coinCount;
    public TMP_Text DEBUG_SPEED;

    public GameObject hamsterBall;
    public GameObject playerCamera;

    public GameObject heart_icon1;
    public GameObject heart_icon2;
    public GameObject heart_icon3;

    public Player player;
    public Slider slideControl;

    // On phone testing, force 50, sidespeed 25
    private float force = 50f;
    private float sideSpeed = 25f;

    // private bool launched;
    private int coinCounter;
    private int coinCounterPrev;

    private int playerStamina;
    private int processStep;

    private int heart_num;
    private int heart_num_prev;
    private int heart_regen_level;

    private bool arrestoMomento;

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        // Debug.Log("RuntimeMethodLoad: After first Scene loaded");
        // player.LoadPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(Application.persistentDataPath.ToString());
        // player.SavePlayer();
        player.LoadPlayer();
        coinCount.text = player.gold.ToString();

        startingPosition = hamsterBall.transform.position;
        centerOfScreen = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f);
        hamsterBall.GetComponent<Rigidbody>().useGravity = false;
        coinCounter = player.gold;
        coinCounterPrev = player.gold;

        heart_num = TrackStats.health_num_hearts;
        heart_num_prev = TrackStats.health_num_hearts;

        Debug.Log("Heart Number :" + heart_num.ToString());

        heart_regen_level = player.health_regen_level;

        updateHeartUI();

        arrestoMomento = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = hamsterBall.transform.position;
        coinCounter = player.gold;
        playerStamina = player.energyStamina;
        heart_num = TrackStats.health_num_hearts;

        processStep = (int)Vector3.Distance(currentPosition, startingPosition);
        score.text = processStep.ToString();

        // Update coin counter visual only with value increase
        if (coinCounter != coinCounterPrev)
        {
            coinCounterPrev = coinCounter;
            coinCount.text = player.gold.ToString();
        }

        // Update Heart Icon only when value decreases
        if (heart_num != heart_num_prev)
        {
            heart_num_prev = heart_num;
            updateHeartUI();
        }

        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);

            if (player.GetCurrentEnergy() > 0)
            {
                Rigidbody rb = hamsterBall.GetComponent<Rigidbody>();

                rb.useGravity = true;
                //rb.AddForce(Vector3.forward * force);
                rb.velocity = new Vector3(0, 0, force);
                //hamsterBall.transform.Translate(transform.forward * force * Time.deltaTime);
                // Debug.Log("MOBILE LOG >>> Inside Launch VALUE >>>>" + launched);

                // 3.5 to 13.5
                var xPos = hamsterBall.transform.position;

                xPos.x = slideControl.value;

                hamsterBall.transform.position = xPos;
                //hamsterBall.transform.Translate(transform.right * sideSpeed * Time.deltaTime);
                //transform.position += transform.right * sideSpeed * Time.deltaTime;
                }
                DEBUG_SPEED.text = touch.position.x.ToString();

                player.ConsumeEnergy();
        }
        else
        {
            //DEBUG_SPEED.text = "";
        }
    }

    void updateHeartUI()
    {
        switch (heart_num)
        {
            case 1:
                heart_icon1.SetActive(true);
                heart_icon2.SetActive(false);
                heart_icon3.SetActive(false);
                break;
            case 2:
                heart_icon1.SetActive(true);
                heart_icon2.SetActive(true);
                heart_icon3.SetActive(false);
                break;
            case 3:
                heart_icon1.SetActive(true);
                heart_icon2.SetActive(true);
                heart_icon3.SetActive(true);
                break;
        }
    }
}
