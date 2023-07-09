using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    //UI
    public ExperienceScript experienceBar;
    public AmmoScript ammoBar;
    public GeneralUIScript GeneralUI;
    public PlayerUIScript playerUI;


    //movement
    [SerializeField]
    private FixedJoystick _joystick;
    private Rigidbody2D _rb;
    private Vector2 move;
    [SerializeField]
    private float _moveSpeed;

    //health
    [SerializeField]
    public HealthBarScript healthBar;
    public float maxHealth = 100;
    public float health;


    //attack
    public Transform meleeWeapon;
    public float rotationSpeed = 100f;
    public float swingRadius = 2f;
    public float attackDuration = 0.5f;
    private bool isAttacking = false;
    private Quaternion initialRotation;

    //level
    public int currentLevel = 1;
    const int XP_INCREMENT_PER_LEVEL = 100;
    public int currentExp;
    public int maxExp;

    //ammo
    public int remainingAmmo;
    public int maxClip;
    public int currentAmmo;
    public float reloadTime;



    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //health
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        //attack
        initialRotation = meleeWeapon.rotation;

        //level
        maxExp = GetRequiredExpForLevel(1);
        experienceBar.SetMaxExperience(maxExp);
        currentExp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        Movement();
        takeDamage();
        healing();
        shoot();
        GainExp(20);
        reload();
    }

    void Movement()
    {
        move.x = _joystick.Horizontal;
        move.y = _joystick.Vertical;
        _rb.MovePosition(_rb.position + move * _moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        health = Mathf.Max(health, 0); // Ensure health doesn't go below zero
        //healthBar.value = health;

        if (health <= 0)
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            TakeDamage(10);
        }
    }

    public void MeleeAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(SwingWeapon());
            Invoke("DisableMeleeAttack", attackDuration);
        }
    }

    //attack melee
    private IEnumerator SwingWeapon()
    {
        float elapsedTime = 0f;
        Quaternion targetRotation = initialRotation;

        while (elapsedTime < attackDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / attackDuration;

            targetRotation *= Quaternion.Euler(0f, 0f, -rotationSpeed * Time.deltaTime);
            meleeWeapon.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);

            yield return null;
        }

        meleeWeapon.rotation = initialRotation;
    }

    private void DisableMeleeAttack()
    {
        isAttacking = false;
    }


    //leveling
    public int GetRequiredExpForLevel(int level)
    {
        return XP_INCREMENT_PER_LEVEL * level * (level + 1) / 2;
    }

    private void LevelUp()
    {
        Debug.Log("Level Up! Current Level: " + currentLevel);

        int requiredExpForNextLevel = GetRequiredExpForLevel(currentLevel + 1);
        maxExp = requiredExpForNextLevel;
        currentExp = 0;
    }

    public void GainExp(int expAmount)
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            currentExp = currentExp + 20;
            experienceBar.SetExperience(currentExp);

            if (currentExp >= maxExp)
        {
            LevelUp();
        }
        }
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
