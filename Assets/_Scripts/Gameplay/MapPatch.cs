using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MapPatch : MonoBehaviour
{

    #region Public Attributes

    [Header("-- Environment Assets Spawn Info --")]
    public float floorPos;
    public float bottomPos;
    public Vector2 minMaxX;
    public Vector2 minMaxY;
    public List<Transform> patchCoinsPos;
    public List<Transform> electricObstaclePos;

    #endregion

    #region Private Attributes

    private bool initialized;
    private float deadEnd;
    private string randomObstacleName;
    private List<string> electricObstacles;
    private List<string> floorObstacles;
    private List<string> groundObstacles;
    private List<PoolObj> spawnedAssets;
    private Transform[,] coinsPositions;
    private EnvironmentManager environmentManager;

    public Vector3 Position
    {
        set { transform.position = value; }
        get { return transform.position; }
    }

    #endregion

    #region Initialization Methods

    public void Init(EnvironmentManager _environmentManager, Vector3 _initialPos, float _moveSpeed, float _deadEnd)
    {
        deadEnd = _deadEnd;
        transform.position = _initialPos;
        environmentManager = _environmentManager;

        spawnedAssets = new List<PoolObj>();
        coinsPositions = new Transform[9, 12];
        floorObstacles = PoolManager.Instance.GetPoolTags("Floor Obstacles");
        groundObstacles = PoolManager.Instance.GetPoolTags("Ground Obstacles");
        electricObstacles = PoolManager.Instance.GetPoolTags("Electrical Obstacles");

        int listCount = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 12; j++, listCount++)
            {
                coinsPositions[i, j] = patchCoinsPos[listCount];
            }
        }
    }

    public void SetUpPatch(GameMap _mapData)
    {
        initialized = true;
        ResetPatch();
    }

    #endregion

    #region Patch Transform Methods

    private void Update()
    {
        if (!initialized)
            return;

        transform.position -= new Vector3(environmentManager.patchMoveSpeed * Time.deltaTime, 0, 0);
    }

    public bool ReachedToDeadEnd()
    {
        if (transform.position.x <= deadEnd)
            return true;
        return false;
    }

    public void ResetPatch()
    {
        foreach (PoolObj obj in spawnedAssets)
            PoolManager.Instance.ReturnToPool(obj);

        StopAllCoroutines();
        spawnedAssets.Clear();
    }

    public void SpawnElements()
    {
        environmentManager.SpawnedPatches += 1;
        if(environmentManager.spawnPickups)
        {
            if(environmentManager.SpawnedPatches % 10 == 0)
            {
                int randomInt = Random.Range(0, 2);
                switch (randomInt)
                {
                    case 0:
                        SpawnPickUp(GameContants.SHIELD);
                        break;
                    case 1:
                        SpawnPickUp(GameContants.MAGNET);
                        break;
                }
                return;
            }
        }

        if (environmentManager.spawnCoins)
        {
            if (Random.Range(0, 3) == 1 && environmentManager.SpawnedPatches > 5)
            {
                SpawnCoins(environmentManager.GetRandomCoinsPattern());
                return;
            }
        }

        if (environmentManager.spawnObstacles)
        {
            SpawnObstacles();
            if (environmentManager.SpawnedPatches % 5 == 0)
                SpawnPickUp(GameContants.BOMB);
            return;
        }
    }

    #endregion

    #region Patch Elements Spawning 

    private void SpawnObstacles()
    {
        SetUpFloorObstacles();
        SetupGroundAssets();
        SetUpElectricObstacles();
    }

    private void SpawnCoins(ArrayLayout coinsPattern)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                if(coinsPattern.rows[i].row[j])
                {
                    spawnedAssets.Add(new PoolObj("Coin", PoolManager.Instance.GetFromPool("Coin")));
                    spawnedAssets[spawnedAssets.Count - 1].Prefab.transform.parent = transform;
                    spawnedAssets[spawnedAssets.Count - 1].Prefab.GetComponent<MapAsset>().SetActive(true);
                    spawnedAssets[spawnedAssets.Count - 1].Prefab.transform.position = coinsPositions[i, j].position;
                }
            }
        }
    }

    private void SpawnPickUp(string pickUpTag)
    {
        spawnedAssets.Add(new PoolObj(pickUpTag, PoolManager.Instance.GetFromPool(pickUpTag)));
        spawnedAssets[spawnedAssets.Count - 1].Prefab.transform.parent = transform;
        spawnedAssets[spawnedAssets.Count - 1].Prefab.transform.localPosition =
            new Vector3(Random.Range(minMaxX.x, minMaxX.y), Random.Range(minMaxY.x, minMaxY.y), 0);
    }

    #endregion

    #region Patch Obstacle Managing Methods

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
        if (environmentManager.SpawnedPatches % 2 == 0 && environmentManager.SpawnedPatches > 10)
        {
            for (int i = 0; i < electricObstaclePos.Count; i++)
            {
                SpawnNewElectricObstacle(electricObstaclePos[i].localPosition.x);
            }
        }
        else
        {
            int randIndex = Random.Range(0, electricObstaclePos.Count);
            SpawnNewElectricObstacle(electricObstaclePos[randIndex].localPosition.x);

            if (Random.Range(0, 4) == 2)
                StartCoroutine(RotateObstacle(spawnedAssets[spawnedAssets.Count - 1].Prefab.transform));
        }

        void SpawnNewElectricObstacle(float xPos)
        {
            float randomRot = Random.Range(0, 5) * 30f * (Random.Range(0, 2) == 1 ? -1 : 1);
            randomObstacleName = electricObstacles[Random.Range(0, electricObstacles.Count)];
            SpawnAssetAt(randomObstacleName, new Vector3(xPos, Random.Range(minMaxY.x, minMaxY.y), 0));
            spawnedAssets[spawnedAssets.Count - 1].Prefab.transform.eulerAngles = new Vector3(0, 0, randomRot);
        }

        IEnumerator RotateObstacle(Transform _obstacle)
        {
            while(true)
            {
                _obstacle.Rotate(0f, 0f, (Random.Range(50f, 100f) * Time.deltaTime));
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void SpawnAssetAt(string _assetName, Vector3 _spawnPos)
    {
        spawnedAssets.Add(new PoolObj(_assetName, PoolManager.Instance.GetFromPool(_assetName)));
        spawnedAssets[spawnedAssets.Count - 1].Prefab.GetComponent<MapAsset>().SetActive(true);
        spawnedAssets[spawnedAssets.Count - 1].Prefab.transform.parent = transform;
        spawnedAssets[spawnedAssets.Count - 1].Prefab.transform.localPosition = _spawnPos;
    }

    public void DestroyAllObstacles()
    {
        bool canDestroy = true;
        List<string> pickUpsTags = PoolManager.Instance.GetPoolTags("Power Up");
        foreach (PoolObj obj in spawnedAssets)
        {
            canDestroy = true;
            for (int i = 0; i < pickUpsTags.Count - 1; i++)
            {
                if (pickUpsTags[i].Equals(obj.Tag))
                {
                    canDestroy = false;
                    break;
                }
            }

            if (canDestroy)
                environmentManager.DestroyMapElement(obj.Prefab);
        }
    }

    #endregion

}