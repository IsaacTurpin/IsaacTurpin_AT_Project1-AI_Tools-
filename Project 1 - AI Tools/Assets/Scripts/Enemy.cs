using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 500;
    private int currentHealth;
    public float movementSpeed = 3f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    private Transform player;
    public Slider healthBarSlider;

    private bool hasDied = false;
    public GameObject deathParticlePrefab; // Reference to the particle effect prefab

    public AudioClip attackPhase1SoundClip;
    public AudioClip attackPhase2SoundClip;
    public AudioClip attackPhase3SoundClip;
    public AudioClip deathSoundClip;  // Add a new public variable for the death sound clip

    private AudioSource audioSource;


    public enum AttackPhase
    {
        Phase1,
        Phase2,
        Phase3
    }

    public AttackPhase currentPhase = AttackPhase.Phase1;

    // Damage values for each phase
    public int damagePhase1 = 50;
    public int damagePhase2 = 75;
    public int damagePhase3 = 100;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (healthBarSlider == null)
        {
            Debug.LogError("Health bar slider is not assigned in the inspector.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            
            if (directionToPlayer.magnitude > attackRange)
            {
                MoveTowardsPlayer(directionToPlayer);
            }
            else
            {
                StopMoving();

                if (Time.time >= nextAttackTime)
                {
                    // Check health thresholds to determine the current phase
                    if (currentHealth > maxHealth * 0.66f)
                    {
                        currentPhase = AttackPhase.Phase1;
                    }
                    else if (currentHealth > maxHealth * 0.33f)
                    {
                        currentPhase = AttackPhase.Phase2;
                    }
                    else
                    {
                        currentPhase = AttackPhase.Phase3;
                    }

                    // Choose the appropriate attack based on the current phase
                    switch (currentPhase)
                    {
                        case AttackPhase.Phase1:
                            AttackPlayerPhase1();
                            break;

                        case AttackPhase.Phase2:
                            AttackPlayerPhase2();
                            break;

                        case AttackPhase.Phase3:
                            AttackPlayerPhase3();
                            break;
                    }

                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
    }

    void MoveTowardsPlayer(Vector3 directionToPlayer)
    {
        transform.Translate(directionToPlayer.normalized * movementSpeed * Time.deltaTime, Space.World);
        transform.LookAt(player);
        animator.SetBool("IsMoving", true);

        // Reset attack triggers
        animator.ResetTrigger("AttackPhase1");
        animator.ResetTrigger("AttackPhase2");
        animator.ResetTrigger("AttackPhase3");
    }


    void StopMoving()
    {
        animator.SetBool("IsMoving", false);
    }

    void AttackPlayerPhase1()
    {
        if (IsPlayerInRange() && !IsObstacleBetweenPlayer())
        {
            animator.SetTrigger("AttackPhase1");
        }
        else
        {
            animator.ResetTrigger("AttackPhase1");  // Reset the trigger if not attacking
        }
    }

    void AttackPlayerPhase2()
    {
        if (IsPlayerInRange() && !IsObstacleBetweenPlayer())
        {
            animator.SetTrigger("AttackPhase2");
        }
        else
        {
            animator.ResetTrigger("AttackPhase2");  // Reset the trigger if not attacking
        }
    }

    void AttackPlayerPhase3()
    {
        if (IsPlayerInRange() && !IsObstacleBetweenPlayer())
        {
            animator.SetTrigger("AttackPhase3");

        }
        else
        {
            animator.ResetTrigger("AttackPhase3");  // Reset the trigger if not attacking
        }
    }

    void PlayAttackSound(AudioClip soundClip)
    {
        if (soundClip != null)
        {
            audioSource.clip = soundClip;
            audioSource.Play();
        }
    }

    AudioClip GetAttackSoundForCurrentPhase()
    {
        // Return the appropriate sound clip based on the current phase
        switch (currentPhase)
        {
            case AttackPhase.Phase1:
                return attackPhase1SoundClip;

            case AttackPhase.Phase2:
                return attackPhase2SoundClip;

            case AttackPhase.Phase3:
                return attackPhase3SoundClip;

            default:
                return null;
        }
    }

    // Animation Event method for dealing damage
    void DealDamage()
    {
        // Check if the player is still in attack range
        if (IsPlayerInRange())
        {
            int damage = GetDamageForCurrentPhase();
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Play the corresponding attack sound effect
                PlayAttackSound(GetAttackSoundForCurrentPhase());

                //Debug.Log(damage);
                playerHealth.TakeDamage(damage);
            }
        }
    }

    int GetDamageForCurrentPhase()
    {
        // Modify this method to return different damage amounts based on the current phase
        switch (currentPhase)
        {
            case AttackPhase.Phase1:
                return damagePhase1;  // Adjust the damage amount for Phase 1

            case AttackPhase.Phase2:
                return damagePhase2;  // Adjust the damage amount for Phase 2

            case AttackPhase.Phase3:
                return damagePhase3;  // Adjust the damage amount for Phase 3

            default:
                return 0;
        }
    }


    bool IsPlayerInRange()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        return directionToPlayer.magnitude <= attackRange;
    }

    bool IsObstacleBetweenPlayer()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - transform.position;

        return Physics.Raycast(transform.position, directionToPlayer, out hit, attackRange) &&
               !hit.collider.CompareTag("Player");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        // Check if the enemy has already died to avoid multiple calls
        if (hasDied)
        {
            return;
        }

        // Add logic for boss death behavior (e.g., play death animation, drop items, etc.)

        // Play the death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Play the death sound effect
        PlayDeathSound();

        // Set a flag to avoid multiple calls
        hasDied = true;
    }

    // Called by an Animation Event when the death animation is complete
    void DieAnimationFinished()
    {
        // Spawn the death particle effect
        if (deathParticlePrefab != null)
        {
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        }

        // Destroy the GameObject
        Destroy(gameObject);
    }

    void PlayDeathSound()
    {
        // Play the death sound effect
        if (deathSoundClip != null)
        {
            AudioSource.PlayClipAtPoint(deathSoundClip, transform.position);
        }
    }

    void UpdateHealthBar()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        healthBarSlider.value = fillAmount;
    }
}







