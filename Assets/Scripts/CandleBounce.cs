using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleBounce : MonoBehaviour
{
    [Tooltip("Minimum light intensity")]
    public float MinIntensity = 0f;
    [Tooltip("Maximum light intensity")]
    public float MaxIntensity = 1f;
    [Tooltip("Amount of flickering")]
    public int Smoothing = 5;
    [Tooltip("Movement of the light source")]
    public float RandomMovement = 0.01f;

    [Tooltip("Time to wait before fade-in starts")]
    public float fadeInWaitTime = 2f; // Wait time before fade-in starts
    [Tooltip("Duration of the fade-in")]
    public float fadeInDuration = 1f; // Duration of the fade-in
    [Tooltip("Target range after fade-in")]
    public float targetRange = 14f; // Final range after fade-in

    private Light[] lights;
    private new Renderer[] renderer;
    private Color[] materialEmissionColors;
    private Vector3[] originalPositions;
    private Queue<float> SmoothQueue;
    private float LastSum;

    void Start()
    {
        lights = gameObject.GetComponentsInChildren<Light>();
        renderer = gameObject.GetComponentsInChildren<Renderer>();
        materialEmissionColors = new Color[renderer.Length];

        originalPositions = new Vector3[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            originalPositions[i] = lights[i].transform.position;

            // Set the initial range to 0 for the fade-in effect
            lights[i].range = 0f;
        }

        for (int i = 0; i < renderer.Length; i++)
        {
            if (renderer[i].material.HasProperty("_EmissionColor"))
            {
                materialEmissionColors[i] = renderer[i].material.GetColor("_EmissionColor");
            }
            else
            {
                materialEmissionColors[i] = Color.black;
            }
        }

        SmoothQueue = new Queue<float>(Smoothing);

        // Start the fade-in coroutine
        StartCoroutine(FadeInLightRange());
    }

    public void Reset()
    {
        SmoothQueue.Clear();
        LastSum = 0;
    }

    void Update()
    {
        while (SmoothQueue.Count >= Smoothing)
        {
            LastSum -= SmoothQueue.Dequeue();
        }

        float newVal = Random.Range(MinIntensity, MaxIntensity);
        SmoothQueue.Enqueue(newVal);
        LastSum += newVal;

        SetIntensity(LastSum / (float)SmoothQueue.Count);
    }

    void SetIntensity(float intensity)
    {
        float value = ((MaxIntensity - MinIntensity) - (MaxIntensity - intensity)) / (MaxIntensity - MinIntensity);

        // Adjust material emission color
        for (int i = 0; i < renderer.Length; i++)
        {
            renderer[i].material.SetColor("_EmissionColor", Color.Lerp(Color.black, materialEmissionColors[i], value));
        }

        // Adjust light intensity and position
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = intensity;

            // Adjust light range based on intensity, doubling the overall range
            float minRange = 5f; // Minimum light range
            float maxRange = 10f; // Maximum light range
            float doubledRange = Mathf.Lerp(minRange, maxRange, value) * 2; // Double the overall range
            lights[i].range = Mathf.Clamp(lights[i].range, 0, doubledRange);

            // Add slight random movement to the light source
            float movement = intensity * RandomMovement;
            lights[i].transform.position = originalPositions[i] + (Vector3.up * movement);
        }
    }

    private IEnumerator FadeInLightRange()
    {
        // Wait before starting the fade-in
        yield return new WaitForSeconds(fadeInWaitTime);

        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeInDuration;

            // Smoothly interpolate the range to the target value
            foreach (Light light in lights)
            {
                light.range = Mathf.Lerp(0, targetRange, t);
            }

            yield return null;
        }

        // Ensure the lights reach the target range
        foreach (Light light in lights)
        {
            light.range = targetRange;
        }
    }
}