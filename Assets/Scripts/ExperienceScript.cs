using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceScript : MonoBehaviour
{
    public Slider slider;
    public void SetMaxExperience(float experience)
    {
        slider.maxValue = experience;
    }

    public void SetExperience(float experience)
    {
        slider.value = experience;
    }
}
