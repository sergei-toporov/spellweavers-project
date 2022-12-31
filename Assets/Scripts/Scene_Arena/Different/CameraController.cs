using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] protected bool isTraceable = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttachToPlayer());
        transform.position = new Vector3(0.0f, 14.0f, 6.0f);
    }

    protected void LateUpdate()
    {
        if (isTraceable)
        {
            transform.position = new Vector3(ArenaManager.Manager.Player.transform.position.x, transform.position.y, ArenaManager.Manager.Player.transform.position.z);
        }
    }

    protected IEnumerator AttachToPlayer()
    {
        if (ArenaManager.Manager.enabled != false || ArenaManager.Manager.Player == null)
        {
            yield return new WaitForEndOfFrame();
        }
        
        isTraceable = true;
    }   

   
}
