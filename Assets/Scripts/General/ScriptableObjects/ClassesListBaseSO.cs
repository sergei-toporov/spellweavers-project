using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public struct CharacterClassMetadata
{
    public string defaultName;
    public SpawnableBase defaultPrefab;
    public float baseHealth;
    public float baseMana;
    public float baseMovementSpeed;
    public float baseAttacksPerMinute;
    public float baseAttackRange;
    public float baseDamage;
    public float baseDamageRadius;
    public float baseHealthRegeneration;
    public float baseManaRegeneration;
    public WeaponHitter weaponHitterPrefab;
    public MonsterDifficultyLevels monsterDifficulty;

    public override string ToString()
    {
        string result = "";
        
        result += $"defName: {defaultName} / ";
        result += $"baseHP: {baseHealth} / ";
        result += $"baseHPR: {baseHealthRegeneration} / ";
        result += $"baseMP: {baseMana} / ";
        result += $"baseMPR: {baseManaRegeneration} / ";
        result += $"baseDamageRadius: {baseDamageRadius} / ";        

        return result;
    }
}

public class ClassesListBaseSO : ScriptableObject
{
    [SerializeField] protected GenericDictionary<string, CharacterClassMetadata> collection = new GenericDictionary<string, CharacterClassMetadata>();
    public GenericDictionary<string, CharacterClassMetadata> Collection { get => collection; }

    public string  GetRandomKey()
    {
        int collectionCount = collection.Count;
        if (collectionCount > 0)
        {
            string[] keys = new string[collectionCount];
            int counter = 0;
            foreach (string key in collection.Keys)
            {
                keys[counter] = key;
                counter++;
            }
            return keys[UnityEngine.Random.Range(0, collectionCount)];
        }

        return null;
    }

    public string GetRandomKeyOfDifficulty(MonsterDifficultyLevels diffLevel)
    {
        int collectionCount = collection.Count;
        if (collectionCount > 0)
        {
            List<string> keys = new List<string>();
            string result;
            int counter = 0;
            foreach (string key in collection.Keys)
            {
                if (collection[key].monsterDifficulty == diffLevel)
                {
                    keys.Add(key);
                    counter++;
                }
            }

            if (keys.Count == 0)
            {
                return null;
            }

            result = keys[UnityEngine.Random.Range(0, keys.Count)];
            return collection.TryGetValue(result, out CharacterClassMetadata metadata) ? result : null;
        }
        return null;
    }

    public CharacterClassMetadata GetRandomClass()
    {
        string key = GetRandomKey();

        if (collection.TryGetValue(key, out CharacterClassMetadata metadata))
        {
            return metadata;
        }

        throw new KeyNotFoundException($"The collection is empty");
    }

    public CharacterClassMetadata GetClassByKey(string key)
    {
        if (collection.TryGetValue(key, out CharacterClassMetadata characterClass))
        {
            return characterClass;
        }

        throw new KeyNotFoundException($"The '{key}' class is not in collection");
    }

}
