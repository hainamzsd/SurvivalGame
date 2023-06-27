using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptDemo : MonoBehaviour
{
    public HealthBarScript healthBar;
    public ExperienceScript experienceBar;
    public AmmoScript ammoBar;

    public float MaxHealth;
    public float health;
    public float maxExperience;
    public float experience;
    public float maxAmmo;
    public float ammo;
    
    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
        healthBar.SetHealth(MaxHealth);

        experience = 0;
        experienceBar.SetMaxExperience(maxExperience);

        ammo = maxAmmo;
        ammoBar.SetMaxAmmo(maxAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        takeDamage();
        addExperience();
        shoot();
    }

    void takeDamage()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            health = health - 20;
            healthBar.SetHealth(health);
        }
    }

    void addExperience()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            experience = experience + 20;
            experienceBar.SetExperience(experience);
        }
    }

    void shoot()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ammo = ammo - 1;
           ammoBar.SetAmmo(ammo);
        }
    }
}
