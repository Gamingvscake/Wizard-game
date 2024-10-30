using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform PlayerTransform;
    private void Update()
    {
        transform.LookAt(PlayerTransform);
    }
}
