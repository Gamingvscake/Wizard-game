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
        // Instantiate the object at the throw point
        GameObject thrownObject = Instantiate(objectToThrow, throwPoint.position, Quaternion.identity);

        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Thrown object needs a Rigidbody!");
            return;
        }

        Vector3 throwDirection = playerCamera.forward;

        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        Vector3 randomTorqueVector = new Vector3(
            Random.Range(-randomTorque, randomTorque),
            Random.Range(-randomTorque, randomTorque),
            Random.Range(-randomTorque, randomTorque)
        );
        rb.AddTorque(randomTorqueVector, ForceMode.Impulse);
    }
}