using Spellweavers;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnableMonster : SpawnableBase
{
    protected CharacterController controller;
    public CharacterController Controller { get => controller ?? GetComponent<CharacterController>(); }

    public Vector3 targetPos = Vector3.zero;

    protected void Start()
    {
        RandomizeStats();
    }

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
            if (Mathf.Abs(transform.position.y) > 10.0f)
            {
                Controller.enabled = false;
                transform.position = startPos;
                Controller.enabled = true;
                Vector3 mv = Controller.velocity * -1;
                mv.y = -9.81f;
                Controller.Move(mv);
            }

            yield return secondDelayObject;
        }
    }

    protected void RandomizeStats()
    {
        charStats.attackRangeBase = RandomizeStat(CharStats.attackRangeBase);
        charStats.attackRange = CharStats.attackRangeBase;
        charStats.healthBase = RandomizeStat(charStats.healthBase);
        charStats.health = charStats.healthBase;
        charStats.healthRegenBase = RandomizeStat(charStats.healthRegenBase);
        charStats.healthRegen = charStats.healthRegenBase;
        charStats.manaBase = RandomizeStat(charStats.manaBase);
        charStats.mana = charStats.manaBase;
        charStats.manaRegenBase = RandomizeStat(charStats.manaRegenBase);
        charStats.manaRegen = charStats.manaRegenBase;
        charStats.attackRangeBase = RandomizeStat(charStats.attackRangeBase);
        charStats.attackRange = charStats.attackRangeBase;
        charStats.attacksPerMinuteBase = RandomizeStat(charStats.attacksPerMinuteBase);
        charStats.attacksPerMinute = charStats.attacksPerMinuteBase;
        charStats.damageBase = RandomizeStat(charStats.damageBase);
        charStats.damage = charStats.damageBase;
    }

    protected static float RandomizeStat (float stat)
    {
        float mod = 1.0f + Random.Range(-0.1f, 0.1f);
        return (stat + stat * ArenaManager.Manager.WaveController.PerWaveStatBonus * ArenaManager.Manager.WaveController.WaveNumber) * mod;
    }

}
