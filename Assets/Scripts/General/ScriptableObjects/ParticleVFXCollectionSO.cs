using System;
using UnityEngine;

namespace Spellweavers
{
    [Serializable]
    public struct ParticleVFXItem
    {
        public ParticleSystem prefab;
    }

    [CreateAssetMenu(fileName = "new_particle_vfx_list", menuName = "Custom Assets/VFX/Particle VFX List", order = 55)]
    public class ParticleVFXCollectionSO : ScriptableObject
    {
        [SerializeField] protected GenericDictionary<string, ParticleVFXItem> collection = new GenericDictionary<string, ParticleVFXItem>();
        public GenericDictionary<string, ParticleVFXItem> Collection { get => collection; }

        protected void OnEnable()
        {
            if (!collection.ContainsKey(ProjectStaticVariables.CollectionEmptyItemKey))
            {
                ParticleVFXItem item = new ParticleVFXItem();
                item.prefab = new ParticleSystem();
                collection.Add(ProjectStaticVariables.CollectionEmptyItemKey, item);
            }
        }
        public ParticleVFXItem GetItemByKey(string key)
        {
            if (collection.TryGetValue(key, out ParticleVFXItem item))
            {
                return item;
            }
            return collection[ProjectStaticVariables.CollectionEmptyItemKey];
        }

        public ParticleVFXItem GetRandomItem()
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
            return collection[ProjectStaticVariables.CollectionEmptyItemKey];
        }
    }
}

