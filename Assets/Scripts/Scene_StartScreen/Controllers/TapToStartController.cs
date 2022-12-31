using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TapToStartController : MonoBehaviour
{

    protected float flashingDelayValue = .75f;
    protected TextMeshProUGUI text;
    protected WaitForSecondsRealtime flashingDelay;
    protected int initAttempts = 0;
    protected int maxInitAttempts = 10;

    protected void Awake()
    {
        flashingDelay = new WaitForSecondsRealtime(flashingDelayValue);
        InitializeText();
    }

    public void StartFlashing()
    {
        if (text != null)
        {
            StartCoroutine(TextFlashingCoroutine());
        }
        else if (initAttempts < maxInitAttempts)
        {
            InitializeText();
            StartFlashing();
            initAttempts++;
        }
        else
        {
            Debug.Log("Some problems with the 'text' elements initialization. Skipping flashing coroutine.");
        }        
    }

    public void StopFlashing()
    {
        StopCoroutine(TextFlashingCoroutine());
    }

    public void MakeVisible()
    {
        text.enabled = true;
    }

    public void MakeInvisible()
    {
        text.enabled = false;
    }

    protected IEnumerator TextFlashingCoroutine()
    {
        while (true)
        {
            MakeVisible();
            yield return flashingDelay;
            MakeInvisible();
            yield return flashingDelay;
        }
    }

    protected void InitializeText()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
}
