using System;
using UnityEngine;

namespace Spellweavers
{
    [CreateAssetMenu(fileName = "new_arena_wave_config", menuName = "Custom Assets/Arena Assets/Arena Wave Config", order = 50)]
    public class ArenaWaveConfigSO : ScriptableObject
    {
        [Serializable]
        public struct WaveData
        {
            public int startAmountOfMonsters;
            public int maxAmountOfMonsters;
            public int currentAmountOfMonsters;
            public int increaseNumberOnWave;
        }
        [SerializeField] protected GenericDictionary<MonsterDifficultyLevels, WaveData> collection = new GenericDictionary<MonsterDifficultyLevels, WaveData>();
        public GenericDictionary<MonsterDifficultyLevels, WaveData> Collection => collection;

        protected void OnEnable()
        {
            InitCollection();
        }

        public void UpdateWaveConfig(int waveNumber)
        {
            GenericDictionary<MonsterDifficultyLevels, WaveData> tmpCollection = new GenericDictionary<MonsterDifficultyLevels, WaveData>();
            foreach (MonsterDifficultyLevels key in collection.Keys)
            {
                var value = collection[key];
                if (waveNumber % collection[key].increaseNumberOnWave == 0 && (collection[key].currentAmountOfMonsters < collection[key].maxAmountOfMonsters))
                {
                    value.currentAmountOfMonsters++;
                }                
                
                tmpCollection.Add(key, value);
            }
            collection = tmpCollection;
        }

        public void InitCollection()
        {
            GenericDictionary<MonsterDifficultyLevels, WaveData> tmpCollection = new GenericDictionary<MonsterDifficultyLevels, WaveData>();
            foreach (MonsterDifficultyLevels key in collection.Keys)
            {
                var value = collection[key];
                value.currentAmountOfMonsters = value.startAmountOfMonsters;
                tmpCollection.Add(key, value);
            }
            collection = tmpCollection;
        }
    }
}

