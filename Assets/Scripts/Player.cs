using System;
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
    public FixedJoystick _joystick;
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
    //public Transform meleeWeapon;
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

    //weapon
    public Transform weapon;
    public SpriteRenderer weaponSprite;
    private bool isFacingRight = true;

    //shoot
    [SerializeField]
    public Bullet bulletPrefab;
    private Quaternion weaponRotation;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //health
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        //attack
        //initialRotation = meleeWeapon.rotation;

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
        healing();
        GainExp(20);
        reload();
        ControlWeapon();
        Shoot(weaponRotation);
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

        healthBar.SetHealth(health);
        GeneralUI.createPopUp(transform.position, damageAmount.ToString(), 2);

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.CompareTag("Weapon"))
        {
            // Assign the weapon to the weapon variable
            weapon = collision.transform;
            weaponSprite = collision.GetComponent<SpriteRenderer>();
        }
    }

    //public void MeleeAttack()
    //{
    //    if (!isAttacking)
    //    {
    //        isAttacking = true;
    //        StartCoroutine(SwingWeapon());
    //        Invoke("DisableMeleeAttack", attackDuration);
    //    }
    //}

    ////attack melee
    //private IEnumerator SwingWeapon()
    //{
    //    float elapsedTime = 0f;
    //    Quaternion targetRotation = initialRotation;

    //    while (elapsedTime < attackDuration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float t = elapsedTime / attackDuration;

    //        targetRotation *= Quaternion.Euler(0f, 0f, -rotationSpeed * Time.deltaTime);
    //        meleeWeapon.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);

    //        yield return null;
    //    }

    //    meleeWeapon.rotation = initialRotation;
    //}

    //private void DisableMeleeAttack()
    //{
    //    isAttacking = false;
    //}


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
            health = health - 20;
            healthBar.SetHealth(health);
            GeneralUI.createPopUp(transform.position, "20", 2);
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

    public void ControlWeapon()
    {
        if (_joystick != null)
        {
            // Get the horizontal input from the joystick
            float horizontal = _joystick.Horizontal;

            
            // Flip the player and the weapon based on movement direction
            if (horizontal > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (horizontal < 0 && isFacingRight)
            {
                Flip();
            }

            // Aim the weapon based on joystick input
            AimWeapon();
        }
    }

    private void Flip()
    {
        // Flip the player horizontally
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
        if(weapon!= null)
        {
            weaponSprite.flipY = !isFacingRight;
        }
    }


    private void AimWeapon()
    {
        if (weapon != null && _joystick != null)
        {
            // Get the direction values from the joystick
            float horizontal = _joystick.Horizontal;
            float vertical = _joystick.Vertical;

            // Check if there is joystick input
            if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f))
            {
                // No joystick input, do not update the weapon's rotation
                weaponRotation = weapon.rotation;
                return;
            }

            // Calculate the rotation angle based on joystick input
            float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;

            // Apply the rotation to the weapon
            weapon.rotation = Quaternion.Euler(0f, 0f, angle);
            weaponRotation = weapon.rotation;
        }
    }

    private void Shoot(Quaternion weaponRotation)
    {
        if (_joystick != null && weapon!=null && Input.GetKeyDown(KeyCode.Space))
        {
            Bullet bullet = Instantiate(bulletPrefab, weapon.position + new Vector3(0f,0f,0), Quaternion.identity);
            bullet.SetDirection(weaponRotation);
            currentAmmo = currentAmmo - 1;
            ammoBar.SetAmmo(currentAmmo);
        }
    }

}
