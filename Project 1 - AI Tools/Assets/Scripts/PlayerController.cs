using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public float walkSpeed = 2f;  // Walk speed of the player
    public float runSpeed = 5f;   // Run speed of the player
    public int attackDamage = 20; // Damage amount for each attack
    public AudioClip swordSwingSoundClip;
    public AudioClip swordHitSoundClip;

    private AudioSource audioSource;


    void Start()
    {
        animator = GetComponent<Animator>();
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If AudioSource is not found, add it to the GameObject
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the default clip for the AudioSource
        audioSource.clip = swordSwingSoundClip;
    }

    void Update()
    {
        // Player movement logic
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Determine movement speed (walk or run) based on player input
        float moveSpeed = GetMoveSpeed();

        // Determine if the player is walking left, right, backward, or forward
        bool isWalkingLeft = (horizontalInput < 0f && !IsRunning());
        bool isWalkingRight = (horizontalInput > 0f && !IsRunning());
        bool isWalkingBack = (verticalInput < 0f && !IsRunning());
        bool isRunningBack = (verticalInput < 0f && IsRunning());

        // Set animator parameters based on player movement
        animator.SetBool("IsWalking", (horizontalInput != 0f || verticalInput != 0f) && !isWalkingBack && !isRunningBack && !isWalkingLeft && !isWalkingRight);
        animator.SetBool("IsRunning", IsRunning() && (horizontalInput != 0f || verticalInput != 0f) && !isWalkingBack && !isRunningBack && !isWalkingLeft && !isWalkingRight);
        animator.SetBool("IsWalkingLeft", isWalkingLeft);
        animator.SetBool("IsWalkingRight", isWalkingRight);
        animator.SetBool("IsWalkingBack", isWalkingBack);
        animator.SetBool("IsRunningBack", isRunningBack);

        // Player movement
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Player attack logic
        if (Input.GetButtonDown("Fire1"))
        {
            // Trigger the attack animation
            animator.SetTrigger("AttackTrigger");
            PlaySwordSwingSound();
        }

        // Reset the attack trigger to prevent continuous attacks
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.ResetTrigger("AttackTrigger");
        }
    }

    float GetMoveSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey("joystick button 4"))  // "joystick button 4" corresponds to the LB button on most controllers
        {
            return runSpeed;
        }
        return walkSpeed;
    }

    bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey("joystick button 4");
    }


    // Called by Animation Event at the moment of the attack animation
    void DealDamage()
    {
        // Detect enemies in front of the player using a raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            
            // Check if the hit object has an Enemy component
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Play the sword hit sound when dealing damage
                PlaySwordHitSound();
                // Deal damage to the enemy
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    public void AttackAnimationEvent()
    {
        // This method will be called from the animation event
        DealDamage();
    }

    void PlaySwordSwingSound()
    {
        // Play the sword swing sound
        audioSource.clip = swordSwingSoundClip;
        audioSource.Play();
    }

    void PlaySwordHitSound()
    {
        // Play the sword hit sound
        audioSource.clip = swordHitSoundClip;
        audioSource.Play();
    }
}




