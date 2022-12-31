using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    protected static ArenaManager manager;
    public static ArenaManager Manager { get => manager; }

    protected readonly Vector2Int arenaSizeLimitsDefault = new(6, 80);

    [Tooltip("A border values for the arena size in one dimension. These values will be used to get amount of rows and columns. If any value is lesser or bigger than the default, it'll be reset to one of the default limits.")]
    
    [SerializeField] protected Vector2Int arenaSizeLimits;

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

    protected List<SpawnPointMonster> spawnPointMonsters;

    protected SpawnPointPlayer spawnPointPlayer;

    [Serializable]
    public struct WaveConfig
    {
        public int lightMobsAmount;
        public int midMobsAmount;
        public int hardMobsAmount;
        public int bossesAmount;
        [HideInInspector] public int spawnedLightMobsAmount;
        [HideInInspector] public int spawnedMidMobsAmount;
        [HideInInspector] public int spawnedHardMobsAmount;
        [HideInInspector] public int spawnedBossesAmount;
    }

    [SerializeField] protected WaveConfig waveConfig;

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
        //GenerateArena();
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateArena();
        }

        if (spawnedMonsters <= 0.0f)
        {
            RemoveSpawnedMonsters();
            ConfigureWave();
            StartCoroutine(MonsterSpawnCoroutine());
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
        spawnPointPlayer.SpawnCharacter();
        player = FindObjectOfType<PlayerController>();
        playerBase = player.GetComponent<SpawnablePlayer>();
    }

    protected void ConfigureWave()
    {
        if (waveNumber > 0)
        {
            waveConfig.lightMobsAmount++;

            if (waveNumber % 4 == 0)
            {
                waveConfig.midMobsAmount++;
            }

            if (waveNumber % 8 == 0)
            {
                waveConfig.hardMobsAmount++;
            }

            if (waveNumber % 20 == 0)
            {
                waveConfig.bossesAmount++;
            }
        }

        waveConfig.spawnedLightMobsAmount = 0;
        waveConfig.spawnedMidMobsAmount = 0;
        waveConfig.spawnedHardMobsAmount = 0;
        waveConfig.spawnedBossesAmount = 0;

        spawnedMonsters = 0;
        totalToSpawn = waveConfig.lightMobsAmount + waveConfig.midMobsAmount + waveConfig.hardMobsAmount + waveConfig.bossesAmount;
        waveNumber++;
    }

    protected IEnumerator MonsterSpawnCoroutine()
    {
        while (spawnedMonsters < totalToSpawn)
        {
            if (waveConfig.spawnedLightMobsAmount < waveConfig.lightMobsAmount)
            {
                if (SpawnRandomMonsterOfDifficultyLevel(MonsterDifficultyLevels.Easy))
                {
                    waveConfig.spawnedLightMobsAmount++;
                    spawnedMonsters++;
                }                
            }

            if (waveConfig.spawnedMidMobsAmount < waveConfig.midMobsAmount)
            {
                if ((waveConfig.spawnedLightMobsAmount == waveConfig.lightMobsAmount) || waveConfig.spawnedLightMobsAmount % 3 == 0)
                {
                    if (SpawnRandomMonsterOfDifficultyLevel(MonsterDifficultyLevels.Medium))
                    {
                        waveConfig.spawnedMidMobsAmount++;
                        spawnedMonsters++;
                    }                    
                }
            }

            if (waveConfig.spawnedHardMobsAmount < waveConfig.hardMobsAmount)
            {
                if ((waveConfig.spawnedLightMobsAmount == waveConfig.lightMobsAmount) || waveConfig.spawnedLightMobsAmount % 6 == 0)
                {
                    if (SpawnRandomMonsterOfDifficultyLevel(MonsterDifficultyLevels.Hard))
                    {
                        waveConfig.spawnedHardMobsAmount++;
                        spawnedMonsters++;
                    }                    
                }


                if (waveConfig.spawnedBossesAmount < waveConfig.bossesAmount)
                {
                    if ((waveConfig.spawnedLightMobsAmount == waveConfig.lightMobsAmount) || waveConfig.spawnedLightMobsAmount % 10 == 0)
                    {
                        if (SpawnRandomMonsterOfDifficultyLevel(MonsterDifficultyLevels.Boss))
                        {
                            waveConfig.spawnedBossesAmount++;
                            spawnedMonsters++;
                        }                        
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    protected SpawnPointMonster GetRandomMonsterSpawnpoint()
    {
        return spawnPointMonsters[UnityEngine.Random.Range(0, spawnPointMonsters.Count)];
    }

    protected bool SpawnRandomMonsterOfDifficultyLevel(MonsterDifficultyLevels diffLevel)
    {
        SpawnPointMonster point = GetRandomMonsterSpawnpoint();
        string key = ArenaResourceManager.Manager.MonsterClassesList.GetRandomKeyOfDifficulty(diffLevel);
        if (key != null)
        {
            point.SetClass(key);
            point.SpawnCharacter();
            return true;
        }

        return false;        
    }

    public void DecreaseSpawnedAmount()
    {
        spawnedMonsters--;
    }


}
