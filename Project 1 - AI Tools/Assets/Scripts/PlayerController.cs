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




