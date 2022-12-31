using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CollectibleStuff
{
    public bool isActive;
    public bool isConstant;
    public bool affectsCharacterStat;
    public bool overTimeEffect;
    public bool hasAbsoluteEffect;
    public AffectedStat affectedStat;
    public float effectAmount;
    public float effectTime;
    public Transform prefab;
    public Image image;
    public float dropChance;
}

[CreateAssetMenu(fileName = "new_collectible_stuff_list", menuName = "Custom Assets/Collectibles/Collectible Stuff List", order = 54)]
public class CollectibleStuffSO : ScriptableObject
{
    [SerializeField] protected GenericDictionary<string, CollectibleStuff> collection = new GenericDictionary<string, CollectibleStuff>();
    public GenericDictionary<string, CollectibleStuff> Collection { get => collection; }

    public CollectibleStuff GetRandomItem()
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
            return collection[keys[UnityEngine.Random.Range(0, collectionCount)]];
        }

        throw new KeyNotFoundException($"The collection is empty");
    }

    public CollectibleStuff GetRandomItemByKey(string key)
    {
        if (collection.TryGetValue(key, out CollectibleStuff stuffUnit))
        {
            return stuffUnit;
        }

        throw new KeyNotFoundException($"The '{key}' class is not in collection");
    }
}
