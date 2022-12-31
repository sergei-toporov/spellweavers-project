public class AbilitiesUpgradeScreen_CharacterStatClassname : AbilitiesUpgradeScreen_CharacterStatValueBase
{
    public override void UpdateText()
    {
        base.UpdateText();
        text.text = $"{ArenaManager.Manager.PlayerBase.ClassName}";
    }
}
