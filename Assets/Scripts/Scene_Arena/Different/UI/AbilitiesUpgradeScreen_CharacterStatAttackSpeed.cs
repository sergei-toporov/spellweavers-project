public class AbilitiesUpgradeScreen_CharacterStatAttackSpeed : AbilitiesUpgradeScreen_CharacterStatValueBase
{
   public override void UpdateText()
    {
        base.UpdateText();
        text.text = $"{ArenaManager.Manager.PlayerBase.CharStats.attacksPerMinuteBase}";
    }
}
