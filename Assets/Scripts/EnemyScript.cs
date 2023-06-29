using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyUIScript enemyUI;
    public GeneralUIScript generalUI;

    public float maxHealth;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        enemyUI.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
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


}
