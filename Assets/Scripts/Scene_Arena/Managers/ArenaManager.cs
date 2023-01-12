using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Spellweavers;
using UnityEngine.UIElements;

public class ArenaManager : MonoBehaviour
{
    protected static ArenaManager manager;
    public static ArenaManager Manager { get => manager; }

    protected readonly Vector2Int arenaSizeLimitsDefault = new(6, 80);

    [Tooltip("A border values for the arena size in one dimension. These values will be used to get amount of rows and columns. If any value is lesser or bigger than the default, it'll be reset to one of the default limits.")]
    
    [SerializeField] protected Vector2Int arenaSizeLimits;

    [SerializeField] protected ArenaWaveController waveController;
    public ArenaWaveController WaveController { get => waveController ?? GetComponent<ArenaWaveController>(); }

    protected bool hasGeneratedArena = false;

    protected ArenaRootObject arenaObject;

    protected Vector2Int arenaSizes;
    public Vector2Int ArenaSizes { get => arenaSizes; }

    protected PlayerController player;
    public PlayerController Player { get => player; }

    protected SpawnablePlayer playerBase;
    public SpawnablePlayer PlayerBase { get => playerBase; }

    [SerializeField] protected List<ArenaGeneratorBase> arenaGenerators;
    protected ArenaGeneratorBase activeGenerator;

    protected float collectedResources = 0;
    public float CollectedResources { get => collectedResources; }

    [SerializeField] protected int waveNumber = 0;

    [SerializeField] protected int spawnedMonsters = 0;

    [SerializeField] protected int totalToSpawn = 0;

    [SerializeField] protected int maxMonstersPerTime = 200;
    public int MaxMonstersPerTime { get => maxMonstersPerTime; }

    protected List<SpawnPointMonster> spawnPointMonsters;
    public List<SpawnPointMonster> SpawnPointMonsters { get => spawnPointMonsters; }

    protected SpawnPointPlayer spawnPointPlayer;
    public SpawnPointPlayer SpawnPointPlayer { get => spawnPointPlayer; }

    [SerializeField] List<Vector3> aiSpawnPositons;

    [SerializeField] List<string> toSpawn;

    protected void Awake()
    {
        if (manager != null && manager != this)
        {
            Destroy(this);
        }
        else
        {
            manager = this;
        }
        
        if (!InitialChecks())
        {
            Debug.LogError("Problems occurred during the initial check. Please, fix them.");
            Application.Quit();
        }

        InitialConfiguration();
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateArena();
        }

        if (spawnedMonsters <= 0.0f)
        {
            //RemoveSpawnedMonsters();
            //ConfigureWave();
            //StartCoroutine(MonsterSpawnCoroutine());
        }
    }

    protected bool InitialChecks()
    {
        if (arenaGenerators.Count == 0)
        {
            Debug.Log("No arena generators in the list. Add any before you can proceed.");
            return false;
        }

        return true;
    }

    protected void InitialConfiguration()
    {
        arenaSizeLimits.x = Mathf.Clamp(arenaSizeLimits.x, arenaSizeLimitsDefault.x, arenaSizeLimitsDefault.y);
        arenaSizeLimits.y = Mathf.Clamp(arenaSizeLimits.y, arenaSizeLimitsDefault.x, arenaSizeLimitsDefault.y);
    }

    public void StartGame()
    {
        GenerateArena();
        ConfigureWave();
        StartCoroutine(MonsterSpawnCoroutine());
    }

    protected void GenerateArena()
    {
        if (!hasGeneratedArena)
        {
            GetComponent<ArenaWaveController>().Controller.Config.InitCollection();
            CreateRootArenaObject();
            activeGenerator = Instantiate(arenaGenerators[UnityEngine.Random.Range(0, arenaGenerators.Count)]);
            arenaSizes = new Vector2Int (GetLimitValue(), GetLimitValue());
            activeGenerator.GenerateArena(arenaObject);
            spawnPointPlayer = activeGenerator.GeneratePlayerSpawnPoint();
            spawnPointMonsters = activeGenerator.GenerateMonsterSpawnPoints();
            StaticBatchingUtility.Combine(arenaObject.GetComponentInChildren<ArenaConstructionObject>().gameObject);
            SetPlayerObject();
            hasGeneratedArena = true;
        }
        else
        {
            RemoveRootArenaObject();
            RemoveSpawnedCharacters();
            Destroy(activeGenerator.gameObject);
            hasGeneratedArena = false;
            GenerateArena();
        }
    }

    protected int GetLimitValue()
    {
        return UnityEngine.Random.Range(arenaSizeLimits.x, arenaSizeLimits.y + 1);
    }

    protected void CreateRootArenaObject()
    {
        arenaObject = new GameObject(ArenaRootObject.ObjectName).AddComponent<ArenaRootObject>();
        ArenaConstructionObject aCO = new GameObject(ArenaConstructionObject.ObjectName).AddComponent<ArenaConstructionObject>();
        aCO.transform.SetParent(arenaObject.transform);
        Rigidbody acoRB = aCO.AddComponent<Rigidbody>();
        acoRB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        acoRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        acoRB.useGravity = false;
    }

    protected void RemoveRootArenaObject()
    {
        Destroy(arenaObject.gameObject);
    }

    protected void RemoveSpawnedCharacters()
    {
        RemoveSpawnedMonsters();
        RemoveSpawnedPlayer();
    }

    protected void RemoveSpawnedMonsters()
    {
        foreach (SpawnableMonster spawnedCharacter in FindObjectsOfType<SpawnableMonster>())
        {
            Destroy(spawnedCharacter.gameObject);
        }
    }

    protected void RemoveSpawnedPlayer()
    {
        Destroy(FindObjectOfType<SpawnablePlayer>().gameObject);
    }
    
    protected void SetPlayerObject()
    {
        spawnPointPlayer.SetClass(ArenaResourceManager.Manager.PlayerClassesList.GetRandomKey());
        spawnPointPlayer.SpawnCharacter(spawnPointPlayer.transform.position);
        player = FindObjectOfType<PlayerController>();
        playerBase = player.GetComponent<SpawnablePlayer>();
    }

    protected void ConfigureWave()
    {
        WaveController.PrepareWave();
    }

    protected void SetAiSpawnParams()
    {
        toSpawn = WaveController.WaveMonsters.SliceFromStart(maxMonstersPerTime);
        aiSpawnPositons = ArenaJobManager.Manager.GetAiSpawnPositions(toSpawn.Count);
    }

    protected IEnumerator MonsterSpawnCoroutine()
    {
        while (WaveController.WaveMonsters.Count > 0 || toSpawn.Count > 0)
        {
            if (toSpawn.Count < 1)
            {
                SetAiSpawnParams();
            }

            bool canSpawn = (FindObjectsOfType<SpawnableMonster>().Length <= maxMonstersPerTime && maxMonstersPerTime > 0) || maxMonstersPerTime < 1;

            if (aiSpawnPositons.Count > 0 && toSpawn.Count > 0 && canSpawn)
            {
                SpawnPointMonster point = GetRandomMonsterSpawnpoint();
                int spawnAmount = toSpawn.Count <= 10 ? toSpawn.Count : 10;
                for (int i = 0; i < spawnAmount; i++)
                {
                    Vector3 position = aiSpawnPositons[aiSpawnPositons.Count - 1];
                    string key = toSpawn[toSpawn.Count - 1];
                    if (key != null)
                    {
                        point.SetClass(key);
                        point.SpawnCharacter(position);
                    }
                    aiSpawnPositons.RemoveAt(aiSpawnPositons.Count - 1);
                    toSpawn.RemoveAt(toSpawn.Count - 1);
                }
            }
            yield return new WaitForSeconds(1f);
        }

        if (FindObjectsOfType<SpawnableMonster>().Length == 0)
        {
            ConfigureWave();
        }        
        yield return new WaitForSeconds(5f);
        StartCoroutine(MonsterSpawnCoroutine());

    }

    protected SpawnPointMonster GetRandomMonsterSpawnpoint()
    {
        return spawnPointMonsters[UnityEngine.Random.Range(0, spawnPointMonsters.Count)];
    }

    protected bool SpawnRandomMonsterOfDifficultyLevel(MonsterDifficultyLevels diffLevel, Vector3 position)
    {
        SpawnPointMonster point = GetRandomMonsterSpawnpoint();
        string key = ArenaResourceManager.Manager.MonsterClassesList.GetRandomKeyOfDifficulty(diffLevel);
        if (key != null)
        {
            point.SetClass(key);
            point.SpawnCharacter(position);
            return true;
        }

        return false;        
    }

    public void DecreaseSpawnedAmount()
    {
        spawnedMonsters--;
    }


}
