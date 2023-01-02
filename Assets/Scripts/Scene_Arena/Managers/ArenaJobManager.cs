using Spellweavers.ArenaScene;
using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ArenaJobManager : MonoBehaviour
{
    protected static ArenaJobManager manager;
    public static ArenaJobManager Manager { get => manager; }

    protected WaitForFixedUpdate delayObject;

    protected int cpuCoresAmount = 1; 

    protected void Awake()
    {
        if (manager != null && manager != this)
        {
            Destroy(this);
        }
        else
        {
            manager = this;
        }

        delayObject = new WaitForFixedUpdate();
        cpuCoresAmount = SystemInfo.processorCount;

        StartCoroutine(AICharactersMovementJobCoroutine());
        StartCoroutine(AICharacterAttackJobCoroutine());
    }

    protected IEnumerator AICharactersMovementJobCoroutine()
    {
        while (true)
        {
            AICharactersMovementJobProcessor();
            yield return delayObject;
        }
    }

    protected void AICharactersMovementJobProcessor()
    {
        SpawnableMonster[] spawnedMonsters = FindObjectsOfType<SpawnableMonster>();
        int length = spawnedMonsters.Length;
        if (length == 0)
        {
            return;
        }

        NativeArray<Vector3> positions = new NativeArray<Vector3>(length, Allocator.TempJob);
        NativeArray<float> movementSpeed = new NativeArray<float>(length, Allocator.TempJob);
        for (int i = 0; i < length; i++)
        {
            positions[i] = spawnedMonsters[i].transform.position;
            movementSpeed[i] = spawnedMonsters[i].CharStats.movementSpeed;
        }

        AICharactersMovementJob movementJob = new AICharactersMovementJob
        {
            playerPosition = ArenaManager.Manager.Player.transform.position,
            positions = positions,
            movementSpeed = movementSpeed
        };

        JobHandle handle = movementJob.Schedule(length, (int) Mathf.Ceil(length / cpuCoresAmount));
        handle.Complete();

        for (int i = 0; i < length; i++)
        {
            spawnedMonsters[i].targetPos = positions[i];
        }
    }

    protected IEnumerator AICharacterAttackJobCoroutine()
    {
        while (true)
        {
            AICharacterAttackJobProcessor();
            yield return delayObject;
        }
    }

    protected void AICharacterAttackJobProcessor()
    {
        MonsterController[] spawnedMonsters = FindObjectsOfType<MonsterController>();
        int length = spawnedMonsters.Length;
        if (length == 0)
        {
            return;
        }

        NativeArray<Vector3> positions = new NativeArray<Vector3>(length, Allocator.TempJob);
        NativeArray<float> attackDistances = new NativeArray<float>(length, Allocator.TempJob);
        NativeArray<bool> attackCapability = new NativeArray<bool>(length, Allocator.TempJob);

        for (int i = 0; i < length; i++)
        {
            positions[i] = spawnedMonsters[i].transform.position;
            attackDistances[i] = spawnedMonsters[i].BaseObject.CharStats.attackRange;
        }

        AICharactersAttackJob attackJob = new AICharactersAttackJob
        {
            positions = positions,
            attackDistances = attackDistances,
            attackCapability = attackCapability,
            playerPosition = ArenaManager.Manager.Player.transform.position
        };

        JobHandle handle = attackJob.Schedule(length, (int) Mathf.Ceil(length / cpuCoresAmount));
        handle.Complete();

        for (int i = 0; i < length; i++)
        {
            spawnedMonsters[i].inAttackRange = attackCapability[i];
        }
    }
}
