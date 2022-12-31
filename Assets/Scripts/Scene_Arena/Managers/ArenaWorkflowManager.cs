using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ArenaStates
{
    Initialization,
    InitializationError,
    Ingame,
    PauseScreen,
    FeatsUpgradeScreen,
    ReturningToStartScreen,
    QuittingGame,
    RestartGame,
    DeathScreen
}
public class ArenaWorkflowManager : MonoBehaviour
{

    protected static ArenaWorkflowManager manager;
    public static ArenaWorkflowManager Manager { get => manager; }

    protected ArenaStates arenaState;
    public ArenaStates ArenaState { get => arenaState; }

    protected ArenaUIController arenaUIController;

    protected int initTriesCounter = 0;
    protected int initTriesMax = 100;

    protected WaitForEndOfFrame EOF_Object = new();

    protected void Awake()
    {
        
        if (manager != this && manager != null)
        {
            Destroy(this);
        }
        else
        {
            manager = this;
        }
        SwitchState(ArenaStates.Initialization);
    }

    protected void Start()
    {
        arenaUIController = FindObjectOfType<ArenaUIController>();
        SwitchState(ArenaStates.Ingame);
        UpdateArenaUI();
        ArenaManager.Manager.StartGame();
    }

    public void SwitchState(ArenaStates state)
    {
        arenaState = state;
        switch (arenaState)
        {
            case ArenaStates.Initialization:
                ManagerInitialization();
                break;

            case ArenaStates.InitializationError:
                Debug.LogError("Errors occurred during initialization. Please, check logs.");
                Application.Quit();
                break;

            case ArenaStates.Ingame:
                arenaUIController.SwitchUI();
                PauseDisable();
                UpdateArenaUI();
                break;

            case ArenaStates.PauseScreen:
            case ArenaStates.FeatsUpgradeScreen:
            case ArenaStates.DeathScreen:
                arenaUIController.SwitchUI();
                PauseEnable();
                break;

            case ArenaStates.ReturningToStartScreen:
                SceneManager.LoadScene(ArenaResourceManager.Manager.SceneList.GetSceneNameByKey("StartScreenScene"));
                break;

            case ArenaStates.QuittingGame:
                Application.Quit();
                break;

            case ArenaStates.RestartGame:
                SceneManager.LoadScene(ArenaResourceManager.Manager.SceneList.GetSceneNameByKey("ArenaScene"));
                break;
        }
    }

    protected void PauseEnable()
    {
        Time.timeScale = 0.0f;
    }

    protected void PauseDisable()
    {
        Time.timeScale = 1.0f;
    }

    protected void ManagerInitialization()
    {
        StartCoroutine(InitArenaResourceManager());
        if (initTriesCounter == initTriesMax)
        {
            Debug.LogError("The 'Arena Resource Manager' has not been initialized.");
            SwitchState(ArenaStates.InitializationError);
        }
        initTriesCounter = 0;
    }

    protected IEnumerator InitArenaResourceManager()
    {
        while (initTriesCounter < initTriesMax || ArenaResourceManager.Manager == null || !ArenaResourceManager.Manager.IsReady) {
            initTriesCounter++;
            yield return EOF_Object;
        }
    }

    public void UpdateArenaUI()
    {
        arenaUIController.UpdateUI();
    }
    

}
