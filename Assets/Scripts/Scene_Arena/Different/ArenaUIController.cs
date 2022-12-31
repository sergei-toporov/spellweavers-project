using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaUIController : MonoBehaviour
{
    [SerializeField] protected ArenaUIController controller;
    public ArenaUIController Controller { get => controller ?? GetComponent<ArenaUIController>(); }

    [SerializeField] protected RectTransform ingameUI;
    public RectTransform IngameUI { get => ingameUI; }

    [SerializeField] protected RectTransform featsUpgradeUI;
    public RectTransform FeatsUpgradeUI { get => featsUpgradeUI; }

    [SerializeField] protected RectTransform pauseScreenUI;
    public RectTransform PauseScreenUI { get => pauseScreenUI; }

    [SerializeField] protected RectTransform deathScreenUI;
    public RectTransform DeathScreenUI { get => deathScreenUI; }

    protected void Awake()
    {
        if (!InitialCheck())
        {
            Debug.LogError($"Errors occured during the initial check of the '{this.name}'. Can't proceed further");
        }
    }

    protected bool InitialCheck()
    {
        if (ingameUI == null)
        {
            Debug.LogError($"The 'IngameUI' element is not set.");
            return false;
        }

        if (featsUpgradeUI == null)
        {
            Debug.LogError($"The 'Feats Upgrade UI' element is not set.");
            return false;
        }

        if (pauseScreenUI == null)
        {
            Debug.LogError($"The 'Pause Screen UI' element is not set.");
            return false;
        }

        if (deathScreenUI == null)
        {
            Debug.LogError($"The 'Death Screen UI' element is not set.");
            return false;
        }

        return true;
    }

    public void SwitchUI()
    {
        switch (ArenaWorkflowManager.Manager.ArenaState)
        {
            case ArenaStates.Ingame:
                DisableUI();
                EnableUIScreen(ingameUI);
                break;
            case ArenaStates.FeatsUpgradeScreen:
                DisableUI();
                EnableUIScreen(featsUpgradeUI);
                break;
            case ArenaStates.PauseScreen:
                DisableUI();
                EnableUIScreen(pauseScreenUI);
                break;
            case ArenaStates.DeathScreen:
                DisableUI();
                EnableUIScreen(deathScreenUI);
                break;
        }
    }

    public void UpdateUI()
    {
        switch (ArenaWorkflowManager.Manager.ArenaState)
        {
            case ArenaStates.Ingame:
                IngameScreen_PlayerResourcesValue resValue = ingameUI.GetComponentInChildren<IngameScreen_PlayerResourcesValue>();
                if (resValue != null)
                {
                    resValue.UpdateText();
                }

                break;
        }
    }

    protected void DisableUI()
    {
        ingameUI.gameObject.SetActive(false);
        pauseScreenUI.gameObject.SetActive(false);
        featsUpgradeUI.gameObject.SetActive(false);
    }

    protected void EnableUIScreen(RectTransform uiScreen)
    {
        uiScreen.gameObject.SetActive(true);
    }
}
