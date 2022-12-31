using TMPro;
using UnityEngine;

abstract public class AbilitiesUpgradeScreen_CharacterStatValueBase : MonoBehaviour
{
    protected TextMeshProUGUI text;
    protected void Awake()
    {
        InitConfiguration();
    }

    public virtual void UpdateText()
    {
        if (text == null)
        {
            InitConfiguration();
            UpdateText();
        }
    }

    protected void InitConfiguration()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
}
