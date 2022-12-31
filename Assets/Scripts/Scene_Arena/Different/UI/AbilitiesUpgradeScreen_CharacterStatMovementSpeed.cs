public class AbilitiesUpgradeScreen_CharacterStatMovementSpeed : AbilitiesUpgradeScreen_CharacterStatValueBase
{
    public override void UpdateText()
    {
        base.UpdateText();
        text.text = $"{ArenaManager.Manager.PlayerBase.CharStats.movementSpeedBase}";
    }
}
