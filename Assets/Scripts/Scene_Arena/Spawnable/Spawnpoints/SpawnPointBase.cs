using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointBase : MonoBehaviour
{
    protected CharacterClassMetadata spawnableClass;

    protected void Awake()
    {
        if (!InitialCheck())
        {
            Debug.LogError("Errors occurred during initial checks. See logs.");
            Application.Quit();
        }
    }

    protected bool InitialCheck()
    {
        return true;
    }

    public virtual void SetClass(string key)
    {
        
    }

    public void SpawnCharacter()
    {
        if (spawnableClass.defaultPrefab != null) {
            SpawnableBase spawnedCharacter = Instantiate(spawnableClass.defaultPrefab, transform.position, Quaternion.identity);
            spawnedCharacter.AddBaseStats(spawnableClass);
        }
        
    }

}
