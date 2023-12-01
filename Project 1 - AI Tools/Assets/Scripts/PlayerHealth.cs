using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private Animator animator;
    private bool hasDied = false;

    public Slider healthSlider; // Reference to the UI slider

    void Start()
    {
        currentHealth = maxHealth;

        // Get the Animator component
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player.");
        }

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
        // Check if the player has already died to avoid multiple calls
        if (hasDied)
        {
            return;
        }

        // Add logic for player death behavior (e.g., play death animation, game over screen, etc.)

        // Play the player death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Set a flag to avoid multiple calls
        hasDied = true;
    }

    // Called by an Animation Event when the player death animation is complete
    void PlayerDieAnimationFinished()
    {
        // Restart the level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


