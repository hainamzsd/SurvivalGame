using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    bool isIdle = true;
    bool isHit = false;
    bool isDead = false;
    bool isWalking = false;
    bool isAttack = false;

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;


    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_animator.SetBool("Grounded", true);
    }

    // Update is called once per frame
    void Update()
    {

        // -- Handle input and movement --
        //float inputX = Input.GetAxis("Horizontal");

        //// Swap direction of sprite depending on walk direction
        //if (inputX > 0)
        //{
        //    GetComponent<SpriteRenderer>().flipX = false;
        //    m_facingDirection = 1;
        //}

        //else if (inputX < 0)
        //{
        //    GetComponent<SpriteRenderer>().flipX = true;
        //    m_facingDirection = -1;
        //}

        // -- Handle Animations --

        //Death
        //if (isDead)
        //{
        //    m_animator.SetBool("noBlood", m_noBlood);
        //    m_animator.SetTrigger("Death");
        //}

        ////Hurt
        //else
        if (isHit)
        {
            m_animator.SetTrigger("Hurt");
            isHit = false;
        }

        //Attack
        else if (isAttack)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + "3");

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        //// Block
        //else if (Input.GetMouseButtonDown(1) && !m_rolling)
        //{
        //    m_animator.SetTrigger("Block");
        //    m_animator.SetBool("IdleBlock", true);
        //}

        //else if (Input.GetMouseButtonUp(1))
        //    m_animator.SetBool("IdleBlock", false);

        //Run
        else if (isWalking)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        //else
        //{
        //    // Prevents flickering transitions to idle
        //    m_delayToIdle -= Time.deltaTime;
        //    if (m_delayToIdle < 0)
        //        m_animator.SetInteger("AnimState", 0);
        //}
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    public void PlayIdle()
    {
        if (isDead) return;

        m_animator.SetInteger("AnimState", 0);
    }

    public void PlayWalk()
    {
        if (isDead) return;

        m_animator.SetInteger("AnimState", 1);
    }

    public void PlayAttack()
    {
        if (isDead) return;

        m_animator.SetTrigger("Attack" + "3");
    }

    public void PlayHit()
    {
        if (isDead) return;
        m_animator.SetTrigger("Hurt");
    }

    public void PlayDead()
    {
        if (isDead) return;

        m_animator.SetBool("noBlood", false);
        m_animator.SetTrigger("Death");
        isDead = true;
        Debug.Log(2);
    }
}
