using UnityEngine;
using UnityEngine.UIElements;

public class CollectibleStuffUnit : MonoBehaviour
{
    protected CollectibleStuff unitData;
    public CollectibleStuff UnitData { get => unitData; }

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
            player.StuffPickup(this);
            Destroy(gameObject);
        }
    }

    public void SetParameters(CollectibleStuff providedData)
    {
        unitData = providedData;
    }
}
