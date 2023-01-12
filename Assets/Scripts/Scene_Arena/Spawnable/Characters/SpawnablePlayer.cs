using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnablePlayer : SpawnableBase
{
    protected PlayerController controller;

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<PlayerController>();
    }
    public void RecalculateStats()
    {
        float diff = 0.0f;
        foreach (PlayerAbility ability in controller.Abilities.Values)
        {
            switch (ability.affectedStat)
            {
                case AffectedStat.HealthBase:
                    diff = BaseStats.baseHealth + BaseStats.baseHealth / 100.0f * ability.changeValue * ability.currentLevel - charStats.healthBase;
                    charStats.healthBase += diff;
                    charStats.health += diff;
                    break;
                case AffectedStat.ManaBase:
                    diff = BaseStats.baseMana + BaseStats.baseMana / 100.0f * ability.changeValue * ability.currentLevel - charStats.manaBase;
                    charStats.manaBase += diff;
                    charStats.mana += diff;
                    break;
                case AffectedStat.MovementSpeedBase:
                    diff = BaseStats.baseMovementSpeed + BaseStats.baseMovementSpeed / 100.0f * ability.changeValue * ability.currentLevel - charStats.movementSpeedBase;
                    charStats.movementSpeedBase += diff;
                    charStats.movementSpeed += diff;
                    break;
                case AffectedStat.AttackRangeBase:
                    diff = BaseStats.baseAttackRange + BaseStats.baseAttackRange / 100.0f * ability.changeValue * ability.currentLevel - charStats.attackRangeBase;
                    charStats.attackRangeBase += diff;
                    charStats.attackRange += diff;
                    break;
                case AffectedStat.AttacksPerMinuteBase:
                    diff = BaseStats.baseAttacksPerMinute + BaseStats.baseAttacksPerMinute / 100.0f * ability.changeValue * ability.currentLevel - charStats.attacksPerMinuteBase;
                    charStats.attacksPerMinuteBase += diff;
                    charStats.attacksPerMinute += diff;
                    controller.SetAttackDelayParameters();
                    break;
                case AffectedStat.DamageBase:
                    diff = BaseStats.baseDamage + BaseStats.baseDamage / 100.0f * ability.changeValue * ability.currentLevel - charStats.damageBase;
                    charStats.damageBase += diff;
                    charStats.damage += diff;
                    break;
                case AffectedStat.DamageRadiusBase:
                    diff = BaseStats.baseDamageRadius + BaseStats.baseDamageRadius / 100.0f * ability.changeValue * ability.currentLevel - charStats.damageRadiusBase;
                    charStats.damageRadiusBase += diff;
                    charStats.damageRadius += diff;
                    break;
                case AffectedStat.HealthRegenBase:
                    diff = BaseStats.baseHealthRegeneration + BaseStats.baseHealthRegeneration / 100.0f * ability.changeValue * ability.currentLevel - charStats.healthRegenBase;
                    charStats.healthRegenBase += diff;
                    charStats.healthRegen += diff;
                    break;
                case AffectedStat.ManaRegenBase:
                    diff = BaseStats.baseManaRegeneration + BaseStats.baseManaRegeneration / 100.0f * ability.changeValue * ability.currentLevel - charStats.manaRegenBase;
                    charStats.manaRegenBase += diff;
                    charStats.manaRegen += diff;
                    break;
            }            
        }

        UpdateBars();
    }

    protected override void CharacterDeath()
    {
        ArenaWorkflowManager.Manager.SwitchState(ArenaStates.DeathScreen);
    }

    public void StuffPickup(CollectibleStuffUnit stuffUnit)
    {
        if (stuffUnit.UnitData.affectsCharacterStat)
        {
            ModifyStats(stuffUnit);
        }

        UpdateBars();
    }

    public void ResourcePickup(CollectibleResourceUnit stuffUnit)
    {
        ArenaResourceManager.Manager.AddPlayerResources(stuffUnit);
    }

    protected void ModifyStats(CollectibleStuffUnit stuffUnit)
    {
        switch (stuffUnit.UnitData.affectedStat)
        {
            case AffectedStat.Health:              
                charStats.health += stuffUnit.UnitData.effectAmount;
                charStats.health = charStats.health > charStats.healthBase ? charStats.healthBase : charStats.health;
                break;
        }
    }
}
