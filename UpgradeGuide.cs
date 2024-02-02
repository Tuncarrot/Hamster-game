using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUpgrade
{
    public int Cost { get; set; }
    public int MaxEnergy { get; set; }

    public EnergyUpgrade(int cost, int maxEnergy)
    {
        Cost = cost;
        MaxEnergy = maxEnergy;
    }
}

public class HealthUpgrade
{
    public int Cost { get; set; }
    public int NumHearts { get; set; }

    public HealthUpgrade(int cost, int numHearts)
    {
        Cost = cost;
        NumHearts = numHearts;
    }
}

public static class UpgradeGuide
{
    public static readonly Dictionary<int, EnergyUpgrade> energyUpgradePath = new Dictionary<int, EnergyUpgrade>
    {
        { 1, new EnergyUpgrade(5, 12)},
        { 2, new EnergyUpgrade(10, 14)},
        { 3, new EnergyUpgrade(15, 16)},
        { 4, new EnergyUpgrade(20, 18)},
        { 5, new EnergyUpgrade(25, 20)},
        { 6, new EnergyUpgrade(30, 22)},
        { 7, new EnergyUpgrade(35, 24)},
        { 8, new EnergyUpgrade(40, 26)},
        { 9, new EnergyUpgrade(45, 28)},
        { 10, new EnergyUpgrade(50, 30)},

        { 11, new EnergyUpgrade(75, 35)},
        { 12, new EnergyUpgrade(100, 40)},
        { 13, new EnergyUpgrade(125, 45)},
        { 14, new EnergyUpgrade(150, 50)},
        { 15, new EnergyUpgrade(175, 55)},
        { 16, new EnergyUpgrade(200, 60)},
        { 17, new EnergyUpgrade(225, 65)},
        { 18, new EnergyUpgrade(250, 70)},
        { 19, new EnergyUpgrade(275, 75)},
        { 20, new EnergyUpgrade(300, 80)},

        { 21, new EnergyUpgrade(350, 100)},
        { 22, new EnergyUpgrade(400, 120)},
        { 23, new EnergyUpgrade(450, 140)},
        { 24, new EnergyUpgrade(500, 160)},
        { 25, new EnergyUpgrade(550, 180)},
        { 26, new EnergyUpgrade(600, 200)},
        { 27, new EnergyUpgrade(650, 220)},
        { 28, new EnergyUpgrade(700, 240)},
        { 29, new EnergyUpgrade(750, 260)},
        { 30, new EnergyUpgrade(800, 280)},

        { 31, new EnergyUpgrade(1000, 400)},
    };

    public static readonly Dictionary<int, HealthUpgrade> healthUpgradePath = new Dictionary<int, HealthUpgrade>
    {
        { 0, new HealthUpgrade(100, 1)},
        { 1, new HealthUpgrade(250, 2)},
        { 2, new HealthUpgrade(500, 3)},
        { 3, new HealthUpgrade(500, 3)},
    };
};