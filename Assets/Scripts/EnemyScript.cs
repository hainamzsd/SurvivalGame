using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyUIScript enemyUI;
    public GeneralUIScript generalUI;

    public float maxHealth;
    public float currentHealth;

    //behaviour
    public float chaseSpeed = 3f;        // Speed at which the enemy chases the player
    public float attackRange = 1f;       // Range at which the enemy attacks the player
    public float attackDamage = 10f;     // Amount of damage done by the enemy's attack
    public float detectionRadius = 10f;   // Radius within which the enemy can detect the player

    private Rigidbody2D rb;
    private bool isAttacking = false;
    private bool isPlayerDetected = false;
    private Vector3 initialPosition;


  

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        enemyUI.SetMaxHealth(maxHealth);

        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }


    private void Update()
    {

        if (!isAttacking)
        {
            if (!isPlayerDetected)
            {
                DetectPlayer();
            }
            else
            {
                ChasePlayer();
            }
        }


        takeDamage();
        die();
    }
  


    void takeDamage()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentHealth = currentHealth - 20;
            enemyUI.SetHealth(currentHealth);
            generalUI.createPopUp(transform.position, "20", 2);
        }
    }

    void die()
    {
        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    private void DetectPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                isPlayerDetected = true;
                break;
            }
        }
    }

    private void ChasePlayer()
    {
        Player player = FindObjectOfType<Player>(); 
        if (player != null)
        {
            Vector2 direction = player.transform.position - transform.position;
            rb.velocity = direction.normalized * chaseSpeed;

            // Check if the player is out of the detection radius
            if (Vector2.Distance(initialPosition, player.transform.position) > detectionRadius)
            {
                isPlayerDetected = false;
                rb.velocity = Vector2.zero;
            }
            // Check if the enemy is close enough to attack
            else if (direction.magnitude <= attackRange)
            {
                AttackPlayer();
            }
        }
    }

    private void AttackPlayer()
    {
        // Perform the attack on the player
        // Replace this with your own code to damage the player or trigger any other attack-related actions
        Debug.Log("Enemy attacks player!");

        isAttacking = true;
        rb.velocity = Vector2.zero;

        // Wait for some time before allowing the enemy to attack again
        float attackCooldown = 2f;
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
