using UnityEngine;

public class AbilitiesUpgradeScreen_CharacterStatClassname : AbilitiesUpgradeScreen_CharacterStatValueBase
{

    protected void OnEnable()
    {
        UpdateText();
    }
    public override void UpdateText()
    {
        base.UpdateText();
        text.text = $"{ArenaManager.Manager.PlayerBase.ClassName}";
    }
}
