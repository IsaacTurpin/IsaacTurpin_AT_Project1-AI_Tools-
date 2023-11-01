using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float rotationSpeed = 3f; // Speed of camera rotation using mouse
    public float controllerRotationSpeed = 150f; // Speed of camera rotation using controller right stick
    public float smoothness = 0.1f; // Smoothing factor for camera movement

    void LateUpdate()
    {
        // Rotate the player based on mouse movement
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        player.Rotate(Vector3.up * mouseX);

        // Rotate the player based on controller right stick movement
        float rightStickX = Input.GetAxis("RightStickX") * controllerRotationSpeed * Time.deltaTime;
        player.Rotate(Vector3.up * rightStickX);

        // Calculate the desired camera position based on player's position
        Vector3 targetPosition = player.position - player.forward * 3f + Vector3.up * 2f; // Example values, adjust as needed

        // Smoothly interpolate the camera's position towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness);

        // Ensure the camera looks at the player's position without rotating in the x-axis
        Vector3 lookAtPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookAtPosition);
    }
}












