using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthSlider; // Reference to the UI slider

    void Start()
    {
        currentHealth = maxHealth;

        // Make sure to call this method to initialize the UI slider
        InitializeHealthBar();
    }

    void InitializeHealthBar()
    {
        // Update the UI slider value based on the player's current health
        float fillAmount = (float)currentHealth / maxHealth;
        healthSlider.value = fillAmount;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Add logic for handling damage effects, such as screen shake, sound effects, or UI updates

        // Make sure health doesn't go below zero
        currentHealth = Mathf.Max(currentHealth, 0);

        // Update the UI slider value based on the player's current health
        float fillAmount = (float)currentHealth / maxHealth;
        healthSlider.value = fillAmount;

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


