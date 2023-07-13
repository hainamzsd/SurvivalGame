using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{

    //UI
    public ExperienceScript experienceBar;
    public AmmoScript ammoBar;
    public GeneralUIScript GeneralUI;
    public PlayerUIScript playerUI;
    public SpriteRenderer spriteRenderer;


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
    //public float rotationSpeed = 100f;
    //public float swingRadius = 2f;
    //public float attackDuration = 0.5f;
    //private bool isAttacking = false;
    //private Quaternion initialRotation;

    //level
    public int currentLevel { get; set; } = 1;
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
    public GameObject Object;
    Vector2 GameobjectRotation;
    private float GameobjectRotation2;
    private float GameobjectRotation3;
    public FixedJoystick aim;
    private Transform shootingpoint;



    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //health
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        //ammo
        maxClip = 10;
        currentAmmo = 10;
        ammoBar.SetAmmo(currentAmmo);
        ammoBar.SetMaxClip(maxClip);
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
        reload();
        ControlWeapon();
    }

    void Movement()
    {
        
            move.x = _joystick.Horizontal;
            move.y = _joystick.Vertical;
            _rb.MovePosition(_rb.position + move * _moveSpeed * Time.deltaTime);
        // Check if the player is not walking
        bool isWalking = move.magnitude > 0f;

        // Set the speed variable in the animator
        animator.SetFloat("speed", isWalking ? 1f : -1f);
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
            Object = collision.gameObject;
            weapon = collision.transform;
            weaponSprite = collision.GetComponent<SpriteRenderer>();
            shootingpoint = collision.transform.Find("ShootingPoint").transform;
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
            experienceBar.SetExperience(expAmount);
        GeneralUI.createPopUp(transform.position, expAmount.ToString() + " EXP", 4);
        if (currentExp >= maxExp)   
        {
            LevelUp();
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

        spriteRenderer.flipX = !isFacingRight;


        //if (weapon!= null)
        //{
            
        //    weaponSprite.flipY = !isFacingRight;
        //}

    }


    public void AimWeapon()
    {
        if (weapon != null && _joystick != null)
        {//Gets the input from the jostick

            GameobjectRotation = new Vector2(aim.Horizontal, aim.Vertical);

            GameobjectRotation3 = GameobjectRotation.x;

            if (isFacingRight)
            {
                //Rotates the object if the player is facing right
                GameobjectRotation2 = GameobjectRotation.x + GameobjectRotation.y * 90;
                Object.transform.rotation = Quaternion.Euler(0f, 0f, GameobjectRotation2);
            }
            else
            {
                //Rotates the object if the player is facing left
                GameobjectRotation2 = GameobjectRotation.x + GameobjectRotation.y * -90;
                Object.transform.rotation = Quaternion.Euler(0f, 180f, -GameobjectRotation2);
            }
            if (GameobjectRotation3 < 0 && isFacingRight)
            {
                // Executes the void: Flip()
                Flip();
            }
            else if (GameobjectRotation3 > 0 && !isFacingRight)
            {
                // Executes the void: Flip()
                Flip();
            }

            // Trigger shoot if the joystick is not centered
            if (GameobjectRotation != Vector2.zero)
            {
                Shoot();
            }
        }
    }

    public float fireRate = 0.5f; // Adjust the value to set the desired fire rate
    private float lastShotTime = 0f;

    public void Shoot()
    {
        if (_joystick != null && weapon != null && CanShoot())
        {
            Bullet bullet = Instantiate(bulletPrefab, shootingpoint.position, weapon.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            // Calculate the direction of the bullet based on the aim variable
            Vector2 bulletDirection = new Vector2(aim.Horizontal, aim.Vertical).normalized;

            // Assign the direction to the bullet script
            bulletScript.SetDirection(bulletDirection, isFacingRight);

            currentAmmo = currentAmmo - 1;
            ammoBar.SetAmmo(currentAmmo);

            lastShotTime = Time.time; // Update the last shot time
        }
    }
    private bool CanShoot()
    {
        // Check if enough time has passed since the last shot
        return Time.time - lastShotTime >= fireRate;
    }

}
