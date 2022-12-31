using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassesManager : MonoBehaviour
{
    protected static ClassesManager manager;
    public static ClassesManager Manager { get => manager; }

    [SerializeField] protected ClassesListPlayerSO playerClasses;
    public ClassesListPlayerSO PlayerClasses { get => playerClasses; }
    
    [SerializeField] protected ClassesListMonsterSO monsterClasses;
    public ClassesListMonsterSO MonsterClasses { get => monsterClasses; }

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
            Debug.LogError("The initial check not passed. Check the logs and fix errors");
            Application.Quit();
        }
    }


    protected bool InitialCheck()
    {
        return true;
    }
}
