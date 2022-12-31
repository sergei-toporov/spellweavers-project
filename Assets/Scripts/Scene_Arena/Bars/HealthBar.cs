using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBar : StatBarBase
{
    protected SpawnableBase parentObject;
    protected void Start()
    {
        parentObject = GetComponentInParent<SpawnableBase>();
        if (parentObject != null)
        {
            barValueChange = UpdateBar;
            SetValues(parentObject.CharStats.healthBase, parentObject.CharStats.healthBase);
        }
    }

    protected override void UpdateBar()
    {
        slider.value = parentObject.CharStats.health;
    }

    public void ResetValues()
    {
        SetValues(parentObject.CharStats.health, parentObject.CharStats.healthBase);
    }
}
