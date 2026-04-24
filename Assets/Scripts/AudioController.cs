using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    public AudioSource source;

    void Awake()
    {
        if (source == null)
            source = GetComponent<AudioSource>();
    }

    // Reproducir sonido
    public void PlaySound(AudioClip clip, bool loop = false)
    {
        source.clip = clip;
        source.loop = loop;
        source.volume = 1f;
        source.Play();
    }

    // Parar sonido
    public void StopSound()
    {
        source.Stop();
    }

    // Bajar volumen progresivamente
    public void FadeOut(float tiempo)
    {
        StartCoroutine(FadeOutRutina(tiempo));
    }

    IEnumerator FadeOutRutina(float tiempo)
    {
        float volumenInicial = source.volume;
        float t = 0f;

        while (t < tiempo)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(volumenInicial, 0f, t / tiempo);
            yield return null;
        }

        source.Stop();
    }
}