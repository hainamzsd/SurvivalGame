using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public bool isDead = false;

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

   
    private EnemyAnimation m_animation;

    private Vector3 currentPosition;
    [SerializeField]
    public SpriteRenderer spriteRenderer;
    private bool isFacingRight = true;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        m_animation = GetComponent<EnemyAnimation>();
        currentHealth = maxHealth;
        enemyUI.SetMaxHealth(maxHealth);

        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        currentPosition = transform.position;
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


    }
  


    void takeDamage()
    {
            m_animation.PlayHit();
            currentHealth = currentHealth - 20;
            enemyUI.SetHealth(currentHealth);
            generalUI.createPopUp(transform.position, "20", 2);
            die();

    }

    public void die()
    {
        Player player = FindObjectOfType<Player>();
        if (currentHealth <= 0)
        {
            if (player != null)
            {
                player.GainExp(20);
            }
            rb.velocity = new Vector2(0, 0);
            isDead = true;
            StartCoroutine(DestroyAfterDelay(1f)); // Wait for 2 seconds before destroying

           

        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        m_animation.PlayDead();

        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            takeDamage();
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

    private void Flip()
    {
        // Flip the enemy horizontally
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
    }

    private void ChasePlayer()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            if(isDead != true)
            {
                m_animation.PlayWalk();

                Vector2 direction = player.transform.position - transform.position;
                rb.velocity = direction.normalized * chaseSpeed;
                if (direction.x > 0 && !isFacingRight)
                {
                    Flip();
                }
                else if (direction.x < 0 && isFacingRight)
                {
                    Flip();
                }

                // Check if the player is out of the detection radius
                if (Vector2.Distance(currentPosition, player.transform.position) > detectionRadius)
                {
                    m_animation.PlayIdle();
                    isPlayerDetected = false;
                    rb.velocity = Vector2.zero;
                }
                // Check if the enemy is close enough to attack
                else if (direction.magnitude <= attackRange)
                {
                    m_animation.PlayAttack();
                    AttackPlayer();
                }
            }
           
        }
    }

    private void AttackPlayer()
    {
        Player player = FindObjectOfType<Player>();

        // Perform the attack on the player
        // Replace this with your own code to damage the player or trigger any other attack-related actions

        player.TakeDamage(20);
        isAttacking = true;
        rb.velocity = Vector2.zero;
        
        // Wait for some time before allowing the enemy to attack again

        float attackCooldown = 1f;
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
