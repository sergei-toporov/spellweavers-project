using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaUI_AttackButton : ArenaUI_ButtonsGeneralController
{
    
    protected override void OnClickListener()
    {
        ArenaManager.Manager.Player.Attack();
    }
}
