using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal; // si usas URP

public class BrokenSpotlight : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    [SerializeField] private float intensityNormal = 30f;

    [Header("Blinking")]
    [SerializeField] private float durationBlinking = 2f;
    [SerializeField] private float intervalMin = 0.05f;
    [SerializeField] private float intervalMax = 0.2f;

    [Header("Final")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip electricityClip;
    [SerializeField] private AudioClip clipBreakdown;
    private bool broken = false;

    public void DestroySpotlight()
    {
        if (broken || (audioSource != null && audioSource.isPlaying)) return;

        broken = true;
        if (audioSource != null && electricityClip != null)
        {
            audioSource.clip = electricityClip;
            audioSource.loop = true;
            audioSource.volume = 0.6f;
            audioSource.pitch = 1f;
            audioSource.Play();
        }
        
        StartCoroutine(SecuenceBreakdown());
    }

    private IEnumerator SecuenceBreakdown()
    {
        float time = 0f;

        while (time < durationBlinking)
        {
            foreach (Light l in lights)
            {
                l.intensity = Random.Range(0f, intensityNormal * Random.Range(0.7f, 1.2f));
            }

            float interval = Random.Range(intervalMin, intervalMax);
            yield return new WaitForSeconds(interval);

            time += interval;

            if (audioSource != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.volume = Random.Range(0.4f, 0.8f);
            }
        }

        float timeSync = 0f;
        float durationSync = 0.5f; // ajustá esto

        while (timeSync < durationSync)
        {
            bool on = Random.value > 0.5f;

            foreach (Light l in lights)
            {
                l.intensity = on ? intensityNormal : intensityNormal * 0.2f;
            }

            float interval = Random.Range(0.03f, 0.08f);
            yield return new WaitForSeconds(interval);

            timeSync += interval;

            if (audioSource != null)
            {
                audioSource.pitch = 1.3f;
                audioSource.volume = 1f;
            }
        }

        // flash final
        foreach (Light l in lights)
        {
            l.intensity = intensityNormal * 2f;
        }
        yield return new WaitForSeconds(0.05f);

        // Apagado definitivo
        foreach (Light l in lights)
        {
            l.intensity = 0f;
            l.enabled = false;
        }


        if (audioSource != null)
        {
            audioSource.Stop();
        }

        yield return new WaitForSeconds(0.03f);

        // sonido de ruptura
        if (audioSource != null && clipBreakdown != null)
        {
            audioSource.loop = false;
            audioSource.clip = clipBreakdown;
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    private void Update()
    {

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            DestroySpotlight();
        }
    }
}