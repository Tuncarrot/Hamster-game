using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const float LeftFenceXPos = -1.045f;
    public const float RightFenceXPos = 0.875f;

    public const float YPosGround = 0.276f;
    public const float ZPosGround = -9.2f;

    public const float LeftHouseXPosAdj = -1.5f;
    public const float RightHouseXPosAdj = 1.5f;

    public const float HouseZPosAdj = 1.23f;

    public const float PositionAheadImproved = 5.0f;
    public const float PositionAhead = 20.0f;
    public const float PositionAheadStart = 3.328f;
    public const float PositionAheadCont = 23.328f;
    public const float PositionAheadDouble = 43.328f;

    public const int GenLeftSide = 0;
    public const int GenRightSide = 1;

    public const float HorizontalSpeed = 1.25f;

    public const int numOfBarriers = 12;
    public const int numOfObj = 15;

    public const float animateSlowSpd = 0.1f;

    public const float HamsterInBallAdj = 0.09f;
    public const float CoinYPosAdj = 0.16f;

    // Horizontal Position
    public const int Center = 0;
    public const int Side = 1;
    public const int FarSide = 2;
    
    // Initial num of segments to spawn
    public const int StartSegmentValue = 30;

    // Obj Scales & Weight
    public const float GenericObjRotateYZ = -90.0f;
    public const float GenericObjRotateX = 90.0f;

    public const float TrashBinScale = 0.005f;
    public const float TrashBinWeight = 0.9f;

    public const float DumpsterScale = 0.003f;
    public const float DumpsterWeight = 1.5f;

    public const float TirePileScale = 0.003f;
    public const float TirePileWeight = 1.1f;

    public const float CoinScale = 0.1f;

    // Physics
    public const float BombBlastRadius = 10.0f;
    public const float BombExplosionForce = 1700.0f;

    public const float CenterXPos = 0.085f;

    // Ground Components
    public const string CityGround = "Ground";
    public const string SeaGround = "RoadObj";
    public const string CityRoadGround = "road_straight_mesh";
    public const string BeachSideGround = "Env_Beach_Straight";
    public const string CityBridge = "Env_Car_Bridge_02";
    public const string CityBridgeBeach = "Env_Canal_End";
    public const string Water = "Env_Water_Tile";
    public const string CityIntersectionGround = "road_corner_mesh";
    public const string BeachCornerGround = "Env_Beach_Corner";
    public const string BeachCornerGroundReversed = "Env_Beach_Corner_REVERSE_PLACEHOLDER";
    public const string SeaWallStraight = "Env_Seawall_Straight";
    public const string SeaWallStraightReversed = "Env_Seawall_Straight_REVERSE_PLACEHOLDER";
    public const string CityGroundT = "road_t_mesh";
    public const string CityRoadGroundRotated = "road_straight_mesh";
    public const string GrassSquareGround = "grass_square_mesh";
    public const string CityBasicGround = "road_square_mesh";

    // Environment Components
    public const string EnvironmentRocks_123 = "Env_Rocks_0";
    public const string EnvironmentBeachChair_123 = "Prop_Beachseat_0";
    public const string EnvironmentBeachUmbrella_123 = "Prop_Umbrella_0";

    // Buildings
    public const string BuildingOffice_Base = "Building_Office";
    public const string BuildingStore_Base = "Building_StoreCorner_";
    public const string BuildingShop_Base = "Building_Shop_0";
    public const string BuildingSpecificShop_Base = "Building_";

    // Details
    public const string DetailGarbageBin = "bin_mesh";
    public const string DetailTrash = "trash_mesh";
    public const string DetailHydrant = "hydrant_mesh";
    public const string DetailExitSign = "Prop_Roadsign_02";
    public const string DetailTree1 = "Prop_Tree_01";
    public const string DetailTree2 = "Prop_Tree_02";
    public const string DetailTree3 = "tree_large_mesh";
    public const string DetailTree4 = "tree_medium_mesh";
    public const string DetailMemorial = "memorial_mesh";
    public const string DetailPlanter = "Env_Planter";
    public const string DetailBillBoard = "billboard_mesh";

    // Locations
    public const float CityBridgeX = 10.0f;
    public const float CityBridgeZ = 15.0f;
    
    // Counter Reps
    public const int RoadLeftSide = 1;
    public const int RoadRightSide = 2;

    public const int City = 1;
    public const int Beach = 2;
    public const int Transition = 3;

    // Distance Markers
    public const float DistanceLeftSide = -20.0f;
    public const float DistanceRightSide = 20.0f;
    public const float DistanceRightSideFar = 40.0f;
    public const float DistanceBeach = 20.0f;
    public const int DistanceAhead = 20;

    // Randomization Values
    public const int CityMaxLength = 5;
    public const int CityMinLength = 3;
    public const int BeachMaxLength = 5;
    public const int BeachMinLength = 3;
    public const int TransitionMaxLength = 3;

    // Transition Steps
    public const int transitionStep_B_C_1 = 1;
    public const int transitionStep_B_C_2 = 2;
    public const int transitionStep_B_C_3 = 3;
    public const int transitionStep_B_C_4 = 4;
    public const int NumOfTranSteps_B_C = 4;

    // Transition Steps
    public const int transitionStep_C_B_1 = 5;
    public const int transitionStep_C_B_2 = 6;
    public const int transitionStep_C_B_3 = 7;
    public const int transitionStep_C_B_4 = 8;
    public const int NumOfTranSteps_C_B = 4;

    // SpawningRates
    public const float Percent_50 = 2;
    public const float Percent_33 = 3;
    public const float Percent_25 = 4;

    // Vehicles
    public const string Vehicle_CarBase = "v_car_";
    public const string Vehicle_VanBase = "v_van_seperate_";
    public const string Vehicle_WagonBase = "v_car_seperate_";
    public const string Vehicle_UFO = "UFO_";

    // Stationary
    public const string Vehicle_Stationary = "dumpster_mesh";

    // Internal Interactive Components
    public const string Interactive_coin = "coin";

}


