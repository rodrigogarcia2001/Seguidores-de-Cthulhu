using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
public class PlayerDie : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject screenGameOver;

    [Header("Referencias")]
    [SerializeField] private Camera camera;
    [SerializeField] private Volume volume;
    [SerializeField] private MonoBehaviour controlCamera;
    [SerializeField] private CanvasGroup blackFade;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip initialMemory;
    [SerializeField] private AudioClip[] memories;
    [SerializeField] private AudioClip whistleFinal;
    [SerializeField] private AudioClip beat;

    private Bloom bloom;
    private Vignette vignette;
    Coroutine memoriesCoroutine;
    private bool die = false;

    void Start()
    {
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out vignette);
    }
    void Update()
    {
        //if (Keyboard.current.kKey.wasPressedThisFrame)
        //{
        //    Morir();
        //}
    }

    public void Morir()
    {
        if (die) return;
        die = true;

        StartCoroutine(SecuenceOfDeath());
    }

    IEnumerator SecuenceOfDeath()
    {

        StartCoroutine(ProggresiveBeats(3.5f));

        yield return new WaitForSecondsRealtime(3.5f);
        yield return null; // 1 frame
        StartCoroutine(ReduceBeatsSmooth());
        //  PRIMER sonido (el importante)
        StartCoroutine(ReproduceMemorySmooth());
        yield return new WaitForSecondsRealtime(0.2f);
        memoriesCoroutine = StartCoroutine(ReproduceMemories());


        if (controlCamera != null)
            controlCamera.enabled = false;

        GetComponentInChildren<MonoBehaviour>().enabled = false;//Evita que el jugaodr se mueva

        float time = 0f;
        float duration = 3f;

        float bloomInicial = bloom.intensity.value;
        float thresholdInicial = bloom.threshold.value;
        float vignetteInicial = vignette.intensity.value;
        float smoothInicial = vignette.smoothness.value;

        Vector3 rotInicial = camera.transform.eulerAngles;
        Vector3 rotFinal = new Vector3(-80f, rotInicial.y, rotInicial.z); // mirar al cielo

        while (time < duration)
        {
            float speed = 0.5f;
            time += Time.deltaTime * speed;
            float t = Mathf.SmoothStep(0f, 1f, time / duration);

            // 1. Rotar cámara hacia arriba
            camera.transform.eulerAngles = Vector3.Lerp(rotInicial, rotFinal, t);

            // 2. Aumentar bloom
            float tSuave = Mathf.SmoothStep(0f, 1f, t);
            bloom.intensity.value = Mathf.Lerp(bloomInicial, 2000f, tSuave);
            float tThreshold = 1f - Mathf.Pow(1f - tSuave, 3f); // más rápido
            bloom.threshold.value = Mathf.Lerp(thresholdInicial, 0f, tThreshold);

            // 3. Cerrar visión (viñeta)

            vignette.intensity.value = Mathf.Lerp(vignetteInicial, 0.6f, tSuave);
            vignette.smoothness.value = Mathf.Lerp(smoothInicial, 0.9f, tSuave);

            yield return null;
        }

        // bajar volume de memories
        StartCoroutine(ReduceAudio());

        yield return new WaitForSecondsRealtime(1.5f);
        if (memoriesCoroutine != null)
        {
            StopCoroutine(memoriesCoroutine);
        }
        // sonido final
        audioSource.Stop();
        audioSource.volume = 1f;
        audioSource.clip = whistleFinal;
        audioSource.loop = false;
        audioSource.Play();

        // cerrar completamente la visión
        float t2 = 0f;
        float durationFinal = 4f;
        while (t2 < durationFinal)
        {
            t2 += Time.deltaTime;
            float tFade = t2 / durationFinal;

            vignette.intensity.value = Mathf.Lerp(0.6f, 1f, tFade);
            vignette.smoothness.value = Mathf.Lerp(0.9f, 1f, tFade);

            blackFade.alpha = Mathf.Lerp(0f, 1f, tFade); // negro total

            yield return null;
        }

        // acá podés hacer game over o reiniciar
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1.5f);
        screenGameOver.SetActive(true);
    }

    IEnumerator ReduceAudio()
    {
        float t = 0f;
        float duration = 1.5f;
        float volumeInitial = audioSource.volume;

        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(volumeInitial, 0f, t / duration);
            yield return null;
        }
    }

    IEnumerator ReproduceMemories()
    {
        yield return new WaitForSecondsRealtime(3f);

        for (int i = 0; i < memories.Length; i++)
        {
            AudioClip clip = memories[i];

            float progress = i / (float)memories.Length;
            float volume = Mathf.Lerp(0.05f, 0.7f, progress);

            audioSource.PlayOneShot(clip, volume);

            yield return new WaitForSecondsRealtime(Random.Range(0.1f, 1.5f));
        }
    }

    IEnumerator ProggresiveBeats(float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            float t = Mathf.Pow(time / duration, 2.5f);

            // cada vez más rápido
            float interval = Mathf.Lerp(1.1f, 0.4f, t);
            interval += Random.Range(-0.02f, 0.02f);

            // más fuerte hacia el final
            float volume = Mathf.Lerp(0.15f, 0.6f, t); ;

            // un poco más agudo/deswaitdo
            float pitch = Mathf.Lerp(0.9f, 1.05f, t);

            audioSource.pitch = pitch;
            audioSource.PlayOneShot(beat, volume);

            yield return new WaitForSecondsRealtime(interval);

            time += interval;
        }
        yield return new WaitForSecondsRealtime(0.2f);
        audioSource.pitch = 1f; // reset
    }

    IEnumerator ReproduceMemorySmooth()
    {
        float t = 0f;
        float duration = 0.5f;

        audioSource.volume = 0f;
        audioSource.PlayOneShot(initialMemory);

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0f, 0.6f, t / duration);
            yield return null;
        }
    }

    IEnumerator ReduceBeatsSmooth()
    {
        float t = 0f;
        float duration = 1f;
        float pitchInicial = audioSource.pitch;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            audioSource.pitch = Mathf.Lerp(pitchInicial, 0.8f, t / duration);
            yield return null;
        }
    }
}