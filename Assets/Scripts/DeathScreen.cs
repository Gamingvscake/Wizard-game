using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{

    public PlayerInflicts playerInflicts;
    public float moveSpeed = 5f;            

    private bool isGameOver = false;        // Flag to check if the health is zero
    private Vector3 targetPosition = new Vector3(0, 45, 0); // Target position for this GameObject

    private void Update()
    {
        
        if (playerInflicts.PlayerCurrentHealth <= 0 && !isGameOver)
        {
            isGameOver = true;
        }

        
        if (isGameOver)
        {
            gameObject.transform.position = Vector3.Lerp(
                gameObject.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }
    }

}
