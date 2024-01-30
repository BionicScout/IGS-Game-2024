using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar(float curValue, float maxValue)
    {
        slider.value = curValue / maxValue;
    }
}
