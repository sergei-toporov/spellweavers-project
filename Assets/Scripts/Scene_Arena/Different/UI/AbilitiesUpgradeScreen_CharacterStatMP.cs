public class AbilitiesUpgradeScreen_CharacterStatMP : AbilitiesUpgradeScreen_CharacterStatValueBase
{
    public override void UpdateText()
    {
        base.UpdateText();
        text.text = $"{ArenaManager.Manager.PlayerBase.CharStats.mana} / {ArenaManager.Manager.PlayerBase.CharStats.manaBase}";
    }
}
