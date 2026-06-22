using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolIA : MonoBehaviour
{
    private enum AIState { Patrol, Chase, Search, Scare }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float arrivalDistance = 0.2f;
    [Tooltip("How long it stays still at each point before moving to the next one")]
    [SerializeField] private float waitTimeAtPoint = 2f;

    [Header("Perception")]
    [SerializeField] private Transform player;
    [SerializeField] private float visionRange = 8f;
    [SerializeField] private float visionAngle = 60f;
    [SerializeField] private Transform eyes; // point from which the enemy "looks"
    [SerializeField] private LayerMask obstacleMask;

    [Header("Scare")]
    [Tooltip("Distance at which the enemy scares the player instead of continuing to chase")]
    [SerializeField] private float scareRange = 1.5f;
    [Tooltip("How long it stays visible/looking at the player before disappearing")]
    [SerializeField] private float scareDuration = 1f;
    [Tooltip("How long it stays invisible before reappearing and chasing again")]
    [SerializeField] private float reappearTime = 2f;

    [Header("Search")]
    [SerializeField] private float searchTime = 4f;

    [Header("Ambient Sounds")]
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private List<AudioClip> ambientSounds;
    [SerializeField] private float soundInterval = 2f;

    [Header("Footsteps")]
    [SerializeField] private AudioSource footstepsAudioSource;

    private List<Transform> routePoints = new List<Transform>();
    private int currentPoint = 0;

    private AIState currentState = AIState.Patrol;
    private Vector3 lastKnownPosition;
    private float searchTimer;
    private bool isDisappearing; // prevents the scare sequence from being relaunched while already in progress
    private bool isWaitingAtPoint;
    private float waitTimer;
    private Renderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {
        StartCoroutine(PlayAmbientSounds());
    }

    private void Update()
    {
        UpdatePerception();

        if (!isDisappearing)
        {
            switch (currentState)
            {
                case AIState.Patrol:
                    Patrol();
                    break;
                case AIState.Chase:
                    Chase();
                    break;
                case AIState.Search:
                    Search();
                    break;
                case AIState.Scare:
                    Scare();
                    break;
            }
        }

        UpdateFootsteps();
    }

    // ---------------- PERCEPTION ----------------

    private void UpdatePerception()
    {
        if (player == null || isDisappearing) return;

        bool seesPlayer = CanSeePlayer();

        if (seesPlayer)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= scareRange)
            {
                currentState = AIState.Scare;
            }
            else
            {
                currentState = AIState.Chase;
            }

            lastKnownPosition = player.position;
        }
        else if (currentState == AIState.Chase || currentState == AIState.Scare)
        {
            // lost sight of the player, switch to searching the last known position
            currentState = AIState.Search;
            searchTimer = searchTime;
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 origin = eyes != null ? eyes.position : transform.position;
        Vector3 directionToPlayer = player.position - origin;

        float distance = directionToPlayer.magnitude;
        if (distance > visionRange) return false;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > visionAngle * 0.5f) return false;

        // if there's a wall/obstacle between the enemy and the player, it can't see it
        if (Physics.Raycast(origin, directionToPlayer.normalized, out RaycastHit hit, distance, obstacleMask))
        {
            return false;
        }

        return true;
    }

    // ---------------- STATES ----------------

    private void Patrol()
    {
        if (routePoints.Count == 0) return;

        if (isWaitingAtPoint)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                isWaitingAtPoint = false;
                currentPoint++;

                if (currentPoint >= routePoints.Count)
                {
                    if (footstepsAudioSource != null) footstepsAudioSource.Stop();
                    gameObject.SetActive(false);
                }
            }

            return;
        }

        Transform target = routePoints[currentPoint];
        MoveTowardsTarget(target.position, moveSpeed);

        if (Vector3.Distance(transform.position, target.position) <= arrivalDistance)
        {
            isWaitingAtPoint = true;
            waitTimer = waitTimeAtPoint;
        }
    }

    private void Chase()
    {
        MoveTowardsTarget(player.position, chaseSpeed);
    }

    private void Search()
    {
        MoveTowardsTarget(lastKnownPosition, moveSpeed);

        searchTimer -= Time.deltaTime;

        bool arrived = Vector3.Distance(transform.position, lastKnownPosition) <= arrivalDistance;

        if (arrived || searchTimer <= 0f)
        {
            currentState = AIState.Patrol;
        }
    }

    private void Scare()
    {
        LookTowards(player.position);

        if (!isDisappearing)
        {
            isDisappearing = true;
            StartCoroutine(DisappearAndReappear());
        }
    }

    private IEnumerator DisappearAndReappear()
    {
        // stays still, showing itself/looking at the player (the "scare")
        yield return new WaitForSeconds(scareDuration);

        SetVisibility(false);
        if (footstepsAudioSource != null) footstepsAudioSource.Stop();

        // time invisible before coming back
        yield return new WaitForSeconds(reappearTime);

        SetVisibility(true);
        isDisappearing = false;
        currentState = AIState.Chase;
    }

    private void SetVisibility(bool visible)
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }
    }

    // ---------------- UTILITIES ----------------

    private void MoveTowardsTarget(Vector3 destination, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        LookTowards(destination);
    }

    private void LookTowards(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        direction.y = 0f; // prevents the character from tilting up/down

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.forward = direction.normalized;
        }
    }

    private void UpdateFootsteps()
    {
        if (footstepsAudioSource == null) return;

        bool isMoving = currentState != AIState.Scare && !isWaitingAtPoint;

        if (isMoving && !footstepsAudioSource.isPlaying)
        {
            footstepsAudioSource.Play();
        }
        else if (!isMoving && footstepsAudioSource.isPlaying)
        {
            footstepsAudioSource.Stop();
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
        if (points == null || points.Count < 2) return;

        gameObject.SetActive(true);

        routePoints.Clear();
        routePoints.AddRange(points);

        transform.position = routePoints[0].position;
        currentPoint = 1;

        currentState = AIState.Patrol;
        isDisappearing = false;
        isWaitingAtPoint = false;
        SetVisibility(true);

        if (footstepsAudioSource != null) footstepsAudioSource.Stop();
    }

    private void OnDrawGizmosSelected()
    {
        // visual aid for the vision range in the editor (does not affect gameplay)
        Gizmos.color = Color.yellow;
        Vector3 origin = eyes != null ? eyes.position : transform.position;
        Gizmos.DrawWireSphere(origin, visionRange);
    }
}