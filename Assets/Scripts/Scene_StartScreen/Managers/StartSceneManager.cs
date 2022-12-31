using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StartSceneStatuses
{
    OnStartTapScreen,
    OnProfilesListScreen,
    OnProfileDetailsScreen,
}

public class StartSceneManager : MonoBehaviour
{
    protected static StartSceneManager manager;
    public static StartSceneManager Manager { get => manager; }

    [SerializeField] protected StartSceneStatuses sceneStatus;
    public StartSceneStatuses SceneStatus { get => sceneStatus; }

    [SerializeField] protected TapToStartController tapToStartScreen;

    [SerializeField] protected ScenesListSO scenesList;

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

        if (!InitialCheck())
        {
            Debug.LogError("Errors occurred during the initial check. Can't proceed further before errors are fixed.");
            Application.Quit();
        }
        sceneStatus = StartSceneStatuses.OnStartTapScreen;
        InitializeUI();    
       
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            ReactOnTouch();
        }
    }

    protected void ReactOnTouch()
    {
        switch (sceneStatus)
        {
            case StartSceneStatuses.OnStartTapScreen:
                SceneManager.LoadScene(scenesList.GetSceneNameByKey("ArenaScene"));
                break;
        }
    }

    protected bool InitialCheck()
    {
        if (tapToStartScreen == null)
        {
            Debug.LogError("The 'TapToStartScreen' parameter is not set. Can't proceed without it.");
            return false;
        }

        if (scenesList == null || scenesList.ScenesList.Count == 0)
        {
            Debug.LogError("The 'Scenes List' either wasn't added or is empty");
            Application.Quit();
        }

        return true;
    }

    protected void InitializeUI()
    {
        tapToStartScreen.StartFlashing();
    }
}
