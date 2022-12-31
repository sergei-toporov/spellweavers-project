using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] protected CameraController cam;
    [SerializeField] protected bool isCameraBooted = false;

    protected void Awake()
    {
        cam = FindObjectOfType<CameraController>();
    }


    protected void LateUpdate()
    {
        transform.LookAt(transform.position + cam.transform.forward);   
    }
}
