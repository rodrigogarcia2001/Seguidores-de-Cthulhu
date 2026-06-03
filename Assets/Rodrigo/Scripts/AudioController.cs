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
    public void FadeOut(float time)
    {
        StartCoroutine(FadeOutRoutine(time));
    }

    IEnumerator FadeOutRoutine(float time)
    {
        float volumeInitial = source.volume;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(volumeInitial, 0f, t / time);
            yield return null;
        }

        source.Stop();
    }
}