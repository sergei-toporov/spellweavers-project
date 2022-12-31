using UnityEngine;

public class CollectibleResourceUnit : MonoBehaviour
{
    protected CollectibleResource unitData;
    public CollectibleResource UnitData { get => unitData; }

    protected float spawnPositionSeed = 0.6f;

    protected void Awake()
    {
        transform.position = new Vector3(
            transform.position.x + Random.Range(-spawnPositionSeed, spawnPositionSeed),
            0.3f,
            transform.position.z + Random.Range(-spawnPositionSeed, spawnPositionSeed)
            );
    }

    protected void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out SpawnablePlayer player))
        {
            player.ResourcePickup(this);
            Destroy(gameObject);
        }
    }

    public void SetParameters(CollectibleResource providedData)
    {
        unitData = providedData;
    }
}
