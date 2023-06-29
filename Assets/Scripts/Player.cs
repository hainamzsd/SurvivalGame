using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    //movement
    [SerializeField]
    private FixedJoystick _joystick;
    private Rigidbody2D _rb;
    private Vector2 move;
    [SerializeField]
    private float _moveSpeed;

    //health
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    //attack
    public Transform meleeWeapon;
    public float rotationSpeed = 100f;
    public float swingRadius = 2f;
    public float attackDuration = 0.5f;
    private bool isAttacking = false;
    private Quaternion initialRotation;

    //level
    public int currentExp;
    public int maxExp;



    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //health
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        //attack
        initialRotation = meleeWeapon.rotation;

        //level
        currentExp = 0;
        maxExp = GetRequiredExpForLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        Movement();
        
    }

    void Movement()
    {
        move.x = _joystick.Horizontal;
        move.y = _joystick.Vertical;
        _rb.MovePosition(_rb.position + move * _moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0); // Ensure health doesn't go below zero
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
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


    //level
    public int GetRequiredExpForLevel(int level)
    {
        // Adjust these parameters to customize the curve
        float baseExp = 100f;   // Base experience points required for level 1
        float expMultiplier = 1.5f;  // Experience points multiplier for each level

        // Calculate the required experience points for the given level
        int requiredExp = Mathf.RoundToInt(baseExp * Mathf.Pow(expMultiplier, level - 1));
        return requiredExp;
    }

    private void LevelUp()
    {
        int currentLevel = (currentExp / maxExp) + 1;
        Debug.Log("Level Up! Current Level: " + currentLevel);

        int requiredExpForNextLevel = GetRequiredExpForLevel(currentLevel + 1);
        maxExp = requiredExpForNextLevel;

        currentExp = currentExp % maxExp;
    }


}
