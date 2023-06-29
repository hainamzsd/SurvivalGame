using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    public Text healthText;

    private float maxHealth;
    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        slider.maxValue = maxHealth;
        slider.value = health;
        healthText.text = health + "/" + maxHealth;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        healthText.text = health + "/" + maxHealth;
    }
}
