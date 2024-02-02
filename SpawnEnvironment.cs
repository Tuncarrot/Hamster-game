using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnvironment : MonoBehaviour
{
    public const int numOfBarriers = Constants.numOfBarriers;
    public const int numOfObj = Constants.numOfObj;

    private GameObject newGroundLeft;
    private GameObject newGroundRight;

    private GameObject newSideLeft;
    private GameObject newSideRight;

    public int houseCheck = 0;
    public int intObjCheck = 0;

    Transform currentObj;

    public bool spawnGround = false;
    public bool initializeStart = false;

    public Material transparentPlane;

    string[] sections = new string[2];

    // Start is called before the first frame update
    void Start()
    {
        // groundPrefab = (GameObject)Resources.Load("/Ground");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("ON TRIGGER ENTER TRIGGERED");
        switch (collision.tag)
        {
            case "initialize":
                if (!initializeStart)
                {
                    initializeStart = true;

                    for (int i = 0; i < Constants.DistanceAhead * Constants.StartSegmentValue; i += Constants.DistanceAhead)
                    {
                        sections = determineGroundSection();

                        generateCoreComponent(i, sections);
                        generateEnvironmentComponent(i, sections);
                        generateDetailComponent(i, sections);

                        // Things that can kill you
                        generateMovingComponent(i, sections, false);
                        generateMovingComponentExtra(i, sections, false);
                        generateStationaryComponent(i, sections, false);

                        generateInternalComponent(i, sections);

                        resetSegmentValues();
                        TrackStats.trackPosition++;
                        // Debug.Log(" TrackStats.trackPosition Initialize " + TrackStats.trackPosition.ToString());
                    }

                    generateFlatPlane();

                    collision.tag = "ball";
                }
                else
                {
                    collision.tag = "ball";
                }
                break;
            // Get the ball rolling
            case "ball":
                if (!spawnGround)
                {
                    // Debug.Log("GENERATE GROUND");

                    int spawningPosition = TrackStats.trackPosition * Constants.DistanceAhead;

                    sections = determineGroundSection();

                    generateCoreComponent(spawningPosition, sections);
                    generateEnvironmentComponent(spawningPosition, sections);
                    generateDetailComponent(spawningPosition, sections);

                    // Things that can kill you
                    generateMovingComponent(spawningPosition, sections, true);
                    generateMovingComponentExtra(spawningPosition, sections, true);
                    generateStationaryComponent(spawningPosition, sections, true);

                    //generateInternalComponent(Constants.PositionAheadStart, i);
                    //generateInternalComponent(Constants.PositionAheadDouble, TrackStats.trackPosition);

                    resetSegmentValues();
                    spawnGround = true;
                    TrackStats.trackPosition++;
                    // Debug.Log(" TrackStats.trackPosition Continue " + TrackStats.trackPosition.ToString());
                }
                break;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        switch (collision.tag)
        {
            case "ball":
                if (spawnGround)
                {
                    // Debug.Log("DELETE GROUND");
                    deleteGround();
                    spawnGround = false;
                }
                break;
        }
    }

    public void generateInternalComponent(int trackPos, string[] sections)
    {
        string resource = "";
        string component = "";

        Vector3 objScale = new Vector3();

        float objPos = 0.0f; ;
        float objWeight = 0.0f;

        component = sections[0];

        resource = determineInternalInteractive();

        #region Center
        switch (component)
        {
            case Constants.CityRoadGround:
                objPos = Random.Range(1.5f, 18.5f);

                GameObject internal_cityRoadGround_boost = new GameObject();
                internal_cityRoadGround_boost = Instantiate((GameObject)Resources.Load(resource), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                internal_cityRoadGround_boost.SetActive(false);
                
                internal_cityRoadGround_boost.transform.Rotate(0.0f, 0.0f, 0.0f);
                internal_cityRoadGround_boost.transform.position = new Vector3(objPos, 1.5f, trackPos);

                Rigidbody groundRigidBody = internal_cityRoadGround_boost.AddComponent<Rigidbody>();
                groundRigidBody.useGravity = false;
                groundRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBody.isKinematic = true;
                
                switch (resource)
                {
                    case Constants.Interactive_coin + "Value1":
                        internal_cityRoadGround_boost.AddComponent<CoinGoldSelfDestruct>();
                        break;
                    case Constants.Interactive_coin + "Energy1":
                        internal_cityRoadGround_boost.AddComponent<energyCoin>();
                        break;
                }
                internal_cityRoadGround_boost.AddComponent<rotateCoins>();

                BoxCollider boxCol = internal_cityRoadGround_boost.AddComponent<BoxCollider>();
                boxCol.isTrigger = true;
                
                internal_cityRoadGround_boost.tag = "genGround";
                internal_cityRoadGround_boost.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(internal_cityRoadGround_boost);
                break;
            case Constants.CityIntersectionGround:

                break;
            case Constants.CityBridgeBeach:

                break;
        }
        #endregion

        component = sections[1];

        #region Side
        switch (component)
        {
            case Constants.CityRoadGround:

                break;
        }
        #endregion

        #region Far Side
        switch (component)
        {
        }
        #endregion

    }

    public Vector3 generateXCoordinates(float xStartPosition, float xEndPosition, int zAdj, float posAhead, float optYAdj = 0.0f)
    {
        Vector3 objPosition = new Vector3(Random.Range(xStartPosition, xEndPosition), Constants.YPosGround + optYAdj, Constants.ZPosGround + (zAdj * 1.67f) + transform.position.z + posAhead);

        return objPosition;
    }

    public void generateDetailComponent(int trackPos, string[] sections)
    {
        string component;
        string[] toSpawn = { "", "" };

        component = sections[0];

        #region Center Spawning
        // CENTER SPAWNING
        switch (component)
        {
            case Constants.CityRoadGround:

                toSpawn = determineDetails(component);

                for (int i=0;i<2;i++)
                {
                    if (toSpawn[i] != "")
                    {
                        GameObject detail_cityRoadGround_Center = new GameObject();
                        detail_cityRoadGround_Center = Instantiate((GameObject)Resources.Load(toSpawn[i]), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        detail_cityRoadGround_Center.SetActive(false);

                        detail_cityRoadGround_Center.transform.Rotate(0.0f, 0.0f, 0.0f);
                        detail_cityRoadGround_Center.transform.position = new Vector3(1.5f + i * 17.0f, 0.0f, trackPos + 5.0f);

                        Rigidbody groundRigidBody = detail_cityRoadGround_Center.AddComponent<Rigidbody>();
                        groundRigidBody.useGravity = false;
                        groundRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBody.isKinematic = true;

                        detail_cityRoadGround_Center.AddComponent<MeshCollider>();

                        detail_cityRoadGround_Center.tag = "genGround";
                        detail_cityRoadGround_Center.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(detail_cityRoadGround_Center);
                    }
                }
                break;
        }
        #endregion

        component = sections[1];

        #region Outer Spawning
        switch (component)
        {
            case Constants.GrassSquareGround:
                string[] park_Components = { "", "", "", "", "" };
                park_Components = determineDetailsComplexObj(component);
                Vector3 position = new Vector3();

                for (int i=0;i<2;i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                // Memorial
                                position = new Vector3(30.0f + i * -40.0f, 0.0f, trackPos + 5.0f);
                                break;
                            case 1:
                                // Planter Left
                                position = new Vector3(36.0f + i * -40.0f, 0.0f, trackPos + 5.0f); ;
                                break;
                            case 2:
                                // Planter Right
                                position = new Vector3(24.0f + i * -40.0f, 0.0f, trackPos + 5.0f); ;
                                break;
                            case 3:
                                // Tree Upper
                                position = new Vector3(23.0f + i * -40.0f, 0.0f, trackPos + 12.0f); ;
                                break;
                            case 4:
                                // Tree Lower 
                                position = new Vector3(33.0f + i * -40.0f, 0.0f, trackPos - 1.0f); ;
                                break;
                            case 5:
                                // Garbage
                                position = new Vector3(23.0f + i * -40.0f, 0.0f, trackPos - 3.0f);
                                break;
                        }

                        GameObject detail_parkComponent_Side = new GameObject();
                        detail_parkComponent_Side = Instantiate((GameObject)Resources.Load(park_Components[j]), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        detail_parkComponent_Side.SetActive(false);

                        detail_parkComponent_Side.transform.Rotate(0.0f, 0.0f, 0.0f);
                        detail_parkComponent_Side.transform.position = position;

                        Rigidbody groundRigidBody = detail_parkComponent_Side.AddComponent<Rigidbody>();
                        groundRigidBody.useGravity = false;
                        groundRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBody.isKinematic = true;

                        detail_parkComponent_Side.AddComponent<MeshCollider>();

                        detail_parkComponent_Side.tag = "genGround";
                        detail_parkComponent_Side.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(detail_parkComponent_Side);
                    }
                }
                break;
        }
        #endregion

        component = sections[2];

        #region Far Spawning
        switch (component)
        {
            case Constants.GrassSquareGround:
                string[] park_Components = { "", "", "", "", "" };
                park_Components = determineDetailsComplexObj(component);
                Vector3 position = new Vector3();

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                // Memorial
                                position = new Vector3(51.0f + i * -80.0f, 0.0f, trackPos + 5.0f);
                                break;
                            case 1:
                                // Planter Left
                                position = new Vector3(45.0f + i * -80.0f, 0.0f, trackPos + 5.0f); ;
                                break;
                            case 2:
                                // Planter Right
                                position = new Vector3(57.0f + i * -80.0f, 0.0f, trackPos + 5.0f); ;
                                break;
                            case 3:
                                // Tree Upper
                                position = new Vector3(43.0f + i * -80.0f, 0.0f, trackPos + 12.0f); ;
                                break;
                            case 4:
                                // Tree Lower 
                                position = new Vector3(53.0f + i * -80.0f, 0.0f, trackPos - 1.0f); ;
                                break;
                            case 5:
                                // Garbage
                                position = new Vector3(43.0f + i * -80.0f, 0.0f, trackPos - 3.0f);
                                break;
                        }

                        GameObject detail_parkComponent_Side = new GameObject();
                        detail_parkComponent_Side = Instantiate((GameObject)Resources.Load(park_Components[j]), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        detail_parkComponent_Side.SetActive(false);

                        detail_parkComponent_Side.transform.Rotate(0.0f, 0.0f, 0.0f);
                        detail_parkComponent_Side.transform.position = position;

                        Rigidbody groundRigidBody = detail_parkComponent_Side.AddComponent<Rigidbody>();
                        groundRigidBody.useGravity = false;
                        groundRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBody.isKinematic = true;

                        detail_parkComponent_Side.AddComponent<MeshCollider>();

                        detail_parkComponent_Side.tag = "genGround";
                        detail_parkComponent_Side.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(detail_parkComponent_Side);
                    }
                }
                break;
        }
        #endregion
    }
    public void generateEnvironmentComponent(int trackPos, string[] sections)
    {
        string component;

        #region Environment Center Spawning
        // CENTER SPAWNING
/*      component = sections[0];
        switch (component)
        {
            case Constants.CityRoadGround:
                for (int i = 0; i < 4; i++)
                {
                    newGroundLeft = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    newGroundLeft.SetActive(false);

                    newGroundLeft.transform.Rotate(0.0f, -90.0f, 0.0f);
                    newGroundLeft.transform.position = new Vector3(0.0f, 0.0f, i * Constants.PositionAheadImproved + trackPos);

                    Rigidbody groundRigidBodyLeft = newGroundLeft.AddComponent<Rigidbody>();
                    groundRigidBodyLeft.useGravity = false;
                    groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyLeft.isKinematic = true;

                    MeshCollider groundMeshColliderLeft = newGroundLeft.AddComponent<MeshCollider>();

                    newGroundLeft.AddComponent<SpawnEnvironment>();
                    newGroundLeft.tag = "genGround";
                    newGroundLeft.SetActive(true);

                    newGroundRight = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    newGroundRight.SetActive(false);

                    newGroundRight.transform.Rotate(0.0f, 90.0f, 0.0f);
                    newGroundRight.transform.position = new Vector3(20.0f, 0.0f, i * Constants.PositionAheadImproved - Constants.PositionAheadImproved + trackPos);

                    Rigidbody groundRigidBodyRight = newGroundRight.AddComponent<Rigidbody>();
                    groundRigidBodyRight.useGravity = false;
                    groundRigidBodyRight.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyRight.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyRight.isKinematic = true;

                    MeshCollider groundMeshColliderRight = newGroundRight.AddComponent<MeshCollider>();

                    newGroundRight.AddComponent<SpawnEnvironment>();
                    newGroundRight.tag = "genGround";
                    newGroundRight.SetActive(true);
                }
                break;
            case Constants.CityBridge:
                newGroundLeft = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                newGroundLeft.SetActive(false);

                newGroundLeft.transform.Rotate(0.0f, -90.0f, 0.0f);
                newGroundLeft.transform.position = new Vector3(Constants.CityBridgeX, 0.0f, Constants.CityBridgeZ + trackPos);

                Rigidbody groundRigidBody = newGroundLeft.AddComponent<Rigidbody>();
                groundRigidBody.useGravity = false;
                groundRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBody.isKinematic = true;

                MeshCollider groundMeshCollider = newGroundLeft.AddComponent<MeshCollider>();

                newGroundLeft.AddComponent<SpawnEnvironment>();
                newGroundLeft.tag = "genGround";
                newGroundLeft.SetActive(true);
                break;
            case Constants.CityIntersectionGround:
                for (int i = 0; i < 2; i++)
                {
                    newGroundLeft = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    newGroundLeft.SetActive(false);

                    newGroundLeft.transform.Rotate(0.0f, 180.0f + i * -270.0f, 0.0f);
                    newGroundLeft.transform.position = new Vector3(0.0f, 0.0f, trackPos - 5.0f + (i * 20.0f));
                    newGroundLeft.AddComponent<MeshCollider>();

                    Rigidbody groundRigidBody1 = newGroundLeft.AddComponent<Rigidbody>();
                    groundRigidBody1.useGravity = false;
                    groundRigidBody1.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBody1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBody1.isKinematic = true;
                    newGroundLeft.SetActive(true);

                    newGroundRight = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    newGroundRight.SetActive(false);

                    newGroundRight.transform.Rotate(0.0f, -270.0f + (i * 270.0f), 0.0f);
                    newGroundRight.transform.position = new Vector3(Constants.DistanceRightSide, 0.0f, trackPos - 5.0f + (i * 20.0f));

                    Rigidbody groundRigidBody2 = newGroundRight.AddComponent<Rigidbody>();
                    groundRigidBody2.useGravity = false;
                    groundRigidBody2.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBody2.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBody2.isKinematic = true;
                    newGroundRight.AddComponent<MeshCollider>();

                    newGroundRight.SetActive(true);
                }
                break;
        }*/
        #endregion

        #region Environment Side Spawning
        // OUTER SIDE SPAWNING
        component = sections[1];
        switch (component)
        {
            case Constants.BeachSideGround:

                string[] envPiece = determineEnvironmentSection(component);

                for (int i = 0; i < 2; i++)
                {
                    if (envPiece[i] != "")
                    {
                        GameObject env_BeachSideGround_Left = new GameObject();
                        env_BeachSideGround_Left = Instantiate((GameObject)Resources.Load(envPiece[i]), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        env_BeachSideGround_Left.SetActive(false);
                        
                        env_BeachSideGround_Left.transform.Rotate(0.0f, 90.0f + i * 180.0f, 0.0f);
                        env_BeachSideGround_Left.transform.position = new Vector3(-10.0f + i * 40.0f, -1.0f, trackPos + 5.0f);
                        env_BeachSideGround_Left.AddComponent<MeshCollider>();

                        Rigidbody groundRigidBodyLeft = env_BeachSideGround_Left.AddComponent<Rigidbody>();
                        groundRigidBodyLeft.useGravity = false;
                        groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyLeft.isKinematic = true;

                        env_BeachSideGround_Left.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(env_BeachSideGround_Left);
                    }
                }
                break;
            case Constants.CityGroundT:
            case Constants.CityBasicGround:
                string[] envPiece1 = determineEnvironmentSection(component);

                for (int i = 0; i < 2; i++)
                {
                    if (envPiece1[i] != "")
                    {
                        
                        GameObject env_CityGroundT_Left = new GameObject();
                        env_CityGroundT_Left = Instantiate((GameObject)Resources.Load(envPiece1[i]), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        env_CityGroundT_Left.SetActive(false);
                        
                        env_CityGroundT_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                        env_CityGroundT_Left.transform.position = new Vector3(-10.0f + i * 40.0f, -1.0f, trackPos + 5.0f);
                        env_CityGroundT_Left.AddComponent<MeshCollider>();

                        Rigidbody groundRigidBodyLeft = env_CityGroundT_Left.AddComponent<Rigidbody>();
                        groundRigidBodyLeft.useGravity = false;
                        groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyLeft.isKinematic = true;

                        env_CityGroundT_Left.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(env_CityGroundT_Left);
                    }
                }
                break;
        }
                #endregion

        #region Environment Far Side Spawning
        // FAR OUTER SIDE SPAWNING
        component = sections[2];
        switch (component)
        {
            case Constants.Water:

                // Spawn on water tile
                string[] envPiece = determineEnvironmentSection(component);

                for (int i =0; i<2;i++)
                {
                    if (envPiece[i] != "")
                    {
                        GameObject env_water_Left = new GameObject();
                        env_water_Left = Instantiate((GameObject)Resources.Load(envPiece[i]), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        env_water_Left.SetActive(false);
                        
                        env_water_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                        env_water_Left.transform.position = new Vector3(-30.0f + i * 80.0f, -3.0f, trackPos + 5.0f);
                        env_water_Left.AddComponent<MeshCollider>();

                        Rigidbody groundRigidBodyLeft = env_water_Left.AddComponent<Rigidbody>();
                        groundRigidBodyLeft.useGravity = false;
                        groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyLeft.isKinematic = true;

                        env_water_Left.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(env_water_Left);
                    }
                }
                break;
            case Constants.CityGroundT:
            case Constants.CityBasicGround:
                string[] envPiece1 = determineEnvironmentSection(component);

                for (int i = 0; i < 2; i++)
                {
                    if (envPiece1[i] != "")
                    {
                        GameObject env_CityGroundT_Left = new GameObject();
                        env_CityGroundT_Left = Instantiate((GameObject)Resources.Load(envPiece1[i]), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        env_CityGroundT_Left.SetActive(false);
                        
                        env_CityGroundT_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                        env_CityGroundT_Left.transform.position = new Vector3(-30.0f + i * 80.0f, -1.0f, trackPos + 5.0f);
                        env_CityGroundT_Left.AddComponent<MeshCollider>();

                        Rigidbody groundRigidBodyLeft = env_CityGroundT_Left.AddComponent<Rigidbody>();
                        groundRigidBodyLeft.useGravity = false;
                        groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyLeft.isKinematic = true;

                        env_CityGroundT_Left.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(env_CityGroundT_Left);
                    }
                }
                break;
/*            case Constants.SeaWallStraight:
                for (int i = 0; i < 4; i++)
                {
                    newGroundLeft = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    newGroundLeft.SetActive(false);

                    newGroundLeft.transform.Rotate(0.0f, 0.0f, 0.0f);
                    newGroundLeft.transform.position = new Vector3(i * Constants.PositionAheadImproved - 35.0f, 0.0f, trackPos + 15.0f);

                    Rigidbody groundRigidBodyLeft1 = newGroundLeft.AddComponent<Rigidbody>();
                    groundRigidBodyLeft1.useGravity = false;
                    groundRigidBodyLeft1.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyLeft1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyLeft1.isKinematic = true;

                    newGroundLeft.AddComponent<MeshCollider>();

                    newGroundLeft.AddComponent<SpawnEnvironment>();
                    newGroundLeft.tag = "genGround";
                    newGroundLeft.SetActive(true);

                    newGroundRight = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    newGroundRight.SetActive(false);

                    newGroundRight.transform.Rotate(0.0f, 00.0f, 0.0f);
                    newGroundRight.transform.position = new Vector3(i * Constants.PositionAheadImproved + 45.0f, 0.0f, trackPos + Constants.DistanceRightSide - 5.0f);

                    Rigidbody groundRigidBodyRight1 = newGroundRight.AddComponent<Rigidbody>();
                    groundRigidBodyRight1.useGravity = false;
                    groundRigidBodyRight1.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyRight1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyRight1.isKinematic = true;

                    newGroundRight.AddComponent<MeshCollider>();

                    newGroundRight.AddComponent<SpawnEnvironment>();
                    newGroundRight.tag = "genGround";
                    newGroundRight.SetActive(true);
                }
                break;
            case Constants.SeaWallStraightReversed:
                for (int i = 0; i < 4; i++)
                {
                    newGroundLeft = Instantiate((GameObject)Resources.Load(Constants.SeaWallStraight), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    newGroundLeft.SetActive(false);

                    newGroundLeft.transform.Rotate(0.0f, 180.0f, 0.0f);
                    newGroundLeft.transform.position = new Vector3(i * Constants.PositionAheadImproved - 40.0f, 0.0f, trackPos - 5.0f);

                    Rigidbody groundRigidBodyLeft2 = newGroundLeft.AddComponent<Rigidbody>();
                    groundRigidBodyLeft2.useGravity = false;
                    groundRigidBodyLeft2.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyLeft2.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyLeft2.isKinematic = true;

                    newGroundLeft.AddComponent<MeshCollider>();

                    newGroundLeft.AddComponent<SpawnEnvironment>();
                    newGroundLeft.tag = "genGround";
                    newGroundLeft.SetActive(true);

                    newGroundRight = Instantiate((GameObject)Resources.Load(Constants.SeaWallStraight), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    newGroundRight.SetActive(false);

                    newGroundRight.transform.Rotate(0.0f, 180.0f, 0.0f);
                    newGroundRight.transform.position = new Vector3(i * Constants.PositionAheadImproved + 40.0f, 0.0f, trackPos - 5.0f);

                    Rigidbody groundRigidBodyRight2 = newGroundRight.AddComponent<Rigidbody>();
                    groundRigidBodyRight2.useGravity = false;
                    groundRigidBodyRight2.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyRight2.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyRight2.isKinematic = true;

                    newGroundRight.AddComponent<MeshCollider>();

                    newGroundRight.AddComponent<SpawnEnvironment>();
                    newGroundRight.tag = "genGround";
                    newGroundRight.SetActive(true);
                }
                break;
            case Constants.CityRoadGroundRotated:
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        newGroundLeft = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        newGroundLeft.SetActive(false);

                        newGroundLeft.transform.Rotate(0.0f, 0.0f, 0.0f);
                        newGroundLeft.transform.position = new Vector3(j * Constants.PositionAheadImproved - 35.0f + i * 80.0f, 0.0f, trackPos + 15.0f); ;

                        Rigidbody groundRigidBodyLeft1 = newGroundLeft.AddComponent<Rigidbody>();
                        groundRigidBodyLeft1.useGravity = false;
                        groundRigidBodyLeft1.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyLeft1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyLeft1.isKinematic = true;

                        newGroundLeft.AddComponent<MeshCollider>();

                        newGroundLeft.AddComponent<SpawnEnvironment>();
                        newGroundLeft.tag = "genGround";
                        newGroundLeft.SetActive(true);

                        newGroundRight = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        newGroundRight.SetActive(false);

                        newGroundRight.transform.Rotate(0.0f, 180.0f, 0.0f);
                        newGroundRight.transform.position = new Vector3(j * Constants.PositionAheadImproved - 40.0f + i * 80.0f, 0.0f, trackPos - 5.0f); ;

                        Rigidbody groundRigidBodyRight1 = newGroundRight.AddComponent<Rigidbody>();
                        groundRigidBodyRight1.useGravity = false;
                        groundRigidBodyRight1.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyRight1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyRight1.isKinematic = true;

                        newGroundRight.AddComponent<MeshCollider>();

                        newGroundRight.AddComponent<SpawnEnvironment>();
                        newGroundRight.tag = "genGround";
                        newGroundRight.SetActive(true);
                    }
                }
                break;*/
        }
        #endregion
    }
    public void generateCoreComponent(int trackPos, string[] sections, bool generateColliders = true)
    {
        string component = sections[0];

        // CENTER SPAWNING
        switch (component)
        {
            case Constants.CityRoadGround:
                for (int i = 0; i < 4; i++)
                {
                    GameObject center_cityRoadGround_Left = new GameObject();
                    center_cityRoadGround_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    center_cityRoadGround_Left.SetActive(false);

                    center_cityRoadGround_Left.transform.Rotate(0.0f, -90.0f, 0.0f);
                    center_cityRoadGround_Left.transform.position = new Vector3(0.0f, 0.0f, i * Constants.PositionAheadImproved + trackPos);

                    Rigidbody groundRigidBodyLeft = center_cityRoadGround_Left.AddComponent<Rigidbody>();
                    groundRigidBodyLeft.useGravity = false;
                    groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyLeft.isKinematic = true;

                    center_cityRoadGround_Left.AddComponent<MeshCollider>();

                    center_cityRoadGround_Left.tag = "genGround";
                    center_cityRoadGround_Left.SetActive(true);

                    // Add reference for deletion
                    // Debug.Log("Storing " + TrackStats.trackPosition.ToString());
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(center_cityRoadGround_Left);

                    GameObject center_cityRoadGround_Right = new GameObject();

                    center_cityRoadGround_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    center_cityRoadGround_Right.SetActive(false);

                    center_cityRoadGround_Right.transform.Rotate(0.0f, 90.0f, 0.0f);
                    center_cityRoadGround_Right.transform.position = new Vector3(20.0f, 0.0f, i * Constants.PositionAheadImproved - Constants.PositionAheadImproved + trackPos);

                    Rigidbody groundRigidBodyRight = center_cityRoadGround_Right.AddComponent<Rigidbody>();
                    groundRigidBodyRight.useGravity = false;
                    groundRigidBodyRight.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyRight.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyRight.isKinematic = true;

                    center_cityRoadGround_Right.AddComponent<MeshCollider>();

                    center_cityRoadGround_Right.tag = "genGround";
                    center_cityRoadGround_Right.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(center_cityRoadGround_Right);

                    if (i == 3)
                    {
                        BoxCollider boxCol2 = center_cityRoadGround_Left.AddComponent<BoxCollider>();
                        boxCol2.center = new Vector3(-1.0f, 7.0f, 0.0f);
                        boxCol2.size = new Vector3(2.0f, 25.0f, 100.0f);
                        boxCol2.isTrigger = true;

                        center_cityRoadGround_Left.AddComponent<SpawnEnvironment>();
                    }
                }
                break;
            case Constants.CityBridge:
                GameObject center_cityBridge = new GameObject();
                center_cityBridge = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                center_cityBridge.SetActive(false);

                center_cityBridge.transform.Rotate(0.0f, -90.0f, 0.0f);
                center_cityBridge.transform.position = new Vector3(Constants.CityBridgeX, 0.0f, Constants.CityBridgeZ + trackPos);

                Rigidbody groundRigidBody = center_cityBridge.AddComponent<Rigidbody>();
                groundRigidBody.useGravity = false;
                groundRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBody.isKinematic = true;

                center_cityBridge.AddComponent<MeshCollider>();

                BoxCollider boxCol = center_cityBridge.AddComponent<BoxCollider>();
                boxCol.center = new Vector3(-1.0f, 7.0f, 0.0f);
                boxCol.size = new Vector3(2.0f, 25.0f, 100.0f);
                boxCol.isTrigger = true;

                center_cityBridge.AddComponent<SpawnEnvironment>();
                center_cityBridge.tag = "genGround";
                center_cityBridge.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(center_cityBridge);

                break;
            case Constants.CityIntersectionGround:
                for (int i=0;i<2;i++)
                {
                    GameObject center_cityIntersectionGround_Left = new GameObject();
                    center_cityIntersectionGround_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    center_cityIntersectionGround_Left.SetActive(false);

                    center_cityIntersectionGround_Left.transform.Rotate(0.0f, 180.0f + i * -270.0f, 0.0f);
                    center_cityIntersectionGround_Left.transform.position = new Vector3(0.0f, 0.0f, trackPos - 5.0f + (i * 20.0f));
                    center_cityIntersectionGround_Left.AddComponent<MeshCollider>();

                    Rigidbody groundRigidBody1 = center_cityIntersectionGround_Left.AddComponent<Rigidbody>();
                    groundRigidBody1.useGravity = false;
                    groundRigidBody1.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBody1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBody1.isKinematic = true;
                    center_cityIntersectionGround_Left.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(center_cityIntersectionGround_Left);

                    GameObject center_cityIntersectionGround_Right = new GameObject();
                    center_cityIntersectionGround_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    center_cityIntersectionGround_Right.SetActive(false);

                    center_cityIntersectionGround_Right.transform.Rotate(0.0f, -270.0f + (i * 270.0f), 0.0f);
                    center_cityIntersectionGround_Right.transform.position = new Vector3(Constants.DistanceRightSide, 0.0f, trackPos - 5.0f + (i * 20.0f));

                    Rigidbody groundRigidBody2 = center_cityIntersectionGround_Right.AddComponent<Rigidbody>();
                    groundRigidBody2.useGravity = false;
                    groundRigidBody2.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBody2.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBody2.isKinematic = true;
                    center_cityIntersectionGround_Right.AddComponent<MeshCollider>();

                    center_cityIntersectionGround_Right.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(center_cityIntersectionGround_Right);

                    if (i == 1)
                    {
                        BoxCollider boxCol1 = center_cityIntersectionGround_Left.AddComponent<BoxCollider>();
                        boxCol1.center = new Vector3(-1.0f, 7.0f, 0.0f);
                        boxCol1.size = new Vector3(2.0f, 25.0f, 100.0f);
                        boxCol1.isTrigger = true;
                        center_cityIntersectionGround_Left.AddComponent<SpawnEnvironment>();
                    }
                }
                break;
        }

        component = sections[1];

        // OUTER SIDE SPAWNING
        switch (component)
        {
            case Constants.BeachSideGround:
                GameObject outer_beachSideGround_Left = new GameObject();
                outer_beachSideGround_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                outer_beachSideGround_Left.SetActive(false);

                outer_beachSideGround_Left.transform.Rotate(0.0f, 90.0f, 0.0f);
                outer_beachSideGround_Left.transform.position = new Vector3(0.0f, 0.0f, trackPos - Constants.PositionAheadImproved);
                outer_beachSideGround_Left.AddComponent<MeshCollider>();

                Rigidbody groundRigidBodyLeft8 = outer_beachSideGround_Left.AddComponent<Rigidbody>();
                groundRigidBodyLeft8.useGravity = false;
                groundRigidBodyLeft8.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyLeft8.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyLeft8.isKinematic = true;

                outer_beachSideGround_Left.AddComponent<SpawnEnvironment>();
                outer_beachSideGround_Left.tag = "genGround";
                outer_beachSideGround_Left.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_beachSideGround_Left);

                GameObject outer_beachSideGround_Right = new GameObject();
                outer_beachSideGround_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                outer_beachSideGround_Right.SetActive(false);

                outer_beachSideGround_Right.transform.Rotate(0.0f, -90.0f, 0.0f);
                outer_beachSideGround_Right.transform.position = new Vector3(Constants.DistanceRightSide, 0.0f, trackPos + Constants.DistanceAhead - Constants.PositionAheadImproved);
                outer_beachSideGround_Right.AddComponent<MeshCollider>();

                Rigidbody groundRigidBodyRight8 = outer_beachSideGround_Right.AddComponent<Rigidbody>();
                groundRigidBodyRight8.useGravity = false;
                groundRigidBodyRight8.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyRight8.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyRight8.isKinematic = true;

                outer_beachSideGround_Right.AddComponent<SpawnEnvironment>();
                outer_beachSideGround_Right.tag = "genGround";
                outer_beachSideGround_Right.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_beachSideGround_Right);
                break;
            case Constants.CityBridgeBeach:
                GameObject outer_CityBridgeBeach_Left = new GameObject();
                outer_CityBridgeBeach_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                outer_CityBridgeBeach_Left.SetActive(false);

                outer_CityBridgeBeach_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                outer_CityBridgeBeach_Left.transform.position = new Vector3(0.0f, 0.0f, trackPos - Constants.PositionAheadImproved + Constants.PositionAhead); ;
                outer_CityBridgeBeach_Left.AddComponent<MeshCollider>();

                Rigidbody groundRigidBodyLeft7 = outer_CityBridgeBeach_Left.AddComponent<Rigidbody>();
                groundRigidBodyLeft7.useGravity = false;
                groundRigidBodyLeft7.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyLeft7.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyLeft7.isKinematic = true;

                outer_CityBridgeBeach_Left.AddComponent<SpawnEnvironment>();
                outer_CityBridgeBeach_Left.tag = "genGround";
                outer_CityBridgeBeach_Left.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_CityBridgeBeach_Left);

                GameObject outer_CityBridgeBeach_Right = new GameObject();
                outer_CityBridgeBeach_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                outer_CityBridgeBeach_Right.SetActive(false);

                outer_CityBridgeBeach_Right.transform.Rotate(0.0f, -180.0f, 0.0f);
                outer_CityBridgeBeach_Right.transform.position = new Vector3(Constants.DistanceRightSide, 0.0f, trackPos - Constants.PositionAheadImproved);
                outer_CityBridgeBeach_Right.AddComponent<MeshCollider>();

                Rigidbody groundRigidBodyRight7 = outer_CityBridgeBeach_Right.AddComponent<Rigidbody>();
                groundRigidBodyRight7.useGravity = false;
                groundRigidBodyRight7.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyRight7.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyRight7.isKinematic = true;

                outer_CityBridgeBeach_Right.AddComponent<SpawnEnvironment>();
                outer_CityBridgeBeach_Right.tag = "genGround";
                outer_CityBridgeBeach_Right.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_CityBridgeBeach_Right);
                break;
            case Constants.BeachCornerGround:
                GameObject out_BeachCornerGround_Left = new GameObject();
                out_BeachCornerGround_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                out_BeachCornerGround_Left.SetActive(false);

                out_BeachCornerGround_Left.transform.Rotate(0.0f, -270.0f, 0.0f);
                out_BeachCornerGround_Left.transform.position = new Vector3(0.0f, 0.0f, trackPos - Constants.PositionAheadImproved); ; ;
                out_BeachCornerGround_Left.AddComponent<MeshCollider>();

                Rigidbody groundRigidBodyLeft6 = out_BeachCornerGround_Left.AddComponent<Rigidbody>();
                groundRigidBodyLeft6.useGravity = false;
                groundRigidBodyLeft6.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyLeft6.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyLeft6.isKinematic = true;

                out_BeachCornerGround_Left.AddComponent<SpawnEnvironment>();
                out_BeachCornerGround_Left.tag = "genGround";
                out_BeachCornerGround_Left.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(out_BeachCornerGround_Left);

                GameObject out_BeachCornerGround_Right = new GameObject();
                out_BeachCornerGround_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                out_BeachCornerGround_Right.SetActive(false);

                out_BeachCornerGround_Right.transform.Rotate(0.0f, -180.0f, 0.0f);
                out_BeachCornerGround_Right.transform.position = new Vector3(Constants.DistanceRightSide, 0.0f, trackPos - Constants.PositionAheadImproved);
                out_BeachCornerGround_Right.AddComponent<MeshCollider>();

                Rigidbody groundRigidBodyRight6 = out_BeachCornerGround_Right.AddComponent<Rigidbody>();
                groundRigidBodyRight6.useGravity = false;
                groundRigidBodyRight6.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyRight6.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyRight6.isKinematic = true;

                out_BeachCornerGround_Right.AddComponent<SpawnEnvironment>();
                out_BeachCornerGround_Right.tag = "genGround";
                out_BeachCornerGround_Right.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(out_BeachCornerGround_Right);
                break;
            case Constants.SeaWallStraight:
                for (int i = 0; i < 4; i++)
                {
                    GameObject outer_SeaWallStraight_Left = new GameObject();
                    outer_SeaWallStraight_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    outer_SeaWallStraight_Left.SetActive(false);

                    outer_SeaWallStraight_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                    outer_SeaWallStraight_Left.transform.position = new Vector3(i * Constants.PositionAheadImproved - 15.0f, 0.0f, trackPos + 15.0f);

                    Rigidbody groundRigidBodyLeft5 = outer_SeaWallStraight_Left.AddComponent<Rigidbody>();
                    groundRigidBodyLeft5.useGravity = false;
                    groundRigidBodyLeft5.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyLeft5.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyLeft5.isKinematic = true;

                    outer_SeaWallStraight_Left.AddComponent<MeshCollider>();

                    outer_SeaWallStraight_Left.AddComponent<SpawnEnvironment>();
                    outer_SeaWallStraight_Left.tag = "genGround";
                    outer_SeaWallStraight_Left.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_SeaWallStraight_Left);

                    GameObject outer_SeaWallStraight_Right = new GameObject();
                    outer_SeaWallStraight_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    outer_SeaWallStraight_Right.SetActive(false);

                    outer_SeaWallStraight_Right.transform.Rotate(0.0f, 00.0f, 0.0f);
                    outer_SeaWallStraight_Right.transform.position = new Vector3(i * Constants.PositionAheadImproved + 25.0f, 0.0f, trackPos + Constants.DistanceRightSide - 5.0f);

                    Rigidbody groundRigidBodyRight5 = outer_SeaWallStraight_Right.AddComponent<Rigidbody>();
                    groundRigidBodyRight5.useGravity = false;
                    groundRigidBodyRight5.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyRight5.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyRight5.isKinematic = true;

                    outer_SeaWallStraight_Right.AddComponent<MeshCollider>();

                    outer_SeaWallStraight_Right.AddComponent<SpawnEnvironment>();
                    outer_SeaWallStraight_Right.tag = "genGround";
                    outer_SeaWallStraight_Right.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_SeaWallStraight_Right);
                }
                break;
            case Constants.BeachCornerGroundReversed:
                GameObject outer_BeachCornerGroundReversed_Left = new GameObject();
                outer_BeachCornerGroundReversed_Left = Instantiate((GameObject)Resources.Load(Constants.BeachCornerGround), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                outer_BeachCornerGroundReversed_Left.SetActive(false);
                
                outer_BeachCornerGroundReversed_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                outer_BeachCornerGroundReversed_Left.transform.position = new Vector3(0.0f, 0.0f, trackPos - Constants.PositionAheadImproved + Constants.DistanceAhead);
                outer_BeachCornerGroundReversed_Left.AddComponent<MeshCollider>();

                Rigidbody groundRigidBodyLeft = outer_BeachCornerGroundReversed_Left.AddComponent<Rigidbody>();
                groundRigidBodyLeft.useGravity = false;
                groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyLeft.isKinematic = true;

                outer_BeachCornerGroundReversed_Left.AddComponent<SpawnEnvironment>();
                outer_BeachCornerGroundReversed_Left.tag = "genGround";
                outer_BeachCornerGroundReversed_Left.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_BeachCornerGroundReversed_Left);

                GameObject outer_BeachCornerGroundReversed_Right = new GameObject();
                outer_BeachCornerGroundReversed_Right = Instantiate((GameObject)Resources.Load(Constants.BeachCornerGround), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                outer_BeachCornerGroundReversed_Right.SetActive(false);
                
                outer_BeachCornerGroundReversed_Right.transform.Rotate(0.0f, -90.0f, 0.0f);
                outer_BeachCornerGroundReversed_Right.transform.position = new Vector3(Constants.DistanceRightSide, 0.0f, trackPos - Constants.PositionAheadImproved + Constants.DistanceAhead);
                outer_BeachCornerGroundReversed_Right.AddComponent<MeshCollider>();

                Rigidbody groundRigidBodyRight = outer_BeachCornerGroundReversed_Right.AddComponent<Rigidbody>();
                groundRigidBodyRight.useGravity = false;
                groundRigidBodyRight.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyRight.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyRight.isKinematic = true;

                outer_BeachCornerGroundReversed_Right.AddComponent<SpawnEnvironment>();
                outer_BeachCornerGroundReversed_Right.tag = "genGround";
                outer_BeachCornerGroundReversed_Right.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_BeachCornerGroundReversed_Right);
                break;
            case Constants.SeaWallStraightReversed:
                for (int i = 0; i < 4; i++)
                {
                    GameObject outer_SeaWallStraightReversed_Left = new GameObject();
                    outer_SeaWallStraightReversed_Left = Instantiate((GameObject)Resources.Load(Constants.SeaWallStraight), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    outer_SeaWallStraightReversed_Left.SetActive(false);

                    outer_SeaWallStraightReversed_Left.transform.Rotate(0.0f, 180.0f, 0.0f);
                    outer_SeaWallStraightReversed_Left.transform.position = new Vector3(i * Constants.PositionAheadImproved - Constants.DistanceAhead, 0.0f, trackPos - 5.0f);

                    Rigidbody groundRigidBodyLeft3 = outer_SeaWallStraightReversed_Left.AddComponent<Rigidbody>();
                    groundRigidBodyLeft3.useGravity = false;
                    groundRigidBodyLeft3.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyLeft3.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyLeft3.isKinematic = true;

                    outer_SeaWallStraightReversed_Left.AddComponent<MeshCollider>();
                    
                    outer_SeaWallStraightReversed_Left.AddComponent<SpawnEnvironment>();
                    outer_SeaWallStraightReversed_Left.tag = "genGround";
                    outer_SeaWallStraightReversed_Left.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_SeaWallStraightReversed_Left);

                    GameObject outer_SeaWallStraightReversed_Right = new GameObject();
                    outer_SeaWallStraightReversed_Right = Instantiate((GameObject)Resources.Load(Constants.SeaWallStraight), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    outer_SeaWallStraightReversed_Right.SetActive(false);
                    
                    outer_SeaWallStraightReversed_Right.transform.Rotate(0.0f, 180.0f, 0.0f);
                    outer_SeaWallStraightReversed_Right.transform.position = new Vector3(i * Constants.PositionAheadImproved + Constants.DistanceAhead, 0.0f, trackPos - 5.0f);

                    Rigidbody groundRigidBodyRight3 = outer_SeaWallStraightReversed_Right.AddComponent<Rigidbody>();
                    groundRigidBodyRight3.useGravity = false;
                    groundRigidBodyRight3.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyRight3.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyRight3.isKinematic = true;

                    outer_SeaWallStraightReversed_Right.AddComponent<MeshCollider>();
                    
                    outer_SeaWallStraightReversed_Right.AddComponent<SpawnEnvironment>();
                    outer_SeaWallStraightReversed_Right.tag = "genGround";
                    outer_SeaWallStraightReversed_Right.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_SeaWallStraightReversed_Right);
                }
                break;
            case Constants.CityGroundT:
                GameObject outer_CityGroundT_Left = new GameObject();
                outer_CityGroundT_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                outer_CityGroundT_Left.SetActive(false);

                outer_CityGroundT_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                outer_CityGroundT_Left.transform.position = new Vector3(Constants.DistanceLeftSide + 10.0f, 0.0f, trackPos - Constants.PositionAheadImproved + Constants.PositionAhead); ;
                outer_CityGroundT_Left.AddComponent<MeshCollider>();

                Rigidbody groundRigidBodyLeft4 = outer_CityGroundT_Left.AddComponent<Rigidbody>();
                groundRigidBodyLeft4.useGravity = false;
                groundRigidBodyLeft4.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyLeft4.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyLeft4.isKinematic = true;

                outer_CityGroundT_Left.AddComponent<SpawnEnvironment>();
                outer_CityGroundT_Left.tag = "genGround";
                outer_CityGroundT_Left.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_CityGroundT_Left);

                GameObject outer_CityGroundT_Right = new GameObject();
                outer_CityGroundT_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                outer_CityGroundT_Right.SetActive(false);
                
                outer_CityGroundT_Right.transform.Rotate(0.0f, -180.0f, 0.0f);
                outer_CityGroundT_Right.transform.position = new Vector3(Constants.DistanceRightSideFar - 10.0f, 0.0f, trackPos - Constants.PositionAheadImproved);
                outer_CityGroundT_Right.AddComponent<MeshCollider>();

                Rigidbody groundRigidBodyRight4 = outer_CityGroundT_Right.AddComponent<Rigidbody>();
                groundRigidBodyRight4.useGravity = false;
                groundRigidBodyRight4.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyRight4.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyRight4.isKinematic = true;

                outer_CityGroundT_Right.AddComponent<SpawnEnvironment>();
                outer_CityGroundT_Right.tag = "genGround";
                outer_CityGroundT_Right.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_CityGroundT_Right);
                break;
            case Constants.CityRoadGroundRotated:
                for (int i=0;i<2;i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        GameObject outer_CityRoadGroundRotated_Left = new GameObject();
                        outer_CityRoadGroundRotated_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        outer_CityRoadGroundRotated_Left.SetActive(false);
                        
                        outer_CityRoadGroundRotated_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                        outer_CityRoadGroundRotated_Left.transform.position = new Vector3(j * Constants.PositionAheadImproved - 15.0f + i * 40.0f, 0.0f, trackPos + 15.0f); ;

                        Rigidbody groundRigidBodyLeft1 = outer_CityRoadGroundRotated_Left.AddComponent<Rigidbody>();
                        groundRigidBodyLeft1.useGravity = false;
                        groundRigidBodyLeft1.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyLeft1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyLeft1.isKinematic = true;

                        outer_CityRoadGroundRotated_Left.AddComponent<MeshCollider>();
                        
                        outer_CityRoadGroundRotated_Left.AddComponent<SpawnEnvironment>();
                        outer_CityRoadGroundRotated_Left.tag = "genGround";
                        outer_CityRoadGroundRotated_Left.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_CityRoadGroundRotated_Left);

                        GameObject outer_CityRoadGroundRotated_Right = new GameObject();
                        outer_CityRoadGroundRotated_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        outer_CityRoadGroundRotated_Right.SetActive(false);
                        
                        outer_CityRoadGroundRotated_Right.transform.Rotate(0.0f, 180.0f, 0.0f);
                        outer_CityRoadGroundRotated_Right.transform.position = new Vector3(j * Constants.PositionAheadImproved - 20.0f + i * 40.0f, 0.0f, trackPos - 5.0f); ;

                        Rigidbody groundRigidBodyRight1 = outer_CityRoadGroundRotated_Right.AddComponent<Rigidbody>();
                        groundRigidBodyRight1.useGravity = false;
                        groundRigidBodyRight1.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyRight1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyRight1.isKinematic = true;

                        outer_CityRoadGroundRotated_Right.AddComponent<MeshCollider>();
                        
                        outer_CityRoadGroundRotated_Right.AddComponent<SpawnEnvironment>();
                        outer_CityRoadGroundRotated_Right.tag = "genGround";
                        outer_CityRoadGroundRotated_Right.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_CityRoadGroundRotated_Right);
                    }
                }
                break;
            case Constants.GrassSquareGround:
            case Constants.CityBasicGround:
                for (int i = 0; i < 2; i++)
                {
                    GameObject outer_GrassSquare_Ground = new GameObject();
                    outer_GrassSquare_Ground = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    outer_GrassSquare_Ground.SetActive(false);

                    outer_GrassSquare_Ground.transform.Rotate(0.0f, 0.0f, 0.0f);
                    outer_GrassSquare_Ground.transform.position = new Vector3(i * 40.0f, 0.0f, trackPos + 15.0f);
                    outer_GrassSquare_Ground.transform.localScale = new Vector3(4.0f, 1.0f, 4.0f);
                    outer_GrassSquare_Ground.AddComponent<MeshCollider>();

                    Rigidbody groundRigidBodyLeft1 = outer_GrassSquare_Ground.AddComponent<Rigidbody>();
                    groundRigidBodyLeft1.useGravity = false;
                    groundRigidBodyLeft1.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyLeft1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyLeft1.isKinematic = true;

                    outer_GrassSquare_Ground.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_GrassSquare_Ground);
                }
                break;
        }

        component = sections[2];

        // FAR OUTER SIDE SPAWNING
        switch (component)
        {
            case Constants.Water:
                GameObject far_Water_Left = new GameObject();
                far_Water_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                far_Water_Left.SetActive(false);
                
                far_Water_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                far_Water_Left.transform.position = new Vector3(Constants.DistanceLeftSide, 0.0f, trackPos - Constants.PositionAheadImproved + Constants.PositionAhead); ;
                
                far_Water_Left.AddComponent<SpawnEnvironment>();
                far_Water_Left.tag = "genGround";
                far_Water_Left.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(far_Water_Left);

                GameObject far_Water_Right = new GameObject();
                far_Water_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                far_Water_Right.SetActive(false);
                
                far_Water_Right.transform.Rotate(0.0f, -180.0f, 0.0f);
                far_Water_Right.transform.position = new Vector3(Constants.DistanceRightSideFar, 0.0f, trackPos - Constants.PositionAheadImproved);
                
                far_Water_Right.AddComponent<SpawnEnvironment>();
                far_Water_Right.tag = "genGround";
                far_Water_Right.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(far_Water_Right);
                break;
            case Constants.CityGroundT:
                GameObject far_CityGroundT_Left = new GameObject();
                far_CityGroundT_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                far_CityGroundT_Left.SetActive(false);
                
                far_CityGroundT_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                far_CityGroundT_Left.transform.position = new Vector3(-30.0f, 0.0f, trackPos - Constants.PositionAheadImproved + Constants.PositionAhead); ;
                far_CityGroundT_Left.AddComponent<MeshCollider>();
                
                far_CityGroundT_Left.AddComponent<SpawnEnvironment>();
                far_CityGroundT_Left.tag = "genGround";
                far_CityGroundT_Left.SetActive(true);
                
                Rigidbody groundRigidBodyLeft = far_CityGroundT_Left.AddComponent<Rigidbody>();
                groundRigidBodyLeft.useGravity = false;
                groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyLeft.isKinematic = true;

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(far_CityGroundT_Left);

                GameObject far_CityGroundT_Right = new GameObject();
                far_CityGroundT_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                far_CityGroundT_Right.SetActive(false);
                
                far_CityGroundT_Right.transform.Rotate(0.0f, -180.0f, 0.0f);
                far_CityGroundT_Right.transform.position = new Vector3(Constants.DistanceRightSideFar + 10.0f, 0.0f, trackPos - Constants.PositionAheadImproved);
                far_CityGroundT_Right.AddComponent<MeshCollider>();

                Rigidbody groundRigidBodyRight = far_CityGroundT_Right.AddComponent<Rigidbody>();
                groundRigidBodyRight.useGravity = false;
                groundRigidBodyRight.constraints = RigidbodyConstraints.FreezeAll;
                groundRigidBodyRight.collisionDetectionMode = CollisionDetectionMode.Continuous;
                groundRigidBodyRight.isKinematic = true;

                far_CityGroundT_Right.AddComponent<SpawnEnvironment>();
                far_CityGroundT_Right.tag = "genGround";
                far_CityGroundT_Right.SetActive(true);

                // Add reference for deletion
                TrackStats.maraudersMap[TrackStats.trackPosition].Add(far_CityGroundT_Right);
                break;
            case Constants.SeaWallStraight:
                for (int i = 0; i < 4; i++)
                {
                    GameObject far_SeaWallStraight_Left = new GameObject();
                    far_SeaWallStraight_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    far_SeaWallStraight_Left.SetActive(false);
                    
                    far_SeaWallStraight_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                    far_SeaWallStraight_Left.transform.position = new Vector3(i * Constants.PositionAheadImproved - 35.0f, 0.0f, trackPos + 15.0f);

                    Rigidbody groundRigidBodyLeft1 = far_SeaWallStraight_Left.AddComponent<Rigidbody>();
                    groundRigidBodyLeft1.useGravity = false;
                    groundRigidBodyLeft1.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyLeft1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyLeft1.isKinematic = true;

                    far_SeaWallStraight_Left.AddComponent<MeshCollider>();

                    far_SeaWallStraight_Left.AddComponent<SpawnEnvironment>();
                    far_SeaWallStraight_Left.tag = "genGround";
                    far_SeaWallStraight_Left.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(far_SeaWallStraight_Left);

                    GameObject far_SeaWallStraight_Right = new GameObject();
                    far_SeaWallStraight_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    far_SeaWallStraight_Right.SetActive(false);
                    
                    far_SeaWallStraight_Right.transform.Rotate(0.0f, 00.0f, 0.0f);
                    far_SeaWallStraight_Right.transform.position = new Vector3(i * Constants.PositionAheadImproved + 45.0f, 0.0f, trackPos + Constants.DistanceRightSide - 5.0f);

                    Rigidbody groundRigidBodyRight1 = far_SeaWallStraight_Right.AddComponent<Rigidbody>();
                    groundRigidBodyRight1.useGravity = false;
                    groundRigidBodyRight1.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyRight1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyRight1.isKinematic = true;

                    far_SeaWallStraight_Right.AddComponent<MeshCollider>();
                    
                    far_SeaWallStraight_Right.AddComponent<SpawnEnvironment>();
                    far_SeaWallStraight_Right.tag = "genGround";
                    far_SeaWallStraight_Right.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(far_SeaWallStraight_Right);
                }
                break;
                case Constants.SeaWallStraightReversed:
                for (int i = 0; i < 4; i++)
                {
                    GameObject far_SeaWallStraightReversed_Left = new GameObject();
                    far_SeaWallStraightReversed_Left = Instantiate((GameObject)Resources.Load(Constants.SeaWallStraight), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    far_SeaWallStraightReversed_Left.SetActive(false);
                    
                    far_SeaWallStraightReversed_Left.transform.Rotate(0.0f, 180.0f, 0.0f);
                    far_SeaWallStraightReversed_Left.transform.position = new Vector3(i * Constants.PositionAheadImproved - 40.0f, 0.0f, trackPos - 5.0f);

                    Rigidbody groundRigidBodyLeft2 = far_SeaWallStraightReversed_Left.AddComponent<Rigidbody>();
                    groundRigidBodyLeft2.useGravity = false;
                    groundRigidBodyLeft2.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyLeft2.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyLeft2.isKinematic = true;

                    far_SeaWallStraightReversed_Left.AddComponent<MeshCollider>();
                    
                    far_SeaWallStraightReversed_Left.AddComponent<SpawnEnvironment>();
                    far_SeaWallStraightReversed_Left.tag = "genGround";
                    far_SeaWallStraightReversed_Left.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(far_SeaWallStraightReversed_Left);

                    GameObject far_SeaWallStraightReversed_Right = new GameObject();
                    far_SeaWallStraightReversed_Right = Instantiate((GameObject)Resources.Load(Constants.SeaWallStraight), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                    far_SeaWallStraightReversed_Right.SetActive(false);
                    
                    far_SeaWallStraightReversed_Right.transform.Rotate(0.0f, 180.0f, 0.0f);
                    far_SeaWallStraightReversed_Right.transform.position = new Vector3(i * Constants.PositionAheadImproved + 40.0f, 0.0f, trackPos - 5.0f);

                    Rigidbody groundRigidBodyRight2 = far_SeaWallStraightReversed_Right.AddComponent<Rigidbody>();
                    groundRigidBodyRight2.useGravity = false;
                    groundRigidBodyRight2.constraints = RigidbodyConstraints.FreezeAll;
                    groundRigidBodyRight2.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    groundRigidBodyRight2.isKinematic = true;

                    far_SeaWallStraightReversed_Right.AddComponent<MeshCollider>();
                    
                    far_SeaWallStraightReversed_Right.AddComponent<SpawnEnvironment>();
                    far_SeaWallStraightReversed_Right.tag = "genGround";
                    far_SeaWallStraightReversed_Right.SetActive(true);

                    // Add reference for deletion
                    TrackStats.maraudersMap[TrackStats.trackPosition].Add(far_SeaWallStraightReversed_Right);
                }
                break;
            case Constants.CityRoadGroundRotated:
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        GameObject far_CityRoadGroundRotated_Left = new GameObject();
                        far_CityRoadGroundRotated_Left = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        far_CityRoadGroundRotated_Left.SetActive(false);
                        
                        far_CityRoadGroundRotated_Left.transform.Rotate(0.0f, 0.0f, 0.0f);
                        far_CityRoadGroundRotated_Left.transform.position = new Vector3(j * Constants.PositionAheadImproved - 35.0f + i * 80.0f, 0.0f, trackPos + 15.0f); ;

                        Rigidbody groundRigidBodyLeft1 = far_CityRoadGroundRotated_Left.AddComponent<Rigidbody>();
                        groundRigidBodyLeft1.useGravity = false;
                        groundRigidBodyLeft1.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyLeft1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyLeft1.isKinematic = true;

                        far_CityRoadGroundRotated_Left.AddComponent<MeshCollider>();
                        
                        far_CityRoadGroundRotated_Left.AddComponent<SpawnEnvironment>();
                        far_CityRoadGroundRotated_Left.tag = "genGround";
                        far_CityRoadGroundRotated_Left.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(far_CityRoadGroundRotated_Left);

                        GameObject far_CityRoadGroundRotated_Right = new GameObject();
                        far_CityRoadGroundRotated_Right = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        far_CityRoadGroundRotated_Right.SetActive(false);
                        
                        far_CityRoadGroundRotated_Right.transform.Rotate(0.0f, 180.0f, 0.0f);
                        far_CityRoadGroundRotated_Right.transform.position = new Vector3(j * Constants.PositionAheadImproved - 40.0f + i * 80.0f, 0.0f, trackPos - 5.0f); ;

                        Rigidbody groundRigidBodyRight1 = far_CityRoadGroundRotated_Right.AddComponent<Rigidbody>();
                        groundRigidBodyRight1.useGravity = false;
                        groundRigidBodyRight1.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyRight1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyRight1.isKinematic = true;

                        far_CityRoadGroundRotated_Right.AddComponent<MeshCollider>();
                        
                        far_CityRoadGroundRotated_Right.AddComponent<SpawnEnvironment>();
                        far_CityRoadGroundRotated_Right.tag = "genGround";
                        far_CityRoadGroundRotated_Right.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(far_CityRoadGroundRotated_Right);
                    }
                }
                break;
            case Constants.GrassSquareGround:
            case Constants.CityBasicGround:
                for (int i = 0; i < 2; i++)
                    {
                        GameObject outer_GrassSquare_Ground = new GameObject();
                        outer_GrassSquare_Ground = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        outer_GrassSquare_Ground.SetActive(false);

                        outer_GrassSquare_Ground.transform.Rotate(0.0f, 0.0f, 0.0f);
                        outer_GrassSquare_Ground.transform.position = new Vector3(-20.0f+ i * 80.0f, 0.0f, trackPos + 15.0f);
                        outer_GrassSquare_Ground.transform.localScale = new Vector3(4.0f, 1.0f, 4.0f);
                        outer_GrassSquare_Ground.AddComponent<MeshCollider>();

                        Rigidbody groundRigidBodyLeft1 = outer_GrassSquare_Ground.AddComponent<Rigidbody>();
                        groundRigidBodyLeft1.useGravity = false;
                        groundRigidBodyLeft1.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyLeft1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyLeft1.isKinematic = true;

                        outer_GrassSquare_Ground.SetActive(true);

                        // Add reference for deletion
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(outer_GrassSquare_Ground);
                    }                
                break;
        }

        // 1 Segment generated
        // (TrackStats.transition != true) ? TrackStats.switchCountdown-- : TrackStats.transitionCountDown--;

        if (!TrackStats.transition)
        {
            TrackStats.switchCountdown--;
            //Debug.Log(TrackStats.segment.ToString() + " VALUE REMAINING " + TrackStats.switchCountdown.ToString());
        }
        else
        {
            TrackStats.transitionCountDown--;
            //Debug.Log("TRANSITION CD REDUCED " + TrackStats.transitionCountDown.ToString());
        }
    }
    public void generateMovingComponent(int trackPos, string[] sections, bool spawnOnMove)
    {
        string component;

        component = sections[0];

        switch (component)
        {
            case Constants.CityRoadGround:
                if (spawnOnMove)
                {
                    string vehicle = determineVehicle(true);

                    if (TrackStats.spawnCar_Center == 0)
                    {
                        //GameObject parent_VehicleObj = new GameObject();
                        GameObject vehicle_CenterSpawn = new GameObject();

                        //Transform parentTrans = parent_VehicleObj.transform;

                        //parent_VehicleObj.name = "Parent Vehicle Obj";
                        Debug.Log("SPAWN VEHICLE " + vehicle);
                        vehicle_CenterSpawn = Instantiate((GameObject)Resources.Load(vehicle), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        vehicle_CenterSpawn.SetActive(false);

                        vehicle_CenterSpawn.transform.Rotate(0.0f, 0.0f, 0.0f);
                        vehicle_CenterSpawn.transform.position = new Vector3(7.5f, -0.2f, trackPos);

                        Rigidbody groundRigidBodyLeft = vehicle_CenterSpawn.AddComponent<Rigidbody>();
                        groundRigidBodyLeft.useGravity = false;
                        groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyLeft.isKinematic = true;

                        // Determine if drift car
                        var oscPattern = UnityEngine.Random.Range(0, 3);

                        BoxCollider endGame;

                        switch (oscPattern)
                        {
                            // Drift car, set box on moving child obj
                            case 2:
                                var scriptHolder = vehicle_CenterSpawn.AddComponent<MoveCarCenterPattern>();
                                scriptHolder.SetOscPattern(oscPattern);
                                GameObject child = vehicle_CenterSpawn.transform.GetChild(0).gameObject;
                                endGame = child.AddComponent<BoxCollider>();
                                break;
                            // Everything else, set box on obj
                            default:
                                endGame = vehicle_CenterSpawn.AddComponent<BoxCollider>();
                                break;
                        }

                        endGame.isTrigger = true;

                        vehicle_CenterSpawn.AddComponent<MeshCollider>();
                        vehicle_CenterSpawn.AddComponent<EndGame>();

                        //vehicle_CenterSpawn.transform.SetParent(parentTrans);

                        vehicle_CenterSpawn.SetActive(true);
                        TrackStats.maraudersMap[TrackStats.trackPosition+2].Add(vehicle_CenterSpawn);

                        // reset counter
                        TrackStats.spawnCar_Center = Random.Range(1, 2);
                    }
                    else
                    {
                        TrackStats.spawnCar_Center--;
                    }
                }
                break;
        }

        component = sections[2];

        switch (component)
        {
            // Intersection
            // DISABLE FOR NOW, KIND OF UNFAIR/NOT ENJOYABLE
            case Constants.CityRoadGround:

                //string vehicle = determineVehicle();
                //Debug.Log("ATTEMPING TO SPAWN " + vehicle);
                //for (int i=0;i<2;i++)
                //{
                //    Debug.Log("SPAWN VEHICLE " + vehicle);

                //    // Car for intersections
                //    GameObject parent_VehicleObj = new GameObject();
                //    GameObject vehicle_CityGround = new GameObject();

                //    parent_VehicleObj.name = "Parent Vehicle Obj";
                //    //vehicle_CityGround.transform.SetParent(parent_VehicleObj.transform);

                //    vehicle_CityGround = Instantiate((GameObject)Resources.Load(vehicle), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                //    vehicle_CityGround.SetActive(false);

                //    vehicle_CityGround.transform.Rotate(0.0f, -90.0f, 0.0f);
                //    vehicle_CityGround.transform.position = new Vector3(-33.0f, 0.0f, 7.5f + trackPos + i * -7.5f);

                //    Rigidbody groundRigidBodyLeft = vehicle_CityGround.AddComponent<Rigidbody>();
                //    groundRigidBodyLeft.useGravity = false;
                //    groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                //    groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                //    groundRigidBodyLeft.isKinematic = true;

                //    vehicle_CityGround.AddComponent<MeshCollider>();

                //    BoxCollider endGame = vehicle_CityGround.AddComponent<BoxCollider>();
                //    endGame.isTrigger = true;
                //    vehicle_CityGround.AddComponent<EndGame>();

                //    vehicle_CityGround.tag = "genGround";
                //    vehicle_CityGround.SetActive(true);

                //    GameObject vehicle_CityGround_Trigger = new GameObject();
                //    vehicle_CityGround_Trigger.name = "Car_Trigger_Object";
                //    vehicle_CityGround_Trigger.AddComponent<MoveCar>();

                //    BoxCollider boxCollider = vehicle_CityGround_Trigger.AddComponent<BoxCollider>();
                //    boxCollider.isTrigger = true;
                //    boxCollider.transform.position = new Vector3(0.0f, 0.0f, trackPos - 30.0f);
                //    boxCollider.center = new Vector3(10.0f, 12.0f, -25.0f);
                //    boxCollider.size = new Vector3(100.0f, 35.0f, 1.0f);

                //    vehicle_CityGround.transform.SetParent(vehicle_CityGround_Trigger.transform);

                //    // Add reference for deletion
                //    TrackStats.maraudersMap[TrackStats.trackPosition].Add(parent_VehicleObj);
                //    TrackStats.maraudersMap[TrackStats.trackPosition].Add(vehicle_CityGround_Trigger);
                //}
                break;
        }
    }
    public void generateMovingComponentExtra(int trackPos, string[] sections, bool spawnOnMove)
    {
        string component;

        component = sections[0];

        switch (component)
        {
            case Constants.CityRoadGround:
                if (spawnOnMove)
                {
                    string vehicle = determineVehicleExtra();

                    if (TrackStats.spawnVehicleExtra_Center == 0)
                    {
                        GameObject vehicle_CenterSpawn_Extra = new GameObject();

                        Debug.Log("SPAWN VEHICLE " + vehicle);
                        vehicle_CenterSpawn_Extra = Instantiate((GameObject)Resources.Load(vehicle), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        vehicle_CenterSpawn_Extra.SetActive(false);

                        vehicle_CenterSpawn_Extra.transform.Rotate(0.0f, 0.0f, 0.0f);
                        vehicle_CenterSpawn_Extra.transform.position = new Vector3(Random.Range(3.5f, 15.5f), 6.0f, trackPos);
                        vehicle_CenterSpawn_Extra.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);

                        Rigidbody groundRigidBodyLeft = vehicle_CenterSpawn_Extra.AddComponent<Rigidbody>();
                        groundRigidBodyLeft.useGravity = false;
                        groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyLeft.isKinematic = true;

                        // Determine if drift car
                        //var scriptHolder = vehicle_CenterSpawn_Extra.AddComponent<MoveCarCenterPattern>();
                        //var oscPattern = UnityEngine.Random.Range(0, 3);
                        //scriptHolder.SetOscPattern(oscPattern);

                        //BoxCollider endGame = vehicle_CenterSpawn_Extra.AddComponent<BoxCollider>();

                        //switch (oscPattern)
                        //{
                        //    // Drift car, set box on moving child obj
                        //    case 2:
                        //        GameObject child = vehicle_CenterSpawn_Extra.transform.GetChild(0).gameObject;
                        //        endGame = child.AddComponent<BoxCollider>();
                        //        break;
                        //    // Everything else, set box on obj
                        //    default:
                        //        endGame = vehicle_CenterSpawn_Extra.AddComponent<BoxCollider>();
                        //        break;
                        //}

                        //endGame.isTrigger = true;

                        vehicle_CenterSpawn_Extra.AddComponent<MeshCollider>();
                        vehicle_CenterSpawn_Extra.AddComponent<EndGame>();

                        // UFO SPECIFIC DATA
                        GameObject spotlight_GameObj = new GameObject();
                        Light spotlight = spotlight_GameObj.AddComponent<Light>();
                        spotlight_GameObj.transform.position = new Vector3(0, 0, 0);
                        //spotlight.transform.rotation = new Quaternion(90, 0, 0, 0);
                        spotlight.transform.Rotate(90, 0, 0);
                        spotlight.type = LightType.Spot;
                        spotlight.lightmapBakeType = LightmapBakeType.Mixed;
                        spotlight.range = 8;
                        spotlight.spotAngle = 70;
                        spotlight.intensity = 10;
                        spotlight.color = new Color(0, 255, 0);

                        spotlight_GameObj.transform.SetParent(vehicle_CenterSpawn_Extra.transform, false);

                        CapsuleCollider capCol = vehicle_CenterSpawn_Extra.AddComponent<CapsuleCollider>();
                        capCol.isTrigger = true;
                        capCol.center = new Vector3(0.0f, -2.0f, 0.0f);
                        capCol.radius = 2;
                        capCol.height = 5;

                        //vehicle_CenterSpawn.transform.SetParent(parentTrans);

                        vehicle_CenterSpawn_Extra.SetActive(true);
                        TrackStats.maraudersMap[TrackStats.trackPosition+2].Add(vehicle_CenterSpawn_Extra);

                        // reset counter
                        TrackStats.spawnVehicleExtra_Center = Random.Range(1, 4);
                    }
                    else
                    {
                        TrackStats.spawnVehicleExtra_Center--;
                    }
                }
                break;
        }

        component = sections[2];

        switch (component)
        {
            // Intersection
            case Constants.CityRoadGround:
                break;
        }
    }
    public void generateStationaryComponent(int trackPos, string[] sections, bool spawnOnMove)
    {
        string component;

        component = sections[0];

        switch (component)
        {
            case Constants.CityRoadGround:
                if (spawnOnMove)
                {
                    string vehicle = determineVehicleStationary();

                    if (TrackStats.spawnVehicleExtra_Center == 0)
                    {
                        GameObject vehicle_stationary = new GameObject();

                        Debug.Log("SPAWN VEHICLE STATIONARY " + vehicle);
                        vehicle_stationary = Instantiate((GameObject)Resources.Load(vehicle), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        vehicle_stationary.SetActive(false);

                        vehicle_stationary.transform.Rotate(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
                        vehicle_stationary.transform.position = new Vector3(Random.Range(3.5f, 15.5f), -0.4f, trackPos);
                        vehicle_stationary.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                        Rigidbody groundRigidBodyLeft = vehicle_stationary.AddComponent<Rigidbody>();
                        groundRigidBodyLeft.useGravity = false;
                        groundRigidBodyLeft.constraints = RigidbodyConstraints.FreezeAll;
                        groundRigidBodyLeft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        groundRigidBodyLeft.isKinematic = true;

                        BoxCollider obj = vehicle_stationary.AddComponent<BoxCollider>();
                        obj.isTrigger = true;
                        //vehicle_stationary.AddComponent<BoxCollider>();
                        vehicle_stationary.AddComponent<EndGame>();

                        //vehicle_CenterSpawn.transform.SetParent(parentTrans);

                        vehicle_stationary.SetActive(true);
                        TrackStats.maraudersMap[TrackStats.trackPosition].Add(vehicle_stationary);

                        // reset counter
                        TrackStats.spawnVehicleExtra_Center = Random.Range(1, 5);
                    }
                    else
                    {
                        TrackStats.spawnVehicleExtra_Center--;
                    }
                }
                break;
        }

        component = sections[2];

        switch (component)
        {
            // Intersection
            case Constants.CityRoadGround:
                break;
        }
    }
    public string[] determineEnvironmentSection(string groundComponent)
    {
        string[] toSpawn = { "", "" };
        string component = "";

        // Rate is 0 - #, higher value = rarer
        // 2 - 50%
        // 3 - 33%
        // 4 - 25%
        // etc
        float spawningRate = 0.0f;

        switch (groundComponent)
        {
            case Constants.BeachSideGround:
            case Constants.Water:

                string componentSpawning = "";

                switch (groundComponent)
                {
                    case Constants.BeachSideGround:
                        if (Random.Range(0, 2) < 1)
                        {
                            componentSpawning = Constants.EnvironmentBeachChair_123;
                            spawningRate = Constants.Percent_33;
                        }
                        else
                        {
                            componentSpawning = Constants.EnvironmentBeachUmbrella_123;
                            spawningRate = Constants.Percent_33;
                        }
                        break;
                    case Constants.Water:
                        componentSpawning = Constants.EnvironmentRocks_123;
                        spawningRate = Constants.Percent_25;
                        break;
                }

                // Run twice for 2 tiles
                for (int i = 0; i < 2; i++)
                {
                    if (Random.Range(0, spawningRate) < 1)
                    {
                        // Spawn
                        float randomNumber = Random.Range(1, 4);
                        component = componentSpawning + randomNumber.ToString();

                        toSpawn[i] = component;
                    }
                }
                break;
            case Constants.CityGroundT:
            case Constants.CityBasicGround:
                string componentSpawning1 = "";

                // Run twice for 2 tiles
                for (int i = 0; i < 2; i++)
                {
                    float randomNumGen = Random.Range(0, 7);

                    if (randomNumGen < 3)
                    {
                        componentSpawning1 = Constants.BuildingOffice_Base;

                        switch (Random.Range(0, 4))
                        {
                            case 0:
                                componentSpawning1 += "Large_";
                                break;
                            case 1:
                                componentSpawning1 += "Medium_";
                                break;
                            case 2:
                                componentSpawning1 += "Small_";
                                break;
                            case 3:
                                componentSpawning1 += "Stepped_";
                                break;
                        }

                        switch (Random.Range(0, 3))
                        {
                            case 0:
                                componentSpawning1 += "Blue";
                                break;
                            case 1:
                                componentSpawning1 += "Grey";
                                break;
                            case 2:
                                componentSpawning1 += "Brown";
                                break;
                        }
                    }
                    else if (randomNumGen == 6)
                    {
                        componentSpawning1 = Constants.BuildingSpecificShop_Base;

                        switch (Random.Range(0, 2))
                        {
                            case 0:
                                componentSpawning1 += "BaberShop";
                                break;
                            case 1:
                                componentSpawning1 += "CoffeeShop";
                                break;
                        }
                    }
                    else if (randomNumGen == 5)
                    {
                        componentSpawning1 = Constants.BuildingStore_Base;

                        switch (Random.Range(0, 3))
                        {
                            case 0:
                                componentSpawning1 += "Video";
                                break;
                            case 1:
                                componentSpawning1 += "Drug";
                                break;
                            case 2:
                                componentSpawning1 += "Pawn";
                                break;
                        }
                    }   
                    else if (randomNumGen == 4)
                    {
                        componentSpawning1 = Constants.BuildingShop_Base;
                        int type = (Random.Range(1, 6));
                        componentSpawning1 += type.ToString();
                    }
                    else if (randomNumGen == 3)
                    {
                        
                    }

                    // Spawn
                    toSpawn[i] = componentSpawning1;

                    componentSpawning1 = "";
                }
                break;
            default:
                // do nothing
                break;
        }

        return toSpawn;
    }
    public string[] determineDetails(string groundComponent)
    {
        string[] toSpawn = { "", "" };
        string component = "";

        // Rate is 0 - #, higher value = rarer
        // 2 - 50%
        // 3 - 33%
        // 4 - 25%
        // etc
        // float spawningRate = 0.0f;

        switch (groundComponent)
        {
            case Constants.CityRoadGround:
                switch (TrackStats.segment)
                {
                    case Constants.City:
                        for (int i=0;i<2;i++)
                        {
                            if (Random.Range(0, 2) < 1)
                            {
                                switch (Random.Range(0, 8))
                                {
                                    case 0:
                                        component = Constants.DetailGarbageBin;
                                        break;
                                    case 1:
                                        component = Constants.DetailTrash;
                                        break;
                                    case 2:
                                        component = Constants.DetailHydrant;
                                        break;
                                    case 3:
                                        component = Constants.DetailExitSign;
                                        break;
                                    case 4:
                                        component = Constants.DetailTree1;
                                        break;
                                    case 5:
                                        component = Constants.DetailTree2;
                                        break;
                                    case 6:
                                        component = Constants.DetailTree3;
                                        break;
                                    case 7:
                                        component = Constants.DetailTree4;
                                        break;
                                }
                                toSpawn[i] = component;
                            }
                        }
                        break;
                }
                break;
        }

        return toSpawn;
    }
    public string[] determineDetailsComplexObj(string groundComponent)
    {
        string[] toSpawn = { "", "", "", "", "", "" };

        switch (groundComponent)
        {
            // Park
            case Constants.GrassSquareGround:
                toSpawn[0] = Constants.DetailMemorial;
                toSpawn[1] = Constants.DetailPlanter;
                toSpawn[2] = Constants.DetailPlanter;
                toSpawn[3] = Constants.DetailTree3;
                toSpawn[4] = Constants.DetailTree4;
                toSpawn[5] = Constants.DetailGarbageBin;
                break;
        }

        return toSpawn;
    }

/*    public void deterAndGenDetailsOnEnvironment(int trackPos, int horizontalPos, string environmentComponent)
    {

        switch (horizontalPos)
        {
            case Constants.Center:

                break;
            case Constants.Side:
                switch (environmentComponent)
                {
                    case Constants.BuildingOffice_Base + "Large_Blue":
                    case Constants.BuildingOffice_Base + "Large_Brown":
                    case Constants.BuildingOffice_Base + "Large_Grey":
                    case Constants.BuildingOffice_Base + "Medium_Blue":
                    case Constants.BuildingOffice_Base + "Medium_Brown":
                    case Constants.BuildingOffice_Base + "Medium_Grey":
                    case Constants.BuildingOffice_Base + "Small_Blue":
                    case Constants.BuildingOffice_Base + "Small_Brown":
                    case Constants.BuildingOffice_Base + "Small_Grey":
                        switch (Random.Range(0, 1))
                        {
                            case 0:
                                float height = 0;

                                switch (environmentComponent)
                                {
                                    case Constants.BuildingOffice_Base + "Large_Blue":
                                    case Constants.BuildingOffice_Base + "Large_Brown":
                                    case Constants.BuildingOffice_Base + "Large_Grey":
                                        height = 25.0f;
                                        break;
                                    case Constants.BuildingOffice_Base + "Medium_Blue":
                                    case Constants.BuildingOffice_Base + "Medium_Brown":
                                    case Constants.BuildingOffice_Base + "Medium_Grey":
                                        height = 15f;
                                        break;
                                    case Constants.BuildingOffice_Base + "Small_Blue":
                                    case Constants.BuildingOffice_Base + "Small_Brown":
                                    case Constants.BuildingOffice_Base + "Small_Grey":
                                        height = 10.0f;
                                        break;
                                }

                                GameObject BuildingOffierLargeBillboard = new GameObject();
                                BuildingOffierLargeBillboard = Instantiate((GameObject)Resources.Load(Constants.DetailBillBoard), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                                BuildingOffierLargeBillboard.SetActive(false);

                                BuildingOffierLargeBillboard.transform.Rotate(0.0f, 0.0f, 0.0f);
                                BuildingOffierLargeBillboard.transform.position = new Vector3(0.0f, height, trackPos - 15.0f);
                                BuildingOffierLargeBillboard.AddComponent<MeshCollider>();

                                Rigidbody groundRigidBodyLeft7 = BuildingOffierLargeBillboard.AddComponent<Rigidbody>();
                                groundRigidBodyLeft7.useGravity = false;
                                groundRigidBodyLeft7.constraints = RigidbodyConstraints.FreezeAll;
                                groundRigidBodyLeft7.collisionDetectionMode = CollisionDetectionMode.Continuous;
                                groundRigidBodyLeft7.isKinematic = true;

                                BuildingOffierLargeBillboard.SetActive(true);

                                // Add reference for deletion
                                TrackStats.maraudersMap[TrackStats.trackPosition].Add(BuildingOffierLargeBillboard);
                                break;
                            default:
                                // Do Nothing
                                break;
                        }
                        break;
                    default:
                        // Do nothing
                        break;
                }
                
                break;
            case Constants.FarSide:

                break;
        }

        }
*/    
    public string[] determineGroundSection()
    {
        string[] groundSection = { "", "", "" };

        if (!TrackStats.transition)
        {
            switch (TrackStats.segment)
            {
                case Constants.City:
                    float randomNumber1 = Random.Range(0, 10);

                    if (randomNumber1 > 5)
                    {
                        groundSection[0] = Constants.CityIntersectionGround;
                        groundSection[1] = Constants.CityRoadGround;
                        groundSection[2] = Constants.CityRoadGround;
                    }
                    else
                    {
                        groundSection[0] = Constants.CityRoadGround;

                        for (int i=1;i<3;i++)
                        {
                            switch (Random.Range(0,7))
                            {
                                case 0:
                                    groundSection[i] = Constants.GrassSquareGround;
                                    break;
                                default:
                                    groundSection[i] = Constants.CityBasicGround;
                                    break;
                            }
                        }
                    }


                    //Debug.Log("PRINT CITY ROW");
                    break;
                case Constants.Beach:
                    //Debug.Log("PRINT BEACH ROW");
                    float randomNumber2 = Random.Range(0, 10);

                    if (randomNumber2 < 5)
                    {
                        groundSection[0] = Constants.CityBridge;
                        groundSection[1] = Constants.CityBridgeBeach;
                        groundSection[2] = Constants.Water;
                    }
                    else
                    {
                        groundSection[0] = Constants.CityRoadGround;
                        groundSection[1] = Constants.BeachSideGround;
                        groundSection[2] = Constants.Water;
                    }
                    break;
            }
        }
        else
        {
            switch (TrackStats.segmentTransition)
            {
                // Close Straight Beach
                case Constants.transitionStep_B_C_1:
                    groundSection[0] = Constants.CityRoadGround;
                    groundSection[1] = Constants.BeachSideGround;
                    groundSection[2] = Constants.Water;
                    TrackStats.segmentTransition = Constants.transitionStep_B_C_2;
                    //Debug.Log("TransitionStep BC 1 Printed");
                    break;
                // Corner Beach
                case Constants.transitionStep_B_C_2:
                    groundSection[0] = Constants.CityRoadGround;
                    groundSection[1] = Constants.BeachCornerGround;
                    groundSection[2] = Constants.Water;
                    TrackStats.segmentTransition = Constants.transitionStep_B_C_3;
                    //Debug.Log("TransitionStep BC 2 Printed");
                    break;
                // Bridge
                case Constants.transitionStep_B_C_3:
                    groundSection[0] = Constants.CityBridge;
                    groundSection[1] = Constants.SeaWallStraight;
                    groundSection[2] = Constants.SeaWallStraight;
                    TrackStats.segmentTransition = Constants.transitionStep_B_C_4;
                    //Debug.Log("TransitionStep BC 3 Printed");
                    break;
                // Road Mesh
                case Constants.transitionStep_B_C_4:
                    groundSection[0] = Constants.CityRoadGround;
                    groundSection[1] = Constants.CityBasicGround;
                    groundSection[2] = Constants.CityBasicGround;
                    //Debug.Log("TransitionStep BC 4 Printed");
                    break;
                case Constants.transitionStep_C_B_1:
                    groundSection[0] = Constants.CityRoadGround;
                    groundSection[1] = Constants.CityBasicGround;
                    groundSection[2] = Constants.CityBasicGround;
                    TrackStats.segmentTransition = Constants.transitionStep_C_B_2;
                    //Debug.Log("TransitionStep CB 1 Printed");
                    break;
                case Constants.transitionStep_C_B_2:
                    groundSection[0] = Constants.CityBridge;
                    groundSection[1] = Constants.SeaWallStraightReversed;
                    groundSection[2] = Constants.SeaWallStraightReversed;
                    TrackStats.segmentTransition = Constants.transitionStep_C_B_3;
                    //Debug.Log("TransitionStep CB 2 Printed");
                    break;
                case Constants.transitionStep_C_B_3:
                    groundSection[0] = Constants.CityRoadGround;
                    groundSection[1] = Constants.BeachCornerGroundReversed;
                    groundSection[2] = Constants.Water;
                    TrackStats.segmentTransition = Constants.transitionStep_C_B_4;
                    //Debug.Log("TransitionStep CB 3 Printed");
                    break;
                case Constants.transitionStep_C_B_4:
                    groundSection[0] = Constants.CityRoadGround;
                    groundSection[1] = Constants.BeachSideGround;
                    groundSection[2] = Constants.Water;
                    //Debug.Log("TransitionStep CB 1 Printed");
                    break;
            }
        }

        return groundSection;
    }
    public string determineVehicleStationary()
    {
        string vehicleStationary = "";

        switch (Random.Range(0, 1))
        {
            case 0:
                vehicleStationary += Constants.Vehicle_Stationary;
                break;
        }

        return vehicleStationary;
    }
    public string determineVehicleExtra()
    {
        string vehicleExtra = "";

        switch (Random.Range(0,1))
        {
            case 0:
                vehicleExtra += Constants.Vehicle_UFO;
                int num = Random.Range(1, 6);
                vehicleExtra += num.ToString();
                break;
        }

        return vehicleExtra;
    }
    public string determineVehicle(bool onlyDriftType = false)
    {
        string vehicle = "";

        vehicle += Constants.Vehicle_CarBase;

        // Need to solve drift car problem
        // Need to create controller for each vehicle prefab + animator
        // or limit drift spawning to car (not ideal)

        //switch (Random.Range(0,3))
        //{
        //    case 0:
        //        vehicle += Constants.Vehicle_CarBase;
        //        break;
        //    case 1:
        //        vehicle += Constants.Vehicle_VanBase;
        //        break;
        //    case 2:
        //        vehicle += Constants.Vehicle_WagonBase;
        //        break;
        //}

        switch (Random.Range(0,3))
        {
            case 0:
                vehicle += "blue";
                break;
            case 1:
                vehicle += "red";
                break;
            case 2:
                vehicle += "green";
                break;
        }

        return vehicle;
    }
    public string determineInternalInteractive()
    {
        string interactiveComp = Constants.Interactive_coin;

        switch (Random.Range(0, 3))
        {
            case 0:
                interactiveComp += "Bomb1";
                break;
            case 1:
                interactiveComp += "Energy1";
                break;
            case 2:
                interactiveComp += "Value1";
                break;
        }

        return interactiveComp;
    }
    public void resetSegmentValues()
    {
        // If switch countdown reaches 0, set transition countdown
        if (TrackStats.switchCountdown == 0)
        {
            TrackStats.transition = true;

            // Set up next segment
            if (TrackStats.segment == Constants.City)
            {
                TrackStats.segment = Constants.Beach;
                TrackStats.transitionCountDown = Constants.NumOfTranSteps_C_B;
                TrackStats.segmentTransition = Constants.transitionStep_C_B_1;
            }
            else
            {
                TrackStats.segment = Constants.City;
                TrackStats.transitionCountDown = Constants.NumOfTranSteps_B_C;
                TrackStats.segmentTransition = Constants.transitionStep_B_C_1;
            }

            TrackStats.switchCountdown = -1;

        }

        if (TrackStats.transitionCountDown == 0)
        {
            int maxRange = 0;
            int minRange = 0;

            switch (TrackStats.segment)
            {
                case Constants.City:
                    maxRange = Constants.CityMaxLength;
                    minRange = Constants.CityMinLength;
                    break;
                case Constants.Beach:
                    maxRange = Constants.BeachMaxLength;
                    minRange = Constants.BeachMinLength;
                    break;
            }

            TrackStats.switchCountdown = Random.Range(minRange, maxRange);
            TrackStats.transitionCountDown = -1;
            TrackStats.transition = false;
        }
    }

    public void deleteGround()
    {
        for (int i = 0; i < TrackStats.maraudersMap[TrackStats.trackPositionDeletion].Count; i++)
        {
            // Debug.Log(" --- DELETE --- " + TrackStats.maraudersMap[TrackStats.trackPositionDeletion][i].name.ToString());
            GameObject.Destroy(TrackStats.maraudersMap[TrackStats.trackPositionDeletion][i]);
        }

        TrackStats.trackPositionDeletion++;

        if (TrackStats.trackPositionDeletion >= 99)
        {
            TrackStats.trackPositionDeletion = 0;
        }
    }

    public void generateTerrain()
    {
        // Debug.Log("Generate Terrain...");
    }

    public void generateFlatPlane()
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = new Vector3(10.0f, 0.01f, 9990.0f);
        plane.transform.localScale = new Vector3(2, 1, 2000);
        plane.GetComponent<Renderer>().material = transparentPlane;

        Debug.Log("Generate flat plane spawned");
        plane.name = "GROUND_BARRIER";
    }
}
