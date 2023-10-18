using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Add logic for handling damage effects, animations, or death behavior here

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add logic for death behavior (e.g., play death animation, drop items, etc.)
        Destroy(gameObject);
    }
}

