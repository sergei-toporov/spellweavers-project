using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContainerController : MonoBehaviour
{
    protected void LateUpdate()
    {        
        transform.position = new Vector3(ArenaManager.Manager.Player.transform.position.x, transform.position.y, ArenaManager.Manager.Player.transform.position.z);        
    }
}
