using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatBarBase : MonoBehaviour
{
    protected Slider slider;

    protected UnityAction barValueChange;
    public UnityAction BarValueChange { get => barValueChange; }

    protected void Awake()
    {
        if (!TryGetComponent(out Slider component))
        {
            Debug.LogError($"The 'Slider' component hasn't been found on this '{name}' bar. Please, add this component.");
            Application.Quit();
        }
        else
        {
            slider = component;
        }
        barValueChange = FakeUpdate;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void FakeUpdate()
    {
        Debug.Log("FakeUpdate");
        return;
    }

    protected virtual void UpdateBar()
    {
        Debug.Log("BaseBarUpdate");
        return;
    }

    protected void SetInitialValues(float value, float max)
    {
        SetValues(value, max);
    }

    protected void SetValues(float value, float max)
    {
        slider.maxValue = max;
        slider.value = value;
    } 
}
