using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaResourceManager : MonoBehaviour
{

    protected static ArenaResourceManager manager;
    public static ArenaResourceManager Manager { get => manager; }

    protected bool isReady = false;
    public bool IsReady { get => isReady; }

    [SerializeField] protected ScenesListSO sceneList;
    public ScenesListSO SceneList { get => sceneList; }

    [SerializeField] protected PlayerAbilitiesSO playerAbilitiesList;
    public PlayerAbilitiesSO PlayerAbilitiesList { get => playerAbilitiesList; }

    [SerializeField] protected GenericDictionary<string, PlayerAbility> availablePlayerAbilities = new GenericDictionary<string, PlayerAbility>();
    public GenericDictionary<string, PlayerAbility> AvailablePlayerAbilities { get => availablePlayerAbilities; }

    [SerializeField] protected SpawnPointPlayer spawnpointPlayerPrefab;
    public SpawnPointPlayer SpawnpointPlayerPrefab { get => spawnpointPlayerPrefab; }

    [SerializeField] protected SpawnPointMonster spawnpointMonsterPrefab;
    public SpawnPointMonster SpawnpointMonsterPrefab { get => spawnpointMonsterPrefab; }

    [SerializeField] protected ClassesListPlayerSO playerClassesList;
    public ClassesListPlayerSO PlayerClassesList { get => playerClassesList; }

    [SerializeField] protected ClassesListMonsterSO monsterClassesList;
    public ClassesListMonsterSO MonsterClassesList { get => monsterClassesList; }

    [SerializeField] protected CollectibleResourcesSO collectibleResourcesList;
    public CollectibleResourcesSO CollectibleResourcesList { get => collectibleResourcesList; }

    [SerializeField] protected CollectibleStuffSO collectibleStuffList;
    public CollectibleStuffSO CollectibleStuffList { get => collectibleStuffList; }

    [SerializeField] protected List<CollectibleResource> availavleCollectibleResources;
    public List<CollectibleResource> AvailableCollectibleResources { get => availavleCollectibleResources; }

    [SerializeField] protected List<CollectibleStuff> availableCollectibleStuff;
    public List<CollectibleStuff> AvailableCollectibleStuff { get => availableCollectibleStuff; }

    protected float resourcesToSpend = 0.0f;
    public float ResourcesToSpend { get => resourcesToSpend; }

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

        if (!InitialCheck())
        {
            Debug.LogError("Errors occurred during the initial check. Fix before comtinue.");
            Application.Quit();
        }

        SetAvailableResources();

        isReady = true;
    }

    protected bool InitialCheck()
    {
        if (sceneList == null || sceneList.ScenesList.Count == 0)
        {
            Debug.LogError("The scenes list collection either is not set or is empty.");
            return false;
        }

        if (playerAbilitiesList == null || playerAbilitiesList.Collection.Count == 0)
        {
            Debug.LogError("The player abilities collection either is not set or is empty.");
            return false;
        }

        if (spawnpointPlayerPrefab == null)
        {
            Debug.LogError("The player spawnpoint prefab is empty.");
            return false;
        }

        if (spawnpointMonsterPrefab == null)
        {
            Debug.LogError("The monsters spawnpoint prefab is empty.");
            return false;
        }

        if (monsterClassesList == null || monsterClassesList.Collection.Count == 0)
        {
            Debug.LogError("The monster classes collection either is not set or is epmty.");
            return false;
        }

        if (playerClassesList == null || playerClassesList.Collection.Count == 0)
        {
            Debug.LogError("The player classes collection either is not set or is epmty.");
            return false;
        }

        if (collectibleResourcesList == null || collectibleResourcesList.Collection.Count == 0)
        {
            Debug.LogError("The collectible resources collection either is not set or is empty.");
            return false;
        }

        if (collectibleStuffList == null || collectibleStuffList.Collection.Count == 0)
        {
            Debug.LogError("The collectible stuff collection either is not set or is empty.");
            return false;
        }

        return true;
    }

    protected void SetAvailableResources()
    {        
        availableCollectibleStuff = new List<CollectibleStuff>(collectibleStuffList.Collection.Count);
        foreach (CollectibleStuff stuffUnit in collectibleStuffList.Collection.Values)
        {
            if (stuffUnit.isActive)
            {
                availableCollectibleStuff.Add(stuffUnit);
            }
        }
        
        availavleCollectibleResources = new List<CollectibleResource>(collectibleResourcesList.Collection.Count);
        foreach (CollectibleResource resourceUnit in collectibleResourcesList.Collection.Values)
        {
            if (resourceUnit.isActive)
            {
                availavleCollectibleResources.Add(resourceUnit);
            }
        }

        foreach (string abilityKey in PlayerAbilitiesList.Collection.Keys)
        {
            if (PlayerAbilitiesList.Collection[abilityKey].isActive)
            {
                availablePlayerAbilities.Add(abilityKey, PlayerAbilitiesList.Collection[abilityKey]);
            }
        }
    }

    public CollectibleStuff GetRandomCollectibleStuff()
    {
        return availableCollectibleStuff[Random.Range(0, availableCollectibleStuff.Count)];
    }

    public void SpawnCollectibleStuff(CollectibleStuff unitData, Vector3 spawnPosition)
    {
        Transform spawnedUnit = Instantiate(unitData.prefab, spawnPosition, Quaternion.identity);
        CollectibleStuffUnit collectible = spawnedUnit.GetComponent<CollectibleStuffUnit>();
        collectible.SetParameters(unitData);
    }

    public void SpawnCollectibleResourcesMandatory(Vector3 spawnPosition) {
        foreach (CollectibleResource res in AvailableCollectibleResources)
        {
            if (res.isMandatory)
            {
                Transform spawnedResource = Instantiate(res.prefab, spawnPosition, res.prefab.rotation);
                spawnedResource.GetComponent<CollectibleResourceUnit>().SetParameters(res);
            }
        }
    }

    public void AddPlayerResources(CollectibleResourceUnit stuffUnit)
    {
        resourcesToSpend += stuffUnit.UnitData.resourceAmountBase;
        ArenaWorkflowManager.Manager.UpdateArenaUI();
    }

    public bool ChargeForAbilityUpgrade(PlayerAbility ability)
    {
        if (resourcesToSpend >= ability.currentImprovementCost)
        {
            resourcesToSpend -= ability.currentImprovementCost;
            ArenaWorkflowManager.Manager.UpdateArenaUI();
            return true;
        }
        else
        {
            return false;
        }
    }
}
