using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Spellweavers
{
    namespace ArenaScene
    {
        public struct AICharactersMovementJob : IJobParallelFor
        {
            public NativeArray<Vector3> positions;
            [ReadOnly] public NativeArray<float> movementSpeed;
            [ReadOnly] public Vector3 playerPosition;
            public void Execute(int index)
            {
                Vector3 dir = (playerPosition - positions[index]).normalized;
                dir.y = -9.81f;
                dir *= movementSpeed[index];
                positions[index] = dir;
            }
        }

        public struct AICharactersAttackJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<Vector3> positions;
            [ReadOnly] public NativeArray<float> attackDistances;
            [ReadOnly] public Vector3 playerPosition;
            [WriteOnly] public NativeArray<bool> attackCapability;

            public void Execute(int index)
            {
                attackCapability[index] = Vector3.Distance(positions[index], playerPosition) <= attackDistances[index];
            }
        }

    }
}