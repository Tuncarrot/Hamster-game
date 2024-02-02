using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGoldSelfDestruct : MonoBehaviour
{

    private Player player;

    private void Start()
    {
        player = GameObject.Find("HamsterBall").GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        switch (collider.tag)
        {
            case "ball":
                Debug.Log("MOBILE LOG >>> Got Coin");
                player.gold++;
                Destroy(gameObject);
                break;
        }
    }
}
