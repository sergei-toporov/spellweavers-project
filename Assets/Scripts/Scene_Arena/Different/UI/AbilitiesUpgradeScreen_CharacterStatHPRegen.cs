public class FeatsUpgradeScreen_CharacterStatHPRegen : AbilitiesUpgradeScreen_CharacterStatValueBase
{
    public override void UpdateText()
    {
        base.UpdateText();
        text.text = $"{ArenaManager.Manager.PlayerBase.CharStats.healthRegenBase}";
    }
}
