using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Player : MonoBehaviour
{
    public int gold = 0;

    public int energyLevel = 1;
    public int energyStamina = 1;
    public int energyMax = 10;
    public int energy;

    public EnergyBar energyBar;

    public int staminaBoost;
    public int heartAmount = 1;

    protected float Timer;

    // Shop stats   
    public int gold_coin_amount;
    public int gold_spawn_rate;

    public bool energy_unlock;
    public int energy_coin_amount;
    public int energy_spawn_rate;

    public bool health_unlock;
    public int health_num_hearts;
    public int health_regen_level;

    public bool special_unlock;
    public bool unlock_boom_coin;
    public bool unlock_x2_coin;

    private void Start()
    {
        energyMax = UpgradeGuide.energyUpgradePath[energyLevel].MaxEnergy;

        energy = energyMax;
        energyBar.SetMaxEnergy(energy); 
        staminaBoost = 1;
    }

    public void CreatePlayer()
    {
        SaveSystem.CreatePlayer(this);
        Debug.Log("MOBILE LOG >>> Checking if Player Exists");
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        Debug.Log("MOBILE LOG >>> SAVING PLAYER DATA TO FILE...");
    }

    public void LoadPlayer()
    {
        PlayerStats data = SaveSystem.LoadPlayer();
        Debug.Log("LOADING PLAYER DATA");

        gold = data.gold;
        energyLevel = data.energyLevel;
        staminaBoost = data.staminaBoost;
        heartAmount = data.healthAmount;

        // For Updating UI
        health_num_hearts = data.health_num_hearts;

        // For keeping track of health
        TrackStats.health_num_hearts = data.health_num_hearts;

        Debug.Log("MOBILE LOG >>>  LOADING PLAYER DATA FROM FILE");
        Debug.Log("MOBILE LOG >>>  -- GOLD " + gold.ToString());
        Debug.Log("MOBILE LOG >>>  -- ENERGY LEVEL " + energyLevel.ToString());
        Debug.Log("MOBILE LOG >>>  -- STAMINA BOOST " + staminaBoost.ToString());
        Debug.Log("MOBILE LOG >>>  -- HEART AMOUNT " + heartAmount.ToString());

        energyMax = UpgradeGuide.energyUpgradePath[energyLevel].MaxEnergy;
    }

    public void ConsumeEnergy()
    {
        Timer += Time.deltaTime;

        if (Timer >= energyStamina)
        {
            Timer = 0f;
            energy--;
            energyBar.SetEnergy(energy);
        }
    }

    public void GrabEnergyCoin()
    {
        energy += staminaBoost;

        if (energy >= energyMax)
        {
            energy = energyMax;
        }

        Debug.Log(staminaBoost.ToString() + " stamina");
    }

    public int GetCurrentEnergy()
    {
        return energy;
    }

    public int GetGold()
    {
        return gold;
    }
}
