using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettingScript : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        setVolumeSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volumeSetting", volume );
    }

    public void setVolumeSettings()
    {
        float volumeSetting = PlayerPrefs.GetFloat("volumeSetting");
        if(volumeSetting != null)
        {
            audioMixer.SetFloat("volume", volumeSetting);
        }
    }
}
