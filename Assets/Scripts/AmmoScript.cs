using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoScript : MonoBehaviour
{
    public Slider slider;
    public Text ammoDisplayText;
    int maxClip;
    public void SetMaxClip(int ammo)
    {
        maxClip = ammo;
        slider.maxValue = maxClip;
        slider.value = ammo;
        
        ammoDisplayText.text = ammo + "/" + maxClip;
    }

    public void SetAmmo(int ammo)
    {
        slider.value = ammo;
        ammoDisplayText.text = ammo + "/" + maxClip;
    }
}
