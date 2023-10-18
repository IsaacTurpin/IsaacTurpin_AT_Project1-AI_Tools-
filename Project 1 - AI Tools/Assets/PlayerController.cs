using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public float walkSpeed = 2f;  // Walk speed of the player
    public float runSpeed = 5f;   // Run speed of the player
    public int attackDamage = 20; // Damage amount for each attack

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Player movement logic
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Determine movement speed (walk or run) based on player input
        float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Determine if the player is walking left, right, backward, or forward
        bool isWalkingLeft = (horizontalInput < 0f && !Input.GetKey(KeyCode.LeftShift));
        bool isWalkingRight = (horizontalInput > 0f && !Input.GetKey(KeyCode.LeftShift));
        bool isWalkingBack = (verticalInput < 0f && !Input.GetKey(KeyCode.LeftShift));
        bool isRunningBack = (verticalInput < 0f && Input.GetKey(KeyCode.LeftShift));

        // Set animator parameters based on player movement
        animator.SetBool("IsWalking", (horizontalInput != 0f || verticalInput != 0f) && !isWalkingBack && !isRunningBack && !isWalkingLeft && !isWalkingRight);
        animator.SetBool("IsRunning", (Input.GetKey(KeyCode.LeftShift) && (horizontalInput != 0f || verticalInput != 0f)) && !isWalkingBack && !isRunningBack && !isWalkingLeft && !isWalkingRight);
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
        }

        // Reset the attack trigger to prevent continuous attacks
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.ResetTrigger("AttackTrigger");
        }
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
}




