using System.Collections;
using UnityEngine;
public class EfectosCordura : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private SistemaCordura sistema;

    [SerializeField] private AudioSource audioRespiracion;
    [SerializeField] private AudioSource audioLocura;

    [Header("Respiración")]
    [SerializeField] private AudioClip respiracionNormal;
    [SerializeField] private AudioClip respiracionAgitada;

    private AudioClip respiracionActual;

    [Header("Sonidos locura (<=50%)")]
    [SerializeField] private AudioClip[] sonidosLocura;
    [SerializeField] private float intervaloMin = 0.5f;   // antes 5
    [SerializeField] private float intervaloMax = 2f;     // antes 10

    private bool en25 = false;
    [SerializeField] private float volumenMax = 1f;
    private bool empezandoMuerte = false;
    private float timerMuerte = 0f;
    private float volumenAlMorir;
    [SerializeField] private float delayAntesFade = 0.2f; // antes 1
    [SerializeField] private float velocidadFade = 4f;    // antes 1.5

    private float intensidadLocura = 0f;
    [SerializeField] private float velocidadEntrada = 3f; // antes 0.5
    void Start()
    {
        // respiración
        audioRespiracion.clip = respiracionNormal;
        audioRespiracion.loop = true;
        audioRespiracion.Play();

        StartCoroutine(RutinaSonidosAleatorios());
    }

    void Update()
    {
        float porcentaje = sistema.CorduraActual / sistema.CorduraMax;
        if (!empezandoMuerte)
        {
            float targetPitch = Mathf.Lerp(0.6f, 1f, porcentaje);
            audioRespiracion.pitch = Mathf.Lerp(audioRespiracion.pitch, targetPitch, Time.deltaTime * 2f);
        }
        float volumen;

        if (porcentaje <= 0.5f)
        {
            intensidadLocura = Mathf.Lerp(intensidadLocura, 1f, Time.deltaTime * velocidadEntrada);
        }
        else
        {
            intensidadLocura = Mathf.Lerp(intensidadLocura, 0f, Time.deltaTime * velocidadEntrada);
        }

        if (!empezandoMuerte)
        {
            if (porcentaje > 0.2f)
            {
                volumen = 1f;
            }
            else
            {
                float t = porcentaje / 0.2f;

                // nunca baja de un mínimo antes de morir
                volumen = Mathf.Lerp(0.2f, 1f, t);
            }

            // detectar llegada a 0
            if (porcentaje <= 0f)
            {
                empezandoMuerte = true;
                timerMuerte = 0f;
                volumenAlMorir = volumen; // guardar volumen actual
            }
        }
        else
        {
            timerMuerte += Time.deltaTime;

            if (timerMuerte < delayAntesFade)
            {
                volumen = volumenAlMorir; // mantiene el volumen real
            }
            else
            {
                float t = (timerMuerte - delayAntesFade) * velocidadFade;
                volumen = Mathf.Lerp(volumenAlMorir, 0f, Mathf.Clamp01(t));
            }
        }

        float volumenLocura = intensidadLocura;

        // aplicar muerte también a locura
        if (empezandoMuerte)
        {
            if (timerMuerte < delayAntesFade)
            {
                volumenLocura *= 1f;
            }
            else
            {
                float t = (timerMuerte - delayAntesFade) * velocidadFade;

                // curva suave (más natural para audio)
                float fade = Mathf.Pow(1f - Mathf.Clamp01(t), 2f);

                volumenLocura *= fade;
            }
        }

        audioLocura.volume = volumenLocura;

        audioRespiracion.volume = Mathf.Clamp01(volumenMax * volumen);
        // 75% respiración agitada
        if (!empezandoMuerte)
        {
            if (porcentaje <= 0.65f && respiracionActual != respiracionAgitada)
            {
                StartCoroutine(CambiarRespiracionSuave(respiracionAgitada));
            }
            else if (porcentaje > 0.65f && respiracionActual != respiracionNormal)
            {
                StartCoroutine(CambiarRespiracionSuave(respiracionNormal));
            }
        }

        // 25% espacio para efecto fuerte
        bool ahoraEn25 = porcentaje <= 0.25f;

        if (ahoraEn25 && !en25)
        {
            en25 = true;
            ActivarEfecto25();
        }
        else if (!ahoraEn25 && en25)
        {
            en25 = false;
            DesactivarEfecto25();
        }
    }

    void CambiarRespiracion(AudioClip clip)
    {
        if (empezandoMuerte) return;
        if (audioRespiracion.clip == clip) return;

        respiracionActual = clip;
        audioRespiracion.clip = clip;
        audioRespiracion.loop = true;
        audioRespiracion.Play();
    }

    IEnumerator RutinaSonidosAleatorios()
    {
        while (true)
        {
            float tiempo = 0f;
            float espera = Mathf.Lerp(intervaloMax, intervaloMin, intensidadLocura);

            while (tiempo < espera)
            {
                if (intensidadLocura <= 0.05f)
                    break;

                tiempo += Time.deltaTime;
                yield return null;
            }

            if (intensidadLocura > 0.05f && sonidosLocura.Length > 0)
            {
                AudioClip clip = sonidosLocura[Random.Range(0, sonidosLocura.Length)];
                audioLocura.PlayOneShot(clip, 1f); // volumen lo controlamos globalmente
            }

            yield return null;
        }
    }

    void ActivarEfecto25()
    {
        Debug.Log("EFECTO 25%");

        // imagenes
    }

    IEnumerator CambiarRespiracionSuave(AudioClip nuevoClip)
    {
        if (empezandoMuerte) yield break;
        if (audioRespiracion.clip == nuevoClip) yield break;

        float duracionFade = 0.5f;
        float t = 0f;
        float volumenInicial = audioRespiracion.volume;

        // fade out
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            audioRespiracion.volume = Mathf.Lerp(volumenInicial, 0f, t / duracionFade);
            yield return null;
        }

        // cambio
        audioRespiracion.clip = nuevoClip;
        respiracionActual = nuevoClip;
        audioRespiracion.Play();

        // fade in
        t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            audioRespiracion.volume = Mathf.Lerp(0f, volumenInicial, t / duracionFade);
            yield return null;
        }
    }

    void DesactivarEfecto25()
    {
        Debug.Log("Se quita efecto fuerte");
        // apagar efectos visuales, etc
    }

}