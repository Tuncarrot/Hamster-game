using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop_UpdateText : MonoBehaviour
{
    public TMP_Text energyCount;

    public Player player;

    public TMP_Text Energy_UpgradeCostText;
    public TMP_Text Energy_LevelText;

    public TMP_Text Health_UpgradeCostText;
    public TMP_Text Health_LevelText;

    public GameObject Energy_StarLv1Filled;
    public GameObject Energy_StarLv2Filled;
    public GameObject Energy_StarLv3Filled;

    public GameObject Health_StarLv1Filled;
    public GameObject Health_StarLv2Filled;
    public GameObject Health_StarLv3Filled;

    public TMP_Text coinCount_MainMenu;
    public TMP_Text coinCount_CoinMenu;
    public TMP_Text coinCount_EnergyMenu;
    public TMP_Text coinCount_HealthMenu;
    public TMP_Text coinCount_UserMenu;

    public bool changeDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScreen();
        UpdateEnergyLevelsIcon();
        UpdateHealthLevelsIcon();
    }

    // Update is called once per frame
    void Update()
    {
        if (changeDetected)
        {
            UpdateScreen();

            changeDetected = false;
        }
    }

    void UpdateScreen()
    {
        int energyLevel = player.energyLevel;

        Debug.Log(energyLevel.ToString() + " ENERGY LEVEL");

        player.LoadPlayer();
        energyCount.text = player.energyMax.ToString();

        UpdateEnergyLevelsIcon();
        UpdateHealthLevelsIcon();
        UpdateCoinCounter();
    }

    void UpdateEnergyLevelsIcon()
    {
        int energyLevel = player.energyLevel;

        Energy_UpgradeCostText.text = UpgradeGuide.energyUpgradePath[energyLevel].Cost.ToString();
        Energy_LevelText.text = "Lv " + energyLevel.ToString();

        if (energyLevel > 9)
        {
            Energy_StarLv1Filled.SetActive(true);
        }

        if (energyLevel > 19)
        {
            Energy_StarLv2Filled.SetActive(true);
        }

        if (energyLevel > 29)
        {
            Energy_StarLv3Filled.SetActive(true);
        }
    }

    void UpdateHealthLevelsIcon()
    {
        int healthLevel = player.health_num_hearts;

        // Level 0 is no hearts, so +1
        Health_UpgradeCostText.text = UpgradeGuide.healthUpgradePath[healthLevel].Cost.ToString();
        Health_LevelText.text = "Lv " + healthLevel.ToString();

        if (healthLevel == 1)
        {
            Health_StarLv1Filled.SetActive(true);
        }

        if (healthLevel == 2)
        {
            Health_StarLv2Filled.SetActive(true);
        }

        if (healthLevel == 3)
        {
            Health_StarLv3Filled.SetActive(true);
        }
    }

    void UpdateCoinCounter()
    {
        player.LoadPlayer();
        string coinCount = player.gold.ToString();

        coinCount_MainMenu.text =
        coinCount_CoinMenu.text =
        coinCount_EnergyMenu.text =
        coinCount_HealthMenu.text =
        coinCount_UserMenu.text = coinCount;
    }
}
