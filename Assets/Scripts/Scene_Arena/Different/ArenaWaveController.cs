using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spellweavers;

namespace Spellweavers
{
    public class ArenaWaveController : MonoBehaviour
    {
        [SerializeField] protected ArenaWaveController controller;
        public ArenaWaveController Controller { get => controller ?? GetComponent<ArenaWaveController>(); }

        [SerializeField] protected ArenaWaveConfigSO config;
        public ArenaWaveConfigSO Config { get => config; }

        protected int waveNumber;
        public int WaveNumber { get => waveNumber; }

        [SerializeField] protected List<string> waveMonsters;
        public List<string> WaveMonsters { get => waveMonsters; }

        [SerializeField] protected List<string> waveMonstersClean;

        public void PrepareWave()
        {

            waveMonsters = new List<string>();
            waveMonstersClean = new List<string>();
            foreach (MonsterDifficultyLevels diffLvl in config.Collection.Keys)
            {
                if (diffLvl != MonsterDifficultyLevels.Boss || (diffLvl == MonsterDifficultyLevels.Boss && waveNumber % 10 == 0 && waveNumber > 0))
                {
                    for (int i = 0; i < config.Collection[diffLvl].currentAmountOfMonsters; i++)
                    {
                        string key = ArenaResourceManager.Manager.MonsterClassesList.GetRandomKeyOfDifficulty(diffLvl);
                        waveMonsters.Add(key);
                    }
                }
            }
            waveMonsters.Shuffle();
            
            foreach (string item in waveMonsters)
            {
                waveMonstersClean.Add(item);
            }

            waveNumber++;
            config.UpdateWaveConfig(waveNumber);
        }
    }

}
