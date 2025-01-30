using UnityEngine;

public class TempThrow : MonoBehaviour
{
    [Header("Throw Settings")]
    public GameObject objectToThrow; // Prefab to throw
    public Transform throwPoint; // The spawn position of the object (should be child of camera)
    public Transform playerCamera; // Assign the player's camera here
    public float throwForce = 10f; // Strength of the throw
    public float randomTorque = 10f; // Random rotation force

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Throw();
        }
    }

    void Throw()
    {
        if (objectToThrow == null || throwPoint == null || playerCamera == null)
        {
            Debug.LogError("Assign 'objectToThrow', 'throwPoint', and 'playerCamera' in the Inspector!");
            return;
        }

        // Instantiate the object at the throw point
        GameObject thrownObject = Instantiate(objectToThrow, throwPoint.position, Quaternion.identity);

        // Ensure it has a Rigidbody
        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Thrown object needs a Rigidbody!");
            return;
        }

        // Get the direction the camera is facing (this includes looking up/down)
        Vector3 throwDirection = playerCamera.forward;

        // Apply forward force in the direction the camera is looking
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        // Apply random rotation for a natural airborne effect
        Vector3 randomTorqueVector = new Vector3(
            Random.Range(-randomTorque, randomTorque),
            Random.Range(-randomTorque, randomTorque),
            Random.Range(-randomTorque, randomTorque)
        );
        rb.AddTorque(randomTorqueVector, ForceMode.Impulse);
    }
}