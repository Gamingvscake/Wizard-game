using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderScript : MonoBehaviour
{
    public Transform EnterObject;
    public Vector3 EnterPoint;
    private void Start()
    {
        EnterPoint = EnterObject.position;
    }
}
