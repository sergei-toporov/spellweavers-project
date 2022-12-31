using UnityEngine;

public class SpawnableMonster : SpawnableBase
{
    protected CharacterController controller;
    public CharacterController Controller { get => controller ?? GetComponent<CharacterController>(); }

    protected void Update()
    {
        if (isReady)
        {
            ControlCharacter();
        }        
    }

    protected override void CharacterDeath()
    {
        Debug.Log($"CharacterDeath called by '{name}");
        if (Random.value < 0.5f && ArenaResourceManager.Manager.AvailableCollectibleStuff.Count > 0)
        {
            CollectibleStuff collectibleStuff = ArenaResourceManager.Manager.GetRandomCollectibleStuff();
            if (collectibleStuff.dropChance > 0.0f && Random.value < (collectibleStuff.dropChance / 100))
            {
                ArenaResourceManager.Manager.SpawnCollectibleStuff(collectibleStuff, transform.position);
            }
        }
        ArenaResourceManager.Manager.SpawnCollectibleResourcesMandatory(transform.position);
        ArenaManager.Manager.DecreaseSpawnedAmount();
        Destroy(gameObject);
    }

    protected void ControlCharacter()
    {
        Vector3 lookAt = new Vector3(
            ArenaManager.Manager.Player.transform.position.x,
            transform.position.y,
            ArenaManager.Manager.Player.transform.position.z
            );
        transform.LookAt(lookAt);

        Vector3 dir = (ArenaManager.Manager.Player.transform.position - transform.position).normalized;
        dir.y = -9.81f;

        Controller.SimpleMove(charStats.movementSpeed * dir);
    }
}
