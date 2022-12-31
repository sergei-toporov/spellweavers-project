public class AbilitiesUpgradeScreen_CharacterStatDamageRadius : AbilitiesUpgradeScreen_CharacterStatValueBase
{
    public override void UpdateText()
    {
        base.UpdateText();
        text.text = $"{ArenaManager.Manager.PlayerBase.CharStats.damageRadiusBase}";
    }
}
