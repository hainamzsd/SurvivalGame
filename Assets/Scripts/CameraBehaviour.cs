using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement
    public Vector3 offset; // Offset from the player's position

    private void LateUpdate()
    {
        // Calculate the desired position for the camera
        Vector3 desiredPosition = player.position + offset;
        desiredPosition.z = transform.position.z; // Maintain the camera's original z-position

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
