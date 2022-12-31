using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointPlayer : SpawnPointBase
{
    public override void SetClass(string key)
    {
        if (ArenaResourceManager.Manager.PlayerClassesList.Collection.TryGetValue(key, out CharacterClassMetadata charClass))
        {
            spawnableClass = charClass;
        }
    }
}
