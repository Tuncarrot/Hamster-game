using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGround : MonoBehaviour
{
    public const int numOfBarriers = Constants.numOfBarriers;
    public const int numOfObj = Constants.numOfObj;

    GameObject groundPrefab;
    private GameObject newGround;

    public int houseCheck = 0;
    public int intObjCheck = 0;

    Transform currentObj;

    public bool spawnGround = false;
    public bool initializeStart = false;

    // Start is called before the first frame update
    void Start()
    {
        groundPrefab = (GameObject)Resources.Load("/Ground");
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

                    // Debug.Log("Inside initialize Start");
                    generateCoreComponent(0, Constants.CityGround);
                    generateSideComponent(Constants.GenLeftSide, Constants.PositionAheadStart, 0);
                    generateSideComponent(Constants.GenRightSide, Constants.PositionAheadStart, 0);
                    generateInternalComponent(Constants.PositionAheadStart, 0);
                    
                    generateCoreComponent(1, Constants.CityGround);
                    generateSideComponent(Constants.GenLeftSide, Constants.PositionAheadCont, 1);
                    generateSideComponent(Constants.GenRightSide, Constants.PositionAheadCont, 1);
                    generateInternalComponent(Constants.PositionAheadCont, 1);

                    generateCoreComponent(2, Constants.CityGround);
                    generateSideComponent(Constants.GenLeftSide, Constants.PositionAheadDouble, 2);
                    generateSideComponent(Constants.GenRightSide, Constants.PositionAheadDouble, 2);
                    generateInternalComponent(Constants.PositionAheadDouble, 2);

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

                    string component = determineGround(TrackStats.trackPosition);

                    generateCoreComponent(TrackStats.trackPosition, component);

                    if (component == Constants.CityGround)
                    {
                        generateSideComponent(Constants.GenLeftSide, Constants.PositionAheadDouble, TrackStats.trackPosition);
                        generateSideComponent(Constants.GenRightSide, Constants.PositionAheadDouble, TrackStats.trackPosition);
                    }

                    generateInternalComponent(Constants.PositionAheadDouble, TrackStats.trackPosition);

                    spawnGround = true;
                    // Debug.Log(TrackStats.trackPosition.ToString() + " Track stat");
                    TrackStats.trackPosition++;
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

    public void generateInternalComponent(float posAhead, int trackPos)
    {
        string resource = "";

        Vector3 objScale = new Vector3();
        float objWeight = 0.0f;

        for (int i = 0; i < numOfObj; i++)
        {
            // Check if spawn house
            float chance = Random.Range(0.0f, 10.0f);

            if (chance > 6.0f)
            {
                // Debug.Log("Obj Generation Triggered...");
                // Check if obj has recently spawned, if so don't spawn anything
                if (intObjCheck < 1)
                {

                    // Apply number to change colour of prefab
                    int objToGenerate = Random.Range(1, 7);

                    switch (objToGenerate)
                    {
                        case 1:
                            resource = "dumpster_mesh";
                            objScale.Set(Constants.DumpsterScale, Constants.DumpsterScale, Constants.DumpsterScale);
                            objWeight = Constants.DumpsterWeight;
                            break;
                        case 2:
                            resource = "Prop_TirePile";
                            objScale.Set(Constants.TirePileScale, Constants.TirePileScale, Constants.TirePileScale);
                            objWeight = Constants.TirePileWeight;
                            break;
                        case 3:
                            resource = "trash_mesh";
                            objScale.Set(Constants.TrashBinScale, Constants.TrashBinScale, Constants.TrashBinScale);
                            objWeight = Constants.TrashBinWeight;
                            break;
                        case 4:
                            resource = "coinValue1";
                            objScale.Set(Constants.CoinScale, Constants.CoinScale, Constants.CoinScale);
                            break;
                        case 5:
                            resource = "coinBomb1";
                            objScale.Set(Constants.CoinScale, Constants.CoinScale, Constants.CoinScale);
                            break;
                        case 6:
                            resource = "coinEnergy1";
                            objScale.Set(Constants.CoinScale, Constants.CoinScale, Constants.CoinScale);
                            break;
                    }

                    // Debug.Log(">>>>> SPAWN >>>>>" + resource);

                    GameObject obstacleObj = Instantiate((GameObject)Resources.Load(resource), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), newGround.transform);
                    obstacleObj.SetActive(false);

                    // Place Obj between the two fences
                    obstacleObj.transform.localScale = objScale;

                    BoxCollider boxCol = obstacleObj.AddComponent<BoxCollider>();

                    // Get not add, prefab already has rigidbody
                    Rigidbody rigBody = obstacleObj.AddComponent<Rigidbody>();
                    rigBody.useGravity = true;
                    rigBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    rigBody.mass = objWeight;

                    switch (resource) // DRY
                    {
                        case "coinValue1":
                            obstacleObj.AddComponent<CoinGoldSelfDestruct>();
                            obstacleObj.transform.position = generateXCoordinates(Constants.LeftFenceXPos, Constants.RightFenceXPos, i, posAhead, Constants.CoinYPosAdj);
                            obstacleObj.transform.Rotate(Constants.GenericObjRotateX, 0.0f, 0.0f);
                            boxCol.isTrigger = true;
                            rigBody.useGravity = false;
                            obstacleObj.tag = "interactive";
                            break;
                        case "coinBomb1":
                            obstacleObj.AddComponent<bomb>();
                            obstacleObj.transform.position = generateXCoordinates(Constants.LeftFenceXPos, Constants.RightFenceXPos, i, posAhead, Constants.CoinYPosAdj);
                            obstacleObj.transform.Rotate(Constants.GenericObjRotateX, 0.0f, 0.0f);
                            boxCol.isTrigger = true;
                            rigBody.useGravity = false;
                            obstacleObj.tag = "interactive";
                            break;
                        case "coinEnergy1":
                            obstacleObj.AddComponent<energyCoin>();
                            obstacleObj.transform.position = generateXCoordinates(Constants.LeftFenceXPos, Constants.RightFenceXPos, i, posAhead, Constants.CoinYPosAdj);
                            obstacleObj.transform.Rotate(Constants.GenericObjRotateX, 0.0f, 0.0f);
                            boxCol.isTrigger = true;
                            rigBody.useGravity = false;
                            obstacleObj.tag = "interactive";
                            break;
                        default:
                            obstacleObj.transform.Rotate(Constants.GenericObjRotateX, Constants.GenericObjRotateYZ, Constants.GenericObjRotateYZ);
                            obstacleObj.transform.position = generateXCoordinates(Constants.LeftFenceXPos, Constants.RightFenceXPos, i, posAhead);
                            break;
                    }

                    obstacleObj.SetActive(true);
                    intObjCheck = 4;
                }
            }
            intObjCheck--;
        }
    }

    public Vector3 generateXCoordinates(float xStartPosition, float xEndPosition, int zAdj, float posAhead, float optYAdj = 0.0f)
    {
        Vector3 objPosition = new Vector3(Random.Range(xStartPosition, xEndPosition), Constants.YPosGround + optYAdj, Constants.ZPosGround + (zAdj * 1.67f) + transform.position.z + posAhead);

        return objPosition;
    }

    public void generateSideComponent(int side, float posAhead, int trackPos)
    {
        GameObject[] barriersArray = new GameObject[numOfBarriers];

        string  resource        = "Building_House_0";

        float   fenceXPos       = 0.0f;
        float   houseXPosAdjust = 0.0f;

        switch (side)
        {
            case Constants.GenLeftSide:
                fenceXPos = Constants.LeftFenceXPos;
                houseXPosAdjust = Constants.LeftHouseXPosAdj;
                break;
            case Constants.GenRightSide:
                fenceXPos = Constants.RightFenceXPos;
                houseXPosAdjust = Constants.RightHouseXPosAdj;
                break;
        }

        for (int i = 0; i < numOfBarriers; i++)
        {
            // Check if spawn house
            float chance = Random.Range(0.0f, 6.0f);

            // Don't spawn house at the end of generated ground
            if (i < numOfBarriers - 3)
            {
                if (chance < 1.0f)
                {
                    // Debug.Log("Chance Triggered...");
                    // Check if house has recently spawned, if so don't spawn anything
                    if (houseCheck < 1)
                    {
                        // Spawn House
                        // Debug.Log("Spawn house...");

                        // Apply number to change colour of prefab
                        int colour = Random.Range(1, 5);
                        resource = "Building_House_0" + colour.ToString();

                        // Debug.Log(">>>>> SPAWN >>>>>" + resource);

                        GameObject houseObj = Instantiate((GameObject)Resources.Load(resource), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), newGround.transform);
                        houseObj.SetActive(false);

                        houseObj.transform.position = new Vector3(fenceXPos + houseXPosAdjust, Constants.YPosGround, Constants.ZPosGround + Constants.HouseZPosAdj + (i * 1.67f) + (trackPos * Constants.PositionAhead));
                        houseObj.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
                        houseObj.transform.Rotate(90.0f, -90.0f, -90.0f);

                        houseObj.SetActive(true);
                        houseCheck = 3;
                    }
                }
            }
            if (houseCheck < 1)    // Dont spawn fences if house has spawned recently
            {
                // Left side of barrier
                GameObject barrierObj = Instantiate((GameObject)Resources.Load("fence_long_mesh_MOD"), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), newGround.transform);
                barrierObj.SetActive(false);

                barrierObj.transform.position = new Vector3(fenceXPos, Constants.YPosGround, Constants.ZPosGround + (i * 1.67f) + (trackPos * Constants.PositionAhead));
                barrierObj.transform.Rotate(0.0f, -90.0f, -90.0f);
                barrierObj.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);

                barriersArray[i] = barrierObj;

                barrierObj.SetActive(true);
            }
            houseCheck--;
        }
    }

    public GameObject generateCoreComponent(int trackPos, string component, bool generateColliders = true)
    {
        // Generate ground

        // Debug.Log("Spawn next ground segment");

        //newGround = Instantiate((GameObject)Resources.Load(component), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        newGround = Instantiate((GameObject)Resources.Load(component));

        newGround.SetActive(false);

        Rigidbody groundRigidBody = newGround.AddComponent<Rigidbody>();
        groundRigidBody.useGravity = false;
        groundRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        groundRigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        groundRigidBody.isKinematic = true;

        BoxCollider groundBoxCollider = newGround.AddComponent<BoxCollider>();

        if (generateColliders)
        {
            BoxCollider triggerBoxCollider = newGround.AddComponent<BoxCollider>();
            // triggerBoxCollider.center = new Vector3(0.0f, -0.01f, 0.03f);
            // triggerBoxCollider.size = new Vector3(0.1f, 0.18f, 0.11f);
            // triggerBoxCollider.isTrigger = true;
        }

        BoxCollider LeftBoxCollider = newGround.AddComponent<BoxCollider>();
        // LeftBoxCollider.size = new Vector3(0.0012f, 0.2f, 0.01f);
        // LeftBoxCollider.center = new Vector3(-0.0095f, 0.0f, 0.005f);

        BoxCollider RightBoxCollider = newGround.AddComponent<BoxCollider>();
        // RightBoxCollider.size = new Vector3(0.0012f, 0.2f, 0.01f); 
        // RightBoxCollider.center = new Vector3(0.0095f, 0.0f, 0.005f);

        // newGround.transform.Rotate(-90.0f, 0.0f, 0.0f);
        //newGround.transform.position = new Vector3(0.0f, 0.0f, trackPos * Constants.PositionAhead);
        newGround.transform.position = new Vector3(0.0f, 0.0f, trackPos);
        // newGround.transform.position = new Vector3(-0.08f, 0.198f, transform.position.z + posAhead);

        newGround.AddComponent<SpawnGround>();
        newGround.tag = "genGround";

        newGround.SetActive(true);

        Debug.Log(trackPos.ToString() + " TRACK POS");

        // Generate terrain 

        // Debug.Log("Generate Terrain...");

        return newGround;
    }

    public string determineGround(int trackPos)
    {
        string groundType = "";

        if (trackPos >= 0 && trackPos < 35)
        {
            //groundType = Constants.CityGround;
            groundType = Constants.SeaGround;
        }
        else if (trackPos >= 35)
        {
            //groundType = Constants.SeaGround;
            groundType = Constants.CityRoadGround;

        }

        return groundType;
    }

    public void deleteGround()
    {
        // Debug.Log("Deleting ground...");
        GameObject.Destroy(this.gameObject);  
    }

    public void generateTerrain()
    {
        // Debug.Log("Generate Terrain...");
    }
}
