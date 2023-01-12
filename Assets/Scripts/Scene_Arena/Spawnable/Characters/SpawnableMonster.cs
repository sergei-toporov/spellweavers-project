using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnableMonster : SpawnableBase
{
    protected CharacterController controller;
    public CharacterController Controller { get => controller ?? GetComponent<CharacterController>(); }

    public Vector3 targetPos = Vector3.zero;

    protected void OnEnable()
    {
        StartCoroutine(CheckVerticalPositionCoroutine());
    }
    protected void Update()
    {
        if (isReady)
        {
            ControlCharacter();
        }        
    }

    protected void OnDisable()
    {
        StopCoroutine(CheckVerticalPositionCoroutine());
    }

    protected override void CharacterDeath()
    {
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
        Controller.SimpleMove(targetPos);
    }

    protected IEnumerator CheckVerticalPositionCoroutine()
    {
        while (true)
        {
            if (Mathf.Abs(transform.position.y) > 100.0f)
            {
                Controller.enabled = false;
                Vector3 pos = new Vector3(
                    ArenaManager.Manager.SpawnPointPlayer.transform.position.x,
                    2.0f,
                    ArenaManager.Manager.SpawnPointPlayer.transform.position.z
                    );
                transform.position = pos;
                Controller.enabled = true;
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

}
