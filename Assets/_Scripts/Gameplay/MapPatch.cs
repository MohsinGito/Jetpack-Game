using System.Collections.Generic;
using GameControllers;
using UnityEngine;

public class MapPatch : MonoBehaviour
{

    #region Public Attributes

    [Header("-- Background Info --")]
    public SpriteRenderer layerOne;
    public SpriteRenderer layerTwo;
    public SpriteRenderer layerThree;

    [Header("-- Environment Assets Spawn Info --")]
    public float floorPos;
    public float bottomPos;
    public Vector2 minMaxX;
    public Vector2 minMaxY;
    public List<Transform> electricObstaclePos;

    #endregion

    #region Private Attributes

    private bool initialized;
    private float deadEnd;
    private float moveSpeed;
    private string randomObstacleName;
    private List<PoolObj> spawnedAssets;
    private List<string> electricObstacles;
    private List<string> floorObstacles;
    private List<string> groundObstacles;

    public Vector3 Position
    {
        set { transform.position = value; }
        get { return transform.position; }
    }

    #endregion

    #region Public Methods

    public void SetUpPatch(GameMap _mapData,Vector3 _initialPos, float _moveSpeed, float _deadEnd)
    {
        deadEnd = _deadEnd;
        moveSpeed = _moveSpeed;
        layerOne.sprite = _mapData.layerOne;
        layerTwo.sprite = _mapData.layerTwo;
        layerThree.sprite = _mapData.layerThree;
        transform.position = _initialPos;

        spawnedAssets = new List<PoolObj>();
        floorObstacles = PoolManager.Instance.GetPoolTags("Floor Obstacles");
        groundObstacles = PoolManager.Instance.GetPoolTags("Ground Obstacles");
        electricObstacles = PoolManager.Instance.GetPoolTags("Electrical Obstacles");

        initialized = true;
        GameSession.OnGameEnd += delegate { initialized = false; };

        ResetPatch();
    }

    public bool ReachedToDeadEnd()
    {
        if(transform.position.x <= deadEnd)
            return true;
        return false;
    }

    public void ResetPatch()
    {
        foreach(PoolObj obj in spawnedAssets)
            PoolManager.Instance.ReturnToPool(obj);

        spawnedAssets.Clear();
        SetUpFloorObstacles();
        SetupGroundAssets();
        SetUpElectricObstacles();
    }

    #endregion

    #region Private Methods

    private void Update()
    {
        if (!initialized)
            return;

        transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    }

    private void SetUpFloorObstacles()
    {
        randomObstacleName = floorObstacles[Random.Range(0, floorObstacles.Count)];
        SpawnAssetAt(randomObstacleName, new Vector3(Random.Range(minMaxX.x, minMaxX.y), floorPos, 0));
    }

    private void SetupGroundAssets()
    {
        groundObstacles = Generics<string>.Randomize(groundObstacles);
        randomObstacleName = groundObstacles[Random.Range(0, groundObstacles.Count)];
        SpawnAssetAt(randomObstacleName, new Vector3(Random.Range(minMaxX.x, minMaxX.y), bottomPos, 0));
    }

    private void SetUpElectricObstacles()
    {
        if(true)
        {
            int randIndex = Random.Range(0, electricObstaclePos.Count);
            SpawnNewElectricObstacle(electricObstaclePos[randIndex].localPosition.x);
        }
        else
        {
            for (int i = 0; i < electricObstaclePos.Count; i++)
            {
                SpawnNewElectricObstacle(electricObstaclePos[i].localPosition.x);
            }
        }

        void SpawnNewElectricObstacle(float xPos)
        {
            float randomRot = Random.Range(0, 5) * 30f * (Random.Range(0, 2) == 1 ? -1 : 1);
            randomObstacleName = electricObstacles[Random.Range(0, electricObstacles.Count)];
            SpawnAssetAt(randomObstacleName, new Vector3(xPos, Random.Range(minMaxY.x, minMaxY.y), 0));
            spawnedAssets[spawnedAssets.Count - 1].Prefab.transform.eulerAngles = new Vector3(0, 0, randomRot);
        }
    }

    private void SpawnAssetAt(string _assetName, Vector3 _spawnPos)
    {
        spawnedAssets.Add(new PoolObj(_assetName, PoolManager.Instance.GetFromPool(_assetName)));
        spawnedAssets[spawnedAssets.Count - 1].Prefab.transform.parent = transform;
        spawnedAssets[spawnedAssets.Count - 1].Prefab.transform.localPosition = _spawnPos;
    }

    #endregion

}