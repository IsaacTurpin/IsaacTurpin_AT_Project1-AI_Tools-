using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
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

        // Add logic for handling damage effects, such as screen shake, sound effects, or UI updates

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add logic for player death behavior (e.g., play death animation, game over screen, etc.)
        // For now, let's simply deactivate the player object
        gameObject.SetActive(false);
    }
}

