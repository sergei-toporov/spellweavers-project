public class AbilitiesUpgradeScreen_CharacterStatHP : AbilitiesUpgradeScreen_CharacterStatValueBase
{
    public override void UpdateText()
    {
        base.UpdateText();
        text.text = $"{ArenaManager.Manager.PlayerBase.CharStats.health} / {ArenaManager.Manager.PlayerBase.CharStats.healthBase}";
    }
}
