public class AbilitiesUpgradeScreen_CharacterStatAttackRange : AbilitiesUpgradeScreen_CharacterStatValueBase
{
    public override void UpdateText()
    {
        base.UpdateText();
        text.text = $"{ArenaManager.Manager.PlayerBase.CharStats.attackRangeBase}";
    }
}
