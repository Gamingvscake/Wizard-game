using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{

    public PlayerInflicts playerInflicts;
    public bool isDead;
    public float moveSpeed = 5f;            

    public bool isGameOver = false;        // Flag to check if the health is zero
    private Vector3 targetPosition = new Vector3(0, 80, 0); // Target position for this GameObject

    private void Update()
    {
        
        if (playerInflicts.PlayerCurrentHealth <= 0 && !isDead)
        {
            isDead = true;
        }

        if (isDead)
        {

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
