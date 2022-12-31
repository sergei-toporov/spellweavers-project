using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArenaGeneratorBase : MonoBehaviour
{
    /// <summary>
    /// A list of construction kits to use for building of the arena.
    /// </summary>
    [Tooltip("A list of prefab sets for generator.")]
    [SerializeField] protected List<ArenaConstructionKit> constructionKits;

    /// <summary>
    /// Generates an arena and puts it into the root object.
    /// </summary>
    /// <param name="rootObject">
    /// Root object for the arena elements.
    /// </param>
    public abstract void GenerateArena(ArenaRootObject rootObject);

    /// <summary>
    /// Generates monster spawn points on the arena.
    /// </summary>
    /// <param name="rootObject">
    /// Root object for the arena elements.
    /// </param>
    /// <returns>
    /// A list with spawnpoints for monsters.
    /// </returns>
    public abstract List<SpawnPointMonster> GenerateMonsterSpawnPoints();

    /// <summary>
    /// Generates player's spawn point on the arena.
    /// </summary>
    /// <param name="rootObject">
    /// Root object for the arena elements.
    /// </param>
    /// <returns>
    /// A player's spawnpoint instance.
    /// </returns>
    public abstract SpawnPointPlayer GeneratePlayerSpawnPoint();

    /// <summary>
    /// {@inheritdoc}
    /// </summary>
    protected virtual void Awake()
    {
        if (!InitialCheck())
        {
            Debug.LogError("This generator couldn't pass the initial check. Cannot proceed further.");
            Application.Quit();
        }
    }

    /// <summary>
    /// Checks some settings for their validity.
    /// </summary>
    /// <returns>
    /// TRUE - everything is OK;
    /// FALSE - some errors occurred.
    /// </returns>
    protected virtual bool InitialCheck()
    {
        if (constructionKits.Count == 0)
        {
            Debug.LogError("No construction sets in the generator.");
            return false;
        }
        return true;
    }
}
