using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A list of stats that can be affected.
/// </summary>
public enum AffectedStat
{
    Health,
    HealthBase,
    Mana,
    ManaBase,
    MovemementSpeed,
    MovementSpeedBase,
    AttacksPerMinute,
    AttacksPerMinuteBase,
    AttackRange,
    AttackRangeBase,
    Damage,
    DamageBase,
    DamageRadius,
    DamageRadiusBase,
    HealthRegen,
    HealthRegenBase,
    ManaRegen,
    ManaRegenBase,
}

[Serializable]
public struct PlayerAbility
{
    public bool isActive;
    public bool changesAbsoluteValue;
    public string name;
    public AffectedStat affectedStat;
    public int currentLevel;
    public int maxLevel;
    public int improvementCostBase;
    public int currentImprovementCost;
    public int changeValue;    
}

[CreateAssetMenu(fileName = "new_player_abilities_list", menuName = "Custom Assets/Player abilities/Abilities list", order = 54)]
public class PlayerAbilitiesSO : ScriptableObject
{
    [SerializeField] protected GenericDictionary<string, PlayerAbility> collection = new GenericDictionary<string, PlayerAbility>();
    public GenericDictionary<string, PlayerAbility> Collection { get => collection; }

    public PlayerAbility GetItemByKey(string key)
    {
        if (collection.TryGetValue(key, out PlayerAbility feat))
        {
            return feat;
        }

        throw new KeyNotFoundException($"The '{key}' item is not in the collection");
    }
}
