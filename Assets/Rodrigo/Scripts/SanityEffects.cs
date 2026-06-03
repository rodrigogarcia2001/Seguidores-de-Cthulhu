using System.Collections;
using UnityEngine;
public class SanityEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SanitySystem system;

    [SerializeField] private AudioSource breathingSound;
    [SerializeField] private AudioSource insanitySound;

    [Header("Breathing")]
    [SerializeField] private AudioClip normalBreathing;
    [SerializeField] private AudioClip upsetBreathing;

    private AudioClip currentBreathing;

    [Header("Insanity Sounds (<=50%)")]
    [SerializeField] private AudioClip[] insanitySounds;
    [SerializeField] private float intervalMin = 0.5f;   // antes 5
    [SerializeField] private float intervalMax = 2f;     // antes 10

    private bool in25 = false;
    [SerializeField] private float volumeMax = 1f;
    private bool deathIncoming = false;
    private float deathTimer = 0f;
    private float volumeAtDeath;
    [SerializeField] private float delayBeforeFade = 0.2f; // antes 1
    [SerializeField] private float speedFade = 4f;    // antes 1.5

    private float insanityIntensity = 0f;
    [SerializeField] private float speedEntrance = 3f; // antes 0.5
    void Start()
    {
        // respiración
        breathingSound.clip = normalBreathing;
        breathingSound.loop = true;
        breathingSound.Play();

        StartCoroutine(RoutineAleatorySounds());
    }

    void Update()
    {
        float percent = system.SanityCurrent / system.SanityMax;
        if (!deathIncoming)
        {
            float targetPitch = Mathf.Lerp(0.6f, 1f, percent);
            breathingSound.pitch = Mathf.Lerp(breathingSound.pitch, targetPitch, Time.deltaTime * 2f);
        }
        float volume;

        if (percent <= 0.5f)
        {
            insanityIntensity = Mathf.Lerp(insanityIntensity, 1f, Time.deltaTime * speedEntrance);
        }
        else
        {
            insanityIntensity = Mathf.Lerp(insanityIntensity, 0f, Time.deltaTime * speedEntrance);
        }

        if (!deathIncoming)
        {
            if (percent > 0.2f)
            {
                volume = 1f;
            }
            else
            {
                float t = percent / 0.2f;

                // nunca baja de un mínimo antes de morir
                volume = Mathf.Lerp(0.2f, 1f, t);
            }

            // detectar llegada a 0
            if (percent <= 0f)
            {
                deathIncoming = true;
                deathTimer = 0f;
                volumeAtDeath = volume; // guardar volume actual
            }
        }
        else
        {
            deathTimer += Time.deltaTime;

            if (deathTimer < delayBeforeFade)
            {
                volume = volumeAtDeath; // mantiene el volume real
            }
            else
            {
                float t = (deathTimer - delayBeforeFade) * speedFade;
                volume = Mathf.Lerp(volumeAtDeath, 0f, Mathf.Clamp01(t));
            }
        }

        float volumeInsanity = insanityIntensity;

        // aplicar muerte también a locura
        if (deathIncoming)
        {
            if (deathTimer < delayBeforeFade)
            {
                volumeInsanity *= 1f;
            }
            else
            {
                float t = (deathTimer - delayBeforeFade) * speedFade;

                // curva suave (más natural para audio)
                float fade = Mathf.Pow(1f - Mathf.Clamp01(t), 2f);

                volumeInsanity *= fade;
            }
        }

        insanitySound.volume = volumeInsanity;

        breathingSound.volume = Mathf.Clamp01(volumeMax * volume);
        // 75% respiración agitada
        if (!deathIncoming)
        {
            if (percent <= 0.65f && currentBreathing != upsetBreathing)
            {
                StartCoroutine(ChangeBreathingSmooth(upsetBreathing));
            }
            else if (percent > 0.65f && currentBreathing != normalBreathing)
            {
                StartCoroutine(ChangeBreathingSmooth(normalBreathing));
            }
        }

        // 25% espacio para efecto fuerte
        bool ahoraEn25 = percent <= 0.25f;

        if (ahoraEn25 && !in25)
        {
            in25 = true;
            ActiveEffect25();
        }
        else if (!ahoraEn25 && in25)
        {
            in25 = false;
            DisableEffect25();
        }
    }

    //void CambiarRespiracion(AudioClip clip)
    //{
    //    if (deathIncoming) return;
    //    if (breathingSound.clip == clip) return;

    //    currentBreathing = clip;
    //    breathingSound.clip = clip;
    //    breathingSound.loop = true;
    //    breathingSound.Play();
    //}

    IEnumerator RoutineAleatorySounds()
    {
        while (true)
        {
            float time = 0f;
            float wait = Mathf.Lerp(intervalMax, intervalMin, insanityIntensity);

            while (time < wait)
            {
                if (insanityIntensity <= 0.05f)
                    break;

                time += Time.deltaTime;
                yield return null;
            }

            if (insanityIntensity > 0.05f && insanitySounds.Length > 0)
            {
                AudioClip clip = insanitySounds[Random.Range(0, insanitySounds.Length)];
                insanitySound.PlayOneShot(clip, 1f); // volume lo controlamos globalmente
            }

            yield return null;
        }
    }

    void ActiveEffect25()
    {
        Debug.Log("EFECTO 25%");

        // imagenes
    }

    IEnumerator ChangeBreathingSmooth(AudioClip newClip)
    {
        if (deathIncoming) yield break;
        if (breathingSound.clip == newClip) yield break;

        float durationFade = 0.5f;
        float t = 0f;
        float volumeInitial = breathingSound.volume;

        // fade out
        while (t < durationFade)
        {
            t += Time.deltaTime;
            breathingSound.volume = Mathf.Lerp(volumeInitial, 0f, t / durationFade);
            yield return null;
        }

        // cambio
        breathingSound.clip = newClip;
        currentBreathing = newClip;
        breathingSound.Play();

        // fade in
        t = 0f;
        while (t < durationFade)
        {
            t += Time.deltaTime;
            breathingSound.volume = Mathf.Lerp(0f, volumeInitial, t / durationFade);
            yield return null;
        }
    }

    void DisableEffect25()
    {
        Debug.Log("Se quita efecto fuerte");
        // apagar efectos visuales, etc
    }

}