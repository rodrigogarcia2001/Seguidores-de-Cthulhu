using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
public class MuerteJugador : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pantallaGameOver;

    [Header("Referencias")]
    [SerializeField] private Camera camara;
    [SerializeField] private Volume volume;
    [SerializeField] private MonoBehaviour controlCamara;
    [SerializeField] private CanvasGroup fadeNegro;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip recuerdoInicial;
    [SerializeField] private AudioClip[] recuerdos;
    [SerializeField] private AudioClip pitidoFinal;
    [SerializeField] private AudioClip latido;

    private Bloom bloom;
    private Vignette vignette;
    Coroutine recuerdosCoroutine;
    private bool muerto = false;

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
        if (muerto) return;
        muerto = true;

        StartCoroutine(SecuenciaMuerte());
    }

    IEnumerator SecuenciaMuerte()
    {

        StartCoroutine(LatidosProgresivos(3.5f));

        yield return new WaitForSecondsRealtime(3.5f);
        yield return null; // 1 frame
        StartCoroutine(BajarLatidosSuave());
        //  PRIMER sonido (el importante)
        StartCoroutine(ReproducirRecuerdoSuave());
        yield return new WaitForSecondsRealtime(0.2f);
        recuerdosCoroutine = StartCoroutine(ReproducirRecuerdos());


        if (controlCamara != null)
            controlCamara.enabled = false;

        GetComponentInChildren<MonoBehaviour>().enabled = false;//Evita que el jugaodr se mueva

        float tiempo = 0f;
        float duracion = 3f;

        float bloomInicial = bloom.intensity.value;
        float thresholdInicial = bloom.threshold.value;
        float vignetteInicial = vignette.intensity.value;
        float smoothInicial = vignette.smoothness.value;

        Vector3 rotInicial = camara.transform.eulerAngles;
        Vector3 rotFinal = new Vector3(-80f, rotInicial.y, rotInicial.z); // mirar al cielo

        while (tiempo < duracion)
        {
            float velocidad = 0.5f;
            tiempo += Time.deltaTime * velocidad;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracion);

            // 1. Rotar cámara hacia arriba
            camara.transform.eulerAngles = Vector3.Lerp(rotInicial, rotFinal, t);

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

        // bajar volumen de recuerdos
        StartCoroutine(BajarAudio());

        yield return new WaitForSecondsRealtime(1.5f);
        if (recuerdosCoroutine != null)
        {
            StopCoroutine(recuerdosCoroutine);
        }
        // sonido final
        audioSource.Stop();
        audioSource.volume = 1f;
        audioSource.clip = pitidoFinal;
        audioSource.loop = false;
        audioSource.Play();

        // cerrar completamente la visión
        float t2 = 0f;
        float duracionFinal = 4f;
        while (t2 < duracionFinal)
        {
            t2 += Time.deltaTime;
            float tFade = t2 / duracionFinal;

            vignette.intensity.value = Mathf.Lerp(0.6f, 1f, tFade);
            vignette.smoothness.value = Mathf.Lerp(0.9f, 1f, tFade);

            fadeNegro.alpha = Mathf.Lerp(0f, 1f, tFade); // negro total

            yield return null;
        }

        // acá podés hacer game over o reiniciar
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1.5f);
        pantallaGameOver.SetActive(true);
    }

    IEnumerator BajarAudio()
    {
        float t = 0f;
        float duracion = 1.5f;
        float volumenInicial = audioSource.volume;

        while (t < duracion)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(volumenInicial, 0f, t / duracion);
            yield return null;
        }
    }

    IEnumerator ReproducirRecuerdos()
    {
        yield return new WaitForSecondsRealtime(3f);

        for (int i = 0; i < recuerdos.Length; i++)
        {
            AudioClip clip = recuerdos[i];

            float progreso = i / (float)recuerdos.Length;
            float volumen = Mathf.Lerp(0.05f, 0.7f, progreso);

            audioSource.PlayOneShot(clip, volumen);

            yield return new WaitForSecondsRealtime(Random.Range(0.1f, 1.5f));
        }
    }

    IEnumerator LatidosProgresivos(float duracion)
    {
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            float t = Mathf.Pow(tiempo / duracion, 2.5f);

            // cada vez más rápido
            float intervalo = Mathf.Lerp(1.1f, 0.4f, t);
            intervalo += Random.Range(-0.02f, 0.02f);

            // más fuerte hacia el final
            float volumen = Mathf.Lerp(0.15f, 0.6f, t); ;

            // un poco más agudo/desesperado
            float pitch = Mathf.Lerp(0.9f, 1.05f, t);

            audioSource.pitch = pitch;
            audioSource.PlayOneShot(latido, volumen);

            yield return new WaitForSecondsRealtime(intervalo);

            tiempo += intervalo;
        }
        yield return new WaitForSecondsRealtime(0.2f);
        audioSource.pitch = 1f; // reset
    }

    IEnumerator ReproducirRecuerdoSuave()
    {
        float t = 0f;
        float duracion = 0.5f;

        audioSource.volume = 0f;
        audioSource.PlayOneShot(recuerdoInicial);

        while (t < duracion)
        {
            t += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0f, 0.6f, t / duracion);
            yield return null;
        }
    }

    IEnumerator BajarLatidosSuave()
    {
        float t = 0f;
        float duracion = 1f;
        float pitchInicial = audioSource.pitch;

        while (t < duracion)
        {
            t += Time.unscaledDeltaTime;
            audioSource.pitch = Mathf.Lerp(pitchInicial, 0.8f, t / duracion);
            yield return null;
        }
    }
}