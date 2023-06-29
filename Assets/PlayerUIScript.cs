using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    private bool reloading = false;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        reloadBarRunning();
    }

    public void reloadActivation(float reloadTime)
    {
        reloading = true;
        slider.maxValue = reloadTime;
       
    }
    public void reloadBarRunning()
    {
        if(reloading == true)
        {
            if (reloading == true)
            {
                if (slider.value == 0)
                {
                    slider.gameObject.SetActive(true);
                }
                slider.value = slider.value + 1 * Time.deltaTime;
                if (slider.value >= slider.maxValue)
                {
                    reloading = false;
                    slider.gameObject.SetActive(false);
                    slider.value = 0;
                }
            }
        }
    }
}
