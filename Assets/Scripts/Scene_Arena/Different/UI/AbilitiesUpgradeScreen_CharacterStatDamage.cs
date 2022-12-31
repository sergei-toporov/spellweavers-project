public class AbilitiesUpgradeScreen_CharacterStatDamage : AbilitiesUpgradeScreen_CharacterStatValueBase
{
    public override void UpdateText()
    {
        base.UpdateText();
        text.text = $"{ArenaManager.Manager.PlayerBase.CharStats.damageBase}";
    }
}
