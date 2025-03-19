using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{
    public AudioSource crystalSound;

    private void Start()
    {
        if (crystalSound != null)
        {
            crystalSound.Play();
        }
        else
        {
            Debug.LogWarning("crystalSound is not assigned.");
        }
    }
}
