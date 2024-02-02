using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private Canvas UIComponent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ball")
        {
            // Check hearts before ending

            if (TrackStats.health_num_hearts <= 0)
            {
                //End game
                // Debug.Log("GAME OVER");
                TrackStats.endGameScreen = true;
                TrackStats.saveGame = true;
                Time.timeScale = 0;
            }
            else
            {
                Debug.Log("~~~DAMAGE TAKEN~~~");
                TrackStats.health_num_hearts--;
            }
        }
    }
}
