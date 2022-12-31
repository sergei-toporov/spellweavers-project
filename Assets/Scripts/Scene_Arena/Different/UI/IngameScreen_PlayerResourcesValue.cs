using TMPro;
using UnityEngine;

public class IngameScreen_PlayerResourcesValue : MonoBehaviour
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


        text.text = $"{ArenaResourceManager.Manager.ResourcesToSpend}";
    }

    protected void InitConfiguration()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
}
