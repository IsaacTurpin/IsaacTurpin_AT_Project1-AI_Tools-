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
    public Slider healthBarSlider; // Reference to the health bar slider

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (healthBarSlider == null)
        {
            Debug.LogError("Health bar slider is not assigned in the inspector.");
        }
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
        }
    }

    void AttackPlayer()
    {
        // Check if there's an obstacle between the enemy and the player using a raycast
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - transform.position;

        if (Physics.Raycast(transform.position, directionToPlayer, out hit, attackRange))
        {
            // Check if the obstacle is the player
            if (hit.collider.CompareTag("Player"))
            {
                // Perform boss's attack logic here
                // For example, dealing damage to the player or triggering attack animations

                // Set the IsAttacking parameter in the Animator Controller to trigger attack animation
                animator.SetBool("IsAttacking", true);

                // Animation Event will handle dealing damage
            }
            else
            {
                // Player is not in the line of sight, so do not attack
                animator.SetBool("IsAttacking", false);
            }
        }

        // Alternatively, you can modify the conditions as needed based on your game's requirements
    }


    // Animation Event method for dealing damage
    void DealDamage()
    {
        // Check if the player is still in attack range
        if (IsPlayerInRange())
        {
            // Deal damage to the player
            int damage = 50;
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    bool IsPlayerInRange()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        return directionToPlayer.magnitude <= attackRange;
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
        healthBarSlider.value = fillAmount;
    }


}





