using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManagement : MonoBehaviour
{
    public Player player;

    public Shop_UpdateText shopUpdate;

    // Start is called before the first frame update
    void Start()
    {
        int energyLevel = player.energyLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyMaxEnergyUpgrade()
    {
        int purse = player.GetGold();
        int energyLevel = player.energyLevel;
        int cost = UpgradeGuide.energyUpgradePath[energyLevel].Cost;

        Debug.Log("PURSE: " + purse.ToString() +
                  " ENERGY LEVEL: " + energyLevel.ToString() +
                  " COST: " + cost.ToString());

        if (purse >= cost)
        {
            player.gold -= cost;
            player.energyLevel++;
            shopUpdate.changeDetected = true;
            player.SavePlayer();
            Debug.Log("Purchased Energy");
        }

        // transaction
        // upgrade player stats
    }

    public void BuyMaxHealthUpgrade()
    {
        int purse = player.GetGold();
        // 0 additional hearts
        int healthLevel = player.health_num_hearts;
        int cost = UpgradeGuide.healthUpgradePath[healthLevel].Cost;

        Debug.Log("PURSE: " + purse.ToString() +
          " HEALTH LEVEL: " + healthLevel.ToString() +
          " COST: " + cost.ToString());

        if (purse >= cost)
        {
            player.gold -= cost;
            player.health_num_hearts++;
            shopUpdate.changeDetected = true;
            player.SavePlayer();
            Debug.Log("Purchased Health");
        }

        // transaction
        // upgrade player stats
    }
}
