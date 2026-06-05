using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemigoPatrullaIA : MonoBehaviour
{
   [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float arrivalDistance = 0.2f;

    [Header("Ambient Sounds")]
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private List<AudioClip> ambientSounds;
    [SerializeField] private float soundInterval = 2f;

    [Header("Footsteps")]
    [SerializeField] private AudioSource footstepsAudioSource;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float minFootstepInterval = 0.45f;
    [SerializeField] private float maxFootstepInterval = 0.7f;

    private Transform targetPoint;

    private float footstepTimer;
    private float nextFootstepTime;

    private void Start()
    {
        StartCoroutine(PlayAmbientSounds());

        nextFootstepTime = Random.Range(
            minFootstepInterval,
            maxFootstepInterval
        );
    }

    private void Update()
    {
        if (targetPoint == null)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        Vector3 direction =
            targetPoint.position - transform.position;

        if (direction != Vector3.zero)
        {
            transform.forward = direction.normalized;
        }

        footstepTimer += Time.deltaTime;

        if (footstepTimer >= nextFootstepTime)
        {
            PlayFootstep();

            footstepTimer = 0f;

            nextFootstepTime = Random.Range(
                minFootstepInterval,
                maxFootstepInterval
            );
        }

        if (Vector3.Distance(
            transform.position,
            targetPoint.position) <= arrivalDistance)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayAmbientSounds()
    {
        while (true)
        {
            if (ambientAudioSource != null &&
                ambientSounds.Count > 0)
            {
                AudioClip clip =
                    ambientSounds[
                        Random.Range(0, ambientSounds.Count)
                    ];

                ambientAudioSource.PlayOneShot(clip);
            }

            yield return new WaitForSeconds(
                soundInterval
            );
        }
    }

    private void PlayFootstep()
    {
        if (footstepsAudioSource == null ||
            footstepSounds.Length == 0)
            return;

        AudioClip clip =
            footstepSounds[
                Random.Range(0, footstepSounds.Length)
            ];

        footstepsAudioSource.pitch =
            Random.Range(0.94f, 1.04f);

        float volume =
            Random.Range(0.85f, 1f);

        footstepsAudioSource.PlayOneShot(
            clip,
            volume
        );
    }

    public void StartRoute(
        Transform startPoint,
        Transform endPoint)
    {
        gameObject.SetActive(true);

        transform.position =
            startPoint.position;

        targetPoint = endPoint;

        footstepTimer = 0f;
    }
}
