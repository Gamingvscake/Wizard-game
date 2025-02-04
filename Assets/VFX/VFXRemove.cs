using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXRemove : MonoBehaviour
{
    public int LifeTime;

    void Update()
    {
        StartCoroutine(Lifespan());
    }

    IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }
}
