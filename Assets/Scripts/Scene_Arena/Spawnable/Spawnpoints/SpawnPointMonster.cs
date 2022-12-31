using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointMonster : SpawnPointBase
{
    public override void SetClass(string key)
    {
        if (ArenaResourceManager.Manager.MonsterClassesList.Collection.TryGetValue(key, out CharacterClassMetadata charClass))
        {
            spawnableClass = charClass;
        }
    }
}
