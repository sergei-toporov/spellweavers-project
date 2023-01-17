using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum MonsterDifficultyLevels
{
    Easy,
    Medium,
    Hard,
    Boss
}

[Serializable]
public struct CharacterStats
{
    public float healthBase;
    public float health;
    public float manaBase;
    public float mana;
    public float movementSpeedBase;
    public float movementSpeed;
    public float attacksPerMinuteBase;
    public float attacksPerMinute;
    public float attackRangeBase;
    public float attackRange;
    public float damageBase;
    public float damage;
    public float damageRadiusBase;
    public float damageRadius;
    public float healthRegenBase;
    public float healthRegen;
    public float manaRegenBase;
    public float manaRegen;
}

abstract public class SpawnableBase : MonoBehaviour
{
    [SerializeField] protected HealthBar healthBarPrefab;
    public HealthBar HealthBarPrefab { get => healthBarPrefab; }
    
    protected HealthBar healthBar;
    public HealthBar HealthBar { get => healthBar; }

    protected Canvas spawnableCanvas;

    [SerializeField] protected CharacterStats charStats;
    public CharacterStats CharStats { get => charStats; }

    [SerializeField] protected CharacterClassMetadata baseStats;
    public CharacterClassMetadata BaseStats { get => baseStats; }

    protected string className;
    public string ClassName { get => className; }
    protected WeaponHitter hitterPrefab;
    public WeaponHitter HitterPrefab { get => hitterPrefab; }

    [SerializeField] protected bool isReady = false;

    protected bool isDead = false;

    protected WaitForSeconds secondDelayObject = new WaitForSeconds(1.0f);

    protected Vector3 startPos;


    protected virtual void Awake()
    {
        if (!InitialCheck())
        {
            Debug.LogError("Errors occurred during the initial check! Please, fix them to proceed.");
            Application.Quit();
        }

        spawnableCanvas = GetComponentInChildren<Canvas>();

        if (healthBarPrefab != null) {            
            float meshHeight = GetComponent<MeshFilter>().sharedMesh.bounds.size.y;
            Vector3 position = transform.position + new Vector3(.0f, meshHeight, .0f);
            healthBar = Instantiate(healthBarPrefab, position, Quaternion.identity);
            healthBar.gameObject.transform.SetParent(spawnableCanvas.transform);
        }
        else
        {
            spawnableCanvas.gameObject.SetActive(false);
        }

        startPos = transform.position;
        className = baseStats.defaultName;
    }

    protected bool InitialCheck()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas == null)
        {
            Debug.LogError($"The canvas component is not found in any children. It is required to show UI elements for the '{gameObject.name}'");
            return false;
        }
        return true;
    }

    protected void SetStartStats()
    {
        charStats.health = baseStats.baseHealth;
        charStats.healthBase = baseStats.baseHealth;
        charStats.mana = baseStats.baseMana;
        charStats.manaBase = baseStats.baseMana;
        charStats.movementSpeed = baseStats.baseMovementSpeed;
        charStats.movementSpeedBase = baseStats.baseMovementSpeed;
        charStats.attacksPerMinute = baseStats.baseAttacksPerMinute;
        charStats.attacksPerMinuteBase = baseStats.baseAttacksPerMinute;
        charStats.attackRange = baseStats.baseAttackRange;
        charStats.attackRangeBase = baseStats.baseAttackRange;
        charStats.manaRegen = baseStats.baseManaRegeneration;
        charStats.manaRegenBase = baseStats.baseManaRegeneration;
        charStats.healthRegen = baseStats.baseHealthRegeneration;
        charStats.healthRegenBase = baseStats.baseHealthRegeneration;
        charStats.damage = baseStats.baseDamage;
        charStats.damageBase = baseStats.baseDamage;
        charStats.damageRadius = baseStats.baseDamageRadius;
        charStats.damageRadiusBase = baseStats.baseDamageRadius;
        hitterPrefab = baseStats.weaponHitterPrefab;

        if (charStats.healthRegen > 0.0f)
        {
            StartCoroutine(HealthRegeneration());
        }
    }

    public void TakeDamage(SpawnableBase attacker)
    {
        if (!isDead && GetType() != attacker.GetType()) {
            charStats.health -= attacker.CharStats.damage;

            if (healthBar.gameObject.scene.rootCount != 0)
            {
                healthBar.BarValueChange.Invoke();
            }
        }

        if (charStats.health <= 0)
        {
            isDead = true;
            CharacterDeath();
        }
    }

    public void AddBaseStats(CharacterClassMetadata metadata)
    {
        if (metadata.defaultPrefab != null)
        {
            baseStats = metadata;
            SetStartStats();
            isReady = true;
        }        
    }

    protected void UpdateBars()
    {
        if (healthBar.gameObject.scene.rootCount != 0)
        {
            healthBar.ResetValues();
        }
    }

    abstract protected void CharacterDeath();

    protected IEnumerator HealthRegeneration()
    {
        while (true)
        {
            if (charStats.health < charStats.healthBase)
            {
                charStats.health += charStats.healthRegen;
                UpdateBars();
            }

            yield return secondDelayObject;
        }
        
    }
}
