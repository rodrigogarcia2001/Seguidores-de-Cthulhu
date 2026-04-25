using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal; // si usas URP

public class FocoRoto : MonoBehaviour
{
    [SerializeField] private Light[] luces;
    [SerializeField] private float intensidadNormal = 30f;

    [Header("Parpadeo")]
    [SerializeField] private float duracionParpadeo = 2f;
    [SerializeField] private float intervaloMin = 0.05f;
    [SerializeField] private float intervaloMax = 0.2f;

    [Header("Final")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip clipElectricidad;
    [SerializeField] private AudioClip clipRuptura;
    private bool roto = false;

    public void RomperFoco()
    {
        if (roto || (audioSource != null && audioSource.isPlaying)) return;

        roto = true;
        if (audioSource != null && clipElectricidad != null)
        {
            audioSource.clip = clipElectricidad;
            audioSource.loop = true;
            audioSource.volume = 0.6f;
            audioSource.pitch = 1f;
            audioSource.Play();
        }
        
        StartCoroutine(SecuenciaRuptura());
    }

    private IEnumerator SecuenciaRuptura()
    {
        float tiempo = 0f;

        while (tiempo < duracionParpadeo)
        {
            foreach (Light l in luces)
            {
                l.intensity = Random.Range(0f, intensidadNormal * Random.Range(0.7f, 1.2f));
            }

            float intervalo = Random.Range(intervaloMin, intervaloMax);
            yield return new WaitForSeconds(intervalo);

            tiempo += intervalo;

            if (audioSource != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.volume = Random.Range(0.4f, 0.8f);
            }
        }

        float tiempoSync = 0f;
        float duracionSync = 0.5f; // ajustá esto

        while (tiempoSync < duracionSync)
        {
            bool encendida = Random.value > 0.5f;

            foreach (Light l in luces)
            {
                l.intensity = encendida ? intensidadNormal : intensidadNormal * 0.2f;
            }

            float intervalo = Random.Range(0.03f, 0.08f);
            yield return new WaitForSeconds(intervalo);

            tiempoSync += intervalo;

            if (audioSource != null)
            {
                audioSource.pitch = 1.3f;
                audioSource.volume = 1f;
            }
        }

        // flash final
        foreach (Light l in luces)
        {
            l.intensity = intensidadNormal * 2f;
        }
        yield return new WaitForSeconds(0.05f);

        // Apagado definitivo
        foreach (Light l in luces)
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
        if (audioSource != null && clipRuptura != null)
        {
            audioSource.loop = false;
            audioSource.clip = clipRuptura;
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    private void Update()
    {

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            RomperFoco();
        }
    }
}