using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaUI_ButtonsGeneralController : MonoBehaviour
{
    protected Button button;

    [SerializeField] protected ArenaStates transitToState;

    protected void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickListener);
    }

    protected virtual void OnClickListener()
    {
        ArenaWorkflowManager.Manager.SwitchState(transitToState);
    }
}
