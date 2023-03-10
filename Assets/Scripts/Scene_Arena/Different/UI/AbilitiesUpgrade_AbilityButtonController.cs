using Spellweavers;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesUpgrade_AbilityButtonController : MonoBehaviour
{
    protected AbilitiesUpgrade_AbilityButtonController controller;
    public AbilitiesUpgrade_AbilityButtonController Controller { get => controller ?? GetComponent<AbilitiesUpgrade_AbilityButtonController>(); }

    protected AbilitiesUpgradeScreen_CharacterStatsPaneController charStatsPane;

    protected Button button;
    [SerializeField] protected string UpdateUIEvent = "UI_UpdateSkillsUpgradeScreen";

    protected TextMeshProUGUI abilityName;
    protected TextMeshProUGUI abilityCurrentLevelValue;
    protected TextMeshProUGUI abilityUpgradeCostValue;

    protected PlayerAbility ability;
    protected string abilityKey;

    protected void OnEnable()
    {
        button = GetComponent<Button>();
        abilityName = GetComponentInChildren<AbilityButton_AbilityName>().GetComponent<TextMeshProUGUI>();
        abilityCurrentLevelValue = GetComponentInChildren<AbilityButton_CurrentLevelValue>().GetComponent<TextMeshProUGUI>();
        abilityUpgradeCostValue = GetComponentInChildren<AbilityButton_NextLevelCostValue>().GetComponent<TextMeshProUGUI>();
        button.onClick.AddListener(OnClickListener);
        charStatsPane = FindObjectOfType<AbilitiesUpgradeScreen_CharacterStatsPaneController>();
        SetInteractibility();
    }

    protected void Update()
    {
        /**
         * Find out why it scaled way too much
         */
        if (transform.localScale.x < 0.5f)
        {
            transform.localScale = new Vector3(0.98f, 0.98f, 0.98f);
        }
    }

    protected void OnDisable()
    {
        button.onClick.RemoveListener(OnClickListener);
    }

    public void AssignAbility(string key)
    {
        abilityKey = key;
        ProcessAbilityData();
    }

    protected void ProcessAbilityData()
    {
        ability = ArenaManager.Manager.Player.GetPlayerAbility(abilityKey);

        SetInteractibility();
        abilityName.text = ability.name;
        abilityCurrentLevelValue.text = $"{ability.currentLevel}";
        abilityUpgradeCostValue.text = $"{ability.currentImprovementCost}";
    }

    protected void OnClickListener()
    {
        ArenaManager.Manager.Player.AddAbility(abilityKey);
        ProcessAbilityData();
        EventsDispatcher.Dispatcher.Dispatch(UpdateUIEvent);
    }

    protected void SetInteractibility()
    {
        button.interactable = (ability.currentImprovementCost <= ArenaResourceManager.Manager.ResourcesToSpend);
    }
}
