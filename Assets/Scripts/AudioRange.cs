using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRange : MonoBehaviour
{
    public GameObject player1Camera;  // Reference to the player camera GameObject
    public float detectionRange = 10f;  // Set range for detection
    public float minVolume = 0.0f;     // Minimum volume value
    public float maxVolume = 0.5f;     // Maximum volume value

    void Update()
    {
        if (player1Camera == null) return; // If player1Camera is not assigned, exit

        // Cast a sphere from the camera's position to detect objects in front of it
        RaycastHit hit;
        Vector3 p1 = player1Camera.transform.position;  // Get the position of Player1Camera

        // Perform the spherecast to detect objects within detectionRange
        if (Physics.SphereCast(p1, 0.5f, player1Camera.transform.forward, out hit, detectionRange))  // Adjust radius if needed
        {
            // Check if the hit object has an AudioSource component
            AudioSource audioSource = hit.collider.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                // Calculate distance to the object and adjust volume based on that
                float distance = hit.distance;
                float volume = Mathf.Lerp(maxVolume, minVolume, distance / detectionRange);

                // Apply the calculated volume
                audioSource.volume = volume;
            }
        }
    }
}
