using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskExploder : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LevelGeom"))
        {
            Destroy(gameObject);
            Debug.Log("bruh");
        }
    }
}
