using System.Collections.Generic;
using UnityEngine;

public class AbilitiesUpgradeScreen_CharacterStatsPaneController : MonoBehaviour
{
    [SerializeField] protected AbilitiesUpgradeScreen_CharacterStatsPaneController controller;
    public AbilitiesUpgradeScreen_CharacterStatsPaneController Controller { get => controller ?? GetComponent<AbilitiesUpgradeScreen_CharacterStatsPaneController>(); }

    protected List<AbilitiesUpgradeScreen_CharacterStatValueBase> stats = new List<AbilitiesUpgradeScreen_CharacterStatValueBase>();

    protected void OnEnable()
    {
        if (stats.Count == 0)
        {
            foreach (AbilitiesUpgradeScreen_CharacterStatValueBase stat in GetComponentsInChildren<AbilitiesUpgradeScreen_CharacterStatValueBase>())
            {
                stats.Add(stat);
            }
        }
        UpdateStats();
    }

    public void UpdateStats()
    {
        foreach (AbilitiesUpgradeScreen_CharacterStatValueBase stat in stats)
        {
            stat.UpdateText();
        }
    }
}
