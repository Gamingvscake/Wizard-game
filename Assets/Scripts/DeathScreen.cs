using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform Player1Camera; // Reference to the camera transform
    public Vector3 targetPosition = new Vector3(0, 45, 0); // Target position
    public float moveSpeed = 5f; // Speed at which the camera moves

    private bool moveCamera = false; // Flag to trigger the camera movement

    void Start()
    {
        // Ensure the camera starts at (0, 0, 0)
        Player1Camera.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        // Start moving the camera once the game begins (or on any specific event)
        if (!moveCamera)
        {
            moveCamera = true; // Set the flag to true to start moving the camera
        }

        // Move the camera if the moveCamera flag is true
        if (moveCamera)
        {
            // Smoothly move the camera from its current position to the target position
            Player1Camera.position = Vector3.MoveTowards(Player1Camera.position, targetPosition, moveSpeed * Time.deltaTime);

            // Optional: Stop the movement once it reaches the target position
            if (Player1Camera.position == targetPosition)
            {
                moveCamera = false; // Stop the camera from moving
                Debug.Log("Camera has reached the target position.");
            }
        }
    }

}
