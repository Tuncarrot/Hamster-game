using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScreen : MonoBehaviour
{
    public GameObject UIScreen;
    public Player player;

    private void Start()
    {
        TrackStats.endGameScreen = false;
        UIScreen.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (TrackStats.endGameScreen && TrackStats.saveGame)
        {
            UIScreen.SetActive(true);
            TrackStats.trackPosition = 0;
            TrackStats.trackPositionDeletion = 0;
            TrackStats.saveGame = false;
            player.SavePlayer();
            Debug.Log("TRIGGERED END GAME");
        }
    }
}
