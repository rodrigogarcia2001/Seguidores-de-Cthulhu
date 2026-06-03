using UnityEngine;
using System.Collections;

public class MatchPerformance : MonoBehaviour, IUsable
{
    public float duration = 6f;
    public AudioClip useSound;

    private AudioSource audioSource;
    private SanitySystem sanitySystem;
    private bool used = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Use(GameObject player)
    {
        if (used) return;

        sanitySystem = player.GetComponent<SanitySystem>();
        if (sanitySystem == null) return;

        used = true;

        // SONIDO SIN SCRIPTABLE OBJECT
        audioSource.PlayOneShot(useSound);

        StartCoroutine(Match());
    }

    private IEnumerator Match()
    {
        sanitySystem.ComeInLight();

        yield return new WaitForSeconds(duration);

        sanitySystem.OutOfLight();

        Destroy(gameObject);
    }
}