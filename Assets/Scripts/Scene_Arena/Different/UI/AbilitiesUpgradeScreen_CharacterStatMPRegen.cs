public class AbilitiesUpgradeScreen_CharacterStatMPRegen : AbilitiesUpgradeScreen_CharacterStatValueBase
{
    public override void UpdateText()
    {
        base.UpdateText();
        text.text = $"{ArenaManager.Manager.PlayerBase.CharStats.manaRegenBase}";
    }
}
