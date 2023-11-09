using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 500;
    private int currentHealth;
    public float movementSpeed = 3f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    private Transform player;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
            }
            else
            {
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
        // Perform boss's attack logic here
        // For example, dealing damage to the player or triggering attack animations

        // For demonstration, let's assume the boss deals 50 damage to the player
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
}


