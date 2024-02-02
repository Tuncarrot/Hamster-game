using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public int gold;
    public int energyLevel;
    public int staminaBoost;
    public int healthAmount;

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


    public PlayerStats(Player player)
    {
        gold = player.gold;
        staminaBoost = player.staminaBoost;
        energyLevel = player.energyLevel;
        healthAmount = player.heartAmount;

        // Shop Stuff
        gold_coin_amount = player.gold_coin_amount;
        gold_spawn_rate = player.gold_spawn_rate;

        energy_unlock = player.energy_unlock;
        energy_coin_amount = player.energy_coin_amount;
        energy_spawn_rate = player.energy_spawn_rate;

        health_unlock = player.health_unlock;
        health_num_hearts = player.health_num_hearts;
        health_regen_level = player.health_regen_level;

        special_unlock = player.special_unlock;
        unlock_boom_coin = player.unlock_boom_coin;
        unlock_x2_coin = player.unlock_x2_coin;
    }
}
