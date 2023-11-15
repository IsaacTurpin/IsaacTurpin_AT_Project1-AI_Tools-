using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public int maxHealth = 500;
    private int currentHealth;
    public float movementSpeed = 3f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    private Transform player;
   // public Canvas canvas; // Reference to the canvas

    // Assign your health bar prefab in the Unity Editor
    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    private HealthBar healthBar; // Assuming you named the script as HealthBar





    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        healthBar = healthBarPrefab.GetComponent<HealthBar>();

        /* if (healthBarPrefab != null)
         {
             // Instantiate Health Bar
             healthBarInstance = Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity);
             healthBarInstance.transform.SetParent(canvas.transform, false);

             // Assuming the HealthBar script is attached to the healthBarPrefab
             healthBar = healthBarInstance.GetComponent<HealthBar>();
         }
         else
         {
             Debug.LogError("Health bar prefab is not assigned in the Unity Editor.");
         } */
    }

    void Update()
    {
        if (player != null)
        {
            // Boss follows the player
            Vector3 directionToPlayer = player.position - transform.position;

            if (directionToPlayer.magnitude > attackRange)
            {
                // Move towards the player if not in attack range
                transform.Translate(directionToPlayer.normalized * movementSpeed * Time.deltaTime, Space.World);

                // Make the enemy face the player
                transform.LookAt(player);

                // Set the IsMoving parameter in the Animator Controller to control walking animation
                animator.SetBool("IsMoving", true);
                animator.SetBool("IsAttacking", false);
            }
            else
            {
                // Stop moving if in attack range
                animator.SetBool("IsMoving", false);

                // Attack the player if in attack range
                if (Time.time >= nextAttackTime)
                {
                    AttackPlayer();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }

            // Update Health Bar Position
           // Vector3 healthBarPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);
           // healthBarInstance.transform.position = healthBarPos;
        }
       // else
       // {
            // If the player is null, destroy the health bar
        //    Destroy(healthBarInstance);
       // }
    }

    void AttackPlayer()
    {
        // Perform boss's attack logic here
        // For example, dealing damage to the player or triggering attack animations

        // Set the IsAttacking parameter in the Animator Controller to trigger attack animation
        animator.SetBool("IsAttacking", true);
    }

    // Animation Event method for dealing damage
    void DealDamage()
    {
        // Deal damage to the player
        int damage = 50;
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Add logic for handling damage effects, animations, or boss rage behavior here

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add logic for boss death behavior (e.g., play death animation, drop items, etc.)
        Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        healthBar.SetHealth(fillAmount);
    }


}





