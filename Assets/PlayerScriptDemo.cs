using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptDemo : MonoBehaviour
{
    //DEMO INPUT
    //R: Reload
    //J: Healing
    //K: Take Damage
    //L: Add Experience
    //H: Shoot

    public HealthBarScript healthBar;
    public ExperienceScript experienceBar;
    public AmmoScript ammoBar;
    public GeneralUIScript GeneralUI;
    public PlayerUIScript playerUI;

    public float maxHealth;
    public float health;
    public float maxExperience;
    public float experience;
    public int remainingAmmo;
    public int maxClip;
    public int currentAmmo;
    public float reloadTime;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        experience = 0;
        experienceBar.SetMaxExperience(maxExperience);

        currentAmmo = maxClip;
        ammoBar.SetMaxClip(maxClip);
    }

    // Update is called once per frame
    void Update()
    {
        takeDamage();
        healing();
        addExperience();
        shoot();
        reload();
    }

    void takeDamage()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            health = health - 20;
            healthBar.SetHealth(health);
            GeneralUI.createPopUp(transform.position, "20", 2);
        }
    }

    void healing()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            health = health + 20;
            healthBar.SetHealth(health);
            GeneralUI.createPopUp(transform.position, "20", 1);
        }
    }

    void addExperience()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            experience = experience + 20;
            experienceBar.SetExperience(experience);
        }
    }

    void shoot()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentAmmo = currentAmmo - 1;
            ammoBar.SetAmmo(currentAmmo);
        }
    }

    void reload()
    {
        //Press R
        if (Input.GetKeyDown(KeyCode.R))
        {
            //No ammo
            if (remainingAmmo <= 0)
            {
                GeneralUI.createPopUp(transform.position, "No Ammo Left", 5);
            }
            //Full clip
            else if (currentAmmo == maxClip)
            {
                GeneralUI.createPopUp(transform.position, "Full Clip", 5);
            }
            //Reloading
            else
            {
                playerUI.reloadActivation(reloadTime);
                int reloadAmmo = maxClip - currentAmmo;
                if (remainingAmmo >= reloadAmmo)
                {
                    currentAmmo = currentAmmo + reloadAmmo;
                    remainingAmmo = remainingAmmo - reloadAmmo;

                }
                else
                {
                    currentAmmo = currentAmmo + remainingAmmo;
                    remainingAmmo = 0;
                }
                ammoBar.SetAmmo(currentAmmo);
            }
        }
    }
}
