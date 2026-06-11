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

    private List<Transform> routePoints = new List<Transform>();

    private int currentPoint = 0;
    private void Start()
    {
        StartCoroutine(PlayAmbientSounds());
    }

    private void Update()
    {
        if (routePoints.Count == 0)
            return;

        Transform target = routePoints[currentPoint];

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        Vector3 direction = target.position - transform.position;
        if (direction != Vector3.zero)
        {
            transform.forward = direction.normalized;
        }

        if (Vector3.Distance(transform.position, target.position) <= arrivalDistance)
        {
            currentPoint++;

            if (currentPoint >= routePoints.Count)
            {
                if (footstepsAudioSource != null)
                {
                    footstepsAudioSource.Stop();
                }
                gameObject.SetActive(false);
            }
        }
        if (!footstepsAudioSource.isPlaying)
        {
            footstepsAudioSource.Play();
        }
    }

    private IEnumerator PlayAmbientSounds()
    {
        while (true)
        {
            if (ambientAudioSource != null && ambientSounds.Count > 0)
            {
                AudioClip clip = ambientSounds[Random.Range(0, ambientSounds.Count)];
                ambientAudioSource.PlayOneShot(clip);
            }

            yield return new WaitForSeconds(soundInterval);
        }
    }

    public void StartRoute(List<Transform> points)
    {
        if (points == null || points.Count < 2)
            return;

        gameObject.SetActive(true);

        routePoints.Clear();
        routePoints.AddRange(points);

        transform.position = routePoints[0].position;

        currentPoint = 1;

        if (footstepsAudioSource != null)
        {
            footstepsAudioSource.Stop();
        }
    }
}
