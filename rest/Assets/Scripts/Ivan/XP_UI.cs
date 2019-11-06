using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XP_UI : MonoBehaviour
{

    public Image xpSlider;
    public XP xp;

    public void UpdateValue()
    {
        if(xpSlider != null)
        {
            xpSlider.fillAmount = (float)xp.GetFillAmount();
        }
    }

    public void OnEnable()
    {
        UpdateValue();
        xp.OnValueChanged.AddListener(UpdateValue);
    }

    public void OnDisable()
    {
        xp.OnValueChanged.RemoveAllListeners();
    }
}
