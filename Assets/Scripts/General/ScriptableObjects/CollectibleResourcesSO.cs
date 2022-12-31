using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CollectibleResource
{
    public bool isActive;
    public bool isMandatory;
    public float resourceAmountBase;
    public float dropChance;
    public Transform prefab;
    public Image image;
}

[CreateAssetMenu(fileName = "new_collectible_resources_list", menuName = "Custom Assets/Collectibles/Collectible Resources List", order = 54)]
public class CollectibleResourcesSO : ScriptableObject
{
    [SerializeField] protected GenericDictionary<string, CollectibleResource> collection = new GenericDictionary<string, CollectibleResource>();
    public GenericDictionary<string, CollectibleResource> Collection { get => collection; }

    public CollectibleResource GetRandomItem()
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

    public CollectibleResource GetItemByKey(string key)
    {
        if (collection.TryGetValue(key, out CollectibleResource resourceUnit))
        {
            return resourceUnit;
        }

        throw new KeyNotFoundException($"The '{key}' class is not in collection");
    }
}
