using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 4x4 generator to use with my prefabs from blender.
/// </summary>
public class Generator4x4 : ArenaGeneratorBase
{
    protected Vector3 floorMeshSize;
    protected Vector3 wallMeshSize;
    protected Vector2 arenaDimensions;
    protected float wallPadding = 4.0f;
    protected Vector2Int arenaSizes;

    protected ArenaRootObject arenaRootObject;
    protected ArenaConstructionObject constructionStore;
    protected Transform[] floorPlates;
    protected Transform[] walls;
    protected Transform[] obstacles;

    protected int floorPlatesAmount;
    protected int wallsAmount;
    protected int obstaclesAmount;
    protected int spawnPointsAmount = 4;

    /// <summary>
    /// {@inheritdoc}
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        if (!InitialCheck())
        {
            Debug.LogError("The 4x4 generator has some errors. Fix them to proceed.");
            Application.Quit();
        }
    }

    /// <summary>
    /// {@inheritdoc}
    /// </summary>
    public override void GenerateArena(ArenaRootObject rootObject)
    {

        arenaRootObject = rootObject;
        constructionStore = rootObject.GetComponentInChildren<ArenaConstructionObject>();

        arenaSizes = ArenaManager.Manager.ArenaSizes;
        ArenaConstructionKit4x4 kit = (ArenaConstructionKit4x4)constructionKits[Random.Range(0, constructionKits.Count)];

        floorPlatesAmount = arenaSizes.x * arenaSizes.y;
        floorPlates = FillTransformArray(kit.Floors, floorPlatesAmount);

        wallsAmount = 2 * arenaSizes.x + 2 * arenaSizes.y;
        walls = FillTransformArray(kit.Walls, wallsAmount);

        obstaclesAmount = (arenaSizes.x + arenaSizes.y) / 2;
        obstacles = FillTransformArray(kit.Obstacles, obstaclesAmount);

        floorMeshSize = floorPlates[0].GetComponent<MeshFilter>().sharedMesh.bounds.size;
        wallMeshSize = walls[0].GetComponent<MeshFilter>().sharedMesh.bounds.size;
        arenaDimensions = new Vector2(floorMeshSize.x * arenaSizes.x, floorMeshSize.z * arenaSizes.y);

        GenerateFloor();
        GenerateWalls();
        GenerateObstacles();
    }

    /// <summary>
    /// {@inheritdoc}
    /// </summary>
    public override List<SpawnPointMonster> GenerateMonsterSpawnPoints()
    {
        List<SpawnPointMonster> spList = new List<SpawnPointMonster>();
        SpawnPointMonster point;
        Vector3[] positions =
        {
            new Vector3(wallPadding, 0.0f, arenaDimensions.y / 2),
            new Vector3(arenaDimensions.x - wallPadding, 0.0f, arenaDimensions.y / 2),
            new Vector3(arenaDimensions.x / 2, 0.0f, wallPadding),
            new Vector3(arenaDimensions.x / 2, 0.0f, arenaDimensions.y - wallPadding),
        };

        for (int i = 0; i < positions.Length; i++)
        {
            point = Instantiate(ArenaResourceManager.Manager.SpawnpointMonsterPrefab, positions[i], Quaternion.identity);
            point.transform.position += new Vector3(0.0f, 1.0f, 0.0f);
            point.transform.parent = arenaRootObject.transform;
            point.gameObject.SetActive(true);
            spList.Add(point);
        }

        return spList;
    }

    /// <summary>
    /// {@inheritdoc}
    /// </summary>
    public override SpawnPointPlayer GeneratePlayerSpawnPoint()
    {
        SpawnPointPlayer point = Instantiate(ArenaResourceManager.Manager.SpawnpointPlayerPrefab, Vector3.zero, Quaternion.identity);
        point.gameObject.transform.position += new Vector3(
            arenaDimensions.x / 2,
            1.0f,
            arenaDimensions.y / 2
            );
        point.transform.parent = arenaRootObject.transform;
        point.gameObject.SetActive(true);
        return point;
    }

    /// <summary>
    /// Returns an array with prefabs built out of given source.
    /// </summary>
    /// <param name="source">
    /// A source list of prefabs.
    /// </param>
    /// <param name="amount">
    /// A number of array elements.
    /// </param>
    /// <returns>
    /// An array with prefabs to use for arena building. Either an empty array if source is empty.
    /// </returns>
    protected Transform[] FillTransformArray(List<Transform> source, int amount)
    {
        Transform[] result = new Transform[amount];
        if (source.Count == 0)
        {
            return result;
        }

        int sourceCount = source.Count;
        for (int i = 0; i < amount; i++)
        {
            result[i] = source[Random.Range(0, sourceCount)];
        }

        return result;
    }

    /// <summary>
    /// {@inheritdoc}
    /// </summary>
    protected override bool InitialCheck()
    {
        for (int i = 0; i < constructionKits.Count; i++)
        {
            ArenaConstructionKit4x4 ck = (ArenaConstructionKit4x4)constructionKits[i];
            if (ck.Floors.Count == 0 || ck.Walls.Count == 0)
            {
                Debug.LogError($"The '{ck.name}' construction kit has either no floors or walls prefabs added to the lists. Please fix that.");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Generates arena floor with prepared prefabs.
    /// </summary>
    protected void GenerateFloor()
    {
        Vector3 position;
        Transform arenaComponent;
        for (int row = 0; row < arenaSizes.x; row++)
        {
            for (int col = 0; col < arenaSizes.y; col++)
            {
                position = new Vector3(row * floorMeshSize.x, 0.0f, col * floorMeshSize.z);
                arenaComponent = Instantiate(floorPlates[row + col], position, Quaternion.identity);
                arenaComponent.transform.parent = constructionStore.transform;
            }
        }
    }

    /// <summary>
    /// Generates arena walls with prepared prefabs.
    /// </summary>
    protected void GenerateWalls()
    {
        Vector3 wallCoords = new Vector3(
            (arenaSizes.x * floorMeshSize.x) - (floorMeshSize.x / 2),
            wallMeshSize.y / 2,
            -floorMeshSize.z / 2
            );
        Transform arenaComponent;
        int wallsCount = walls.Length;
        Quaternion rotation = Quaternion.identity;

        for (int i = 0; i < arenaSizes.x; i++)
        {
            arenaComponent = Instantiate(walls[Random.Range(0, wallsCount)], wallCoords, rotation);
            arenaComponent.parent = constructionStore.transform;
            wallCoords.x -= floorMeshSize.x;
        }

        rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        for (int i = 0; i < arenaSizes.y; i++)
        {
            arenaComponent = Instantiate(walls[Random.Range(0, wallsCount)], wallCoords, rotation);
            arenaComponent.parent = constructionStore.transform;
            wallCoords.z += floorMeshSize.z;
        }

        rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        for (int i = 0; i < arenaSizes.x; i++)
        {
            arenaComponent = Instantiate(walls[Random.Range(0, wallsCount)], wallCoords, rotation);
            arenaComponent.parent = constructionStore.transform;
            wallCoords.x += floorMeshSize.x;
        }

        rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
        for (int i = 0; i < arenaSizes.y; i++)
        {
            arenaComponent = Instantiate(walls[Random.Range(0, wallsCount)], wallCoords, rotation);
            arenaComponent.parent = constructionStore.transform;
            wallCoords.z -= floorMeshSize.z;
        }
    }

    /// <summary>
    /// Generates arena obstacles with prepared prefabs.
    /// </summary>
    protected void GenerateObstacles()
    {
        if (obstacles.Length == 0)
        {
            return;
        }

        Transform arenaComponent;
        int obstaclesCount = obstacles.Length;
        Vector3 position;
        float obstacleCenter;
        for (int i = 0; i < obstaclesAmount; i++)
        {
            position = GetRandomPointOnArena();
            arenaComponent = Instantiate(obstacles[Random.Range(0, obstaclesCount)], position, Quaternion.identity);
            obstacleCenter = arenaComponent.GetComponent<MeshFilter>().sharedMesh.bounds.size.y / 2;
            arenaComponent.position += new Vector3(0.0f, obstacleCenter, 0.0f);
            arenaComponent.parent = constructionStore.transform;
        }
    }

    /// <summary>
    /// Returns a random position Vector3 on the generated arena. Either returns zero vector.
    /// </summary>
    /// <returns>
    /// Vector3 with coordinates.
    /// </returns>
    protected Vector3 GetRandomPointOnArena()
    {
        if (arenaDimensions.x == 0 || arenaDimensions.y == 0)
        {
            return Vector3.zero;
        }

        float x = arenaDimensions.x - wallPadding;
        float y = arenaDimensions.y - wallPadding;

        return new Vector3(
            Random.Range(wallPadding, x),
            0.0f,
            Random.Range(wallPadding, y)
            );
    }
}
