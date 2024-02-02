using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackStats : MonoBehaviour
{
    public static int trackPosition = 0;
    public static int trackPositionDeletion = 0;
    public static int switchCountdown = 2;
    public static int transitionCountDown = 0;
    public static int segment = Constants.Beach;
    public static int segmentTransition = Constants.transitionStep_B_C_1;
    public static int spawnCar_Center = 0;
    public static int spawnVehicleExtra_Center = 0;

    public static int health_num_hearts = 0;

    public static bool transition = false;
    public static bool endGameScreen = false;
    public static bool saveGame = false;

    public static Dictionary<int, List<GameObject>> maraudersMap;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        maraudersMap = new Dictionary<int, List<GameObject>>();

        // We set 105 but cycle through and reset at 99
        // Some objects move at different speeds which causes them to delete sooner
        // Initialize next list when you generate row
        for (int i = 0; i < 105; i++)
        {
            maraudersMap[i] = new List<GameObject>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddReference(int position, GameObject gameObj)
    {

    }
}
