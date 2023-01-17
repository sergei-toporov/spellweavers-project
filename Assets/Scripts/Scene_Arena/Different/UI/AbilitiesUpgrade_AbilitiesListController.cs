using UnityEngine;

public class AbilitiesUpgrade_AbilitiesListController : MonoBehaviour
{
    [SerializeField] protected AbilitiesUpgrade_AbilitiesListController controller;
    public AbilitiesUpgrade_AbilitiesListController Controller { get => controller ?? GetComponent<AbilitiesUpgrade_AbilitiesListController>(); }

    [SerializeField] protected AbilitiesUpgrade_AbilityButtonController buttonPrefab;
    public AbilitiesUpgrade_AbilityButtonController ButtonPrefab { get => buttonPrefab; }

    [SerializeField] protected AbilitiesListContent buttonsBlock;
    public AbilitiesListContent ButtonsBlock { get => buttonsBlock; }

    protected void Awake()
    {
        if (buttonsBlock == null)
        {
            buttonsBlock = GetComponentInChildren<AbilitiesListContent>();
        }
        foreach (string key in ArenaResourceManager.Manager.PlayerAbilitiesList.Collection.Keys)
        {
            if (ArenaResourceManager.Manager.PlayerAbilitiesList.Collection[key].isActive)
            {
                AbilitiesUpgrade_AbilityButtonController button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
                button.transform.SetParent(buttonsBlock.transform);
                button.AssignAbility(key);
            }            
        }
    }

    public void RefreshList()
    {
        buttonsBlock.gameObject.SetActive(false);
        buttonsBlock.gameObject.SetActive(true);
    }
}
