using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Reference to the player's transform
    public Vector3 offset;    // Offset of the camera from the player

    void LateUpdate()
    {
        if (target != null)
        {
            // Set the position of the camera to the player's position + offset
            transform.position = target.position + offset;

            // Ensure the camera looks at the player's position without rotating in the x-axis
            Vector3 lookAtPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(lookAtPosition);
        }
    }
}



