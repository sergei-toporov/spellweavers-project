using TMPro;
using UnityEngine;

public class IngameScreen_MonstersAliveValue : MonoBehaviour
{
    protected TextMeshProUGUI text;
    protected void Awake()
    {
        InitConfiguration();
    }

    protected void Update()
    {
        UpdateText();
    }
    public virtual void UpdateText()
    {
        if (text == null)
        {
            InitConfiguration();
            UpdateText();
        }


        text.text = $"{FindObjectsOfType<SpawnableMonster>().Length}";
    }

    protected void InitConfiguration()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
}
