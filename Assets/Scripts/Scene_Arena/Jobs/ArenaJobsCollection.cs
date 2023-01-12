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

        public struct AIBuildSpawnpointsJob : IJobParallelFor
        {
            [WriteOnly] public NativeArray<Vector3> positions;
            [ReadOnly] public NativeArray<Vector3> basePositions;
            [ReadOnly] public int amount;
            
            public void Execute(int index)
            {
                System.Random rnd = new System.Random();
                int spAmount = basePositions.Length;
                Vector3 spPos = basePositions[rnd.Next(0, spAmount)];
                Vector3 mod = new Vector3(
                    GetRandomFloatModifier(),
                    1.0f,
                    GetRandomFloatModifier()
                    );
                positions[index] = spPos + mod;
            }

            private float GetRandomFloatModifier(int multi = 1)
            {
                System.Random rnd = new System.Random();
                float result = (float)rnd.NextDouble() * multi;
                if ((float)rnd.NextDouble() < 0.5)
                {
                    result = -result;
                }
                return result;
            }
        }

    }
}