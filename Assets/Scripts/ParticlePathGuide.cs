using System.Collections.Generic;
using UnityEngine;

public class ParticlePathGuide : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float arrivalDistance = 0.5f;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem guideParticles;

    private List<Transform> routePoints = new List<Transform>();
    private int currentPoint = 0;

    private void Update()
    {
        if (routePoints.Count == 0 || guideParticles == null)
            return;

        Transform target = routePoints[currentPoint];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // Dirección visual
        Vector3 direction = target.position - transform.position;
        if (direction != Vector3.zero)
        {
            guideParticles.transform.forward = direction.normalized;
        }

        // Avance de puntos
        if (Vector3.Distance(transform.position, target.position) <= arrivalDistance)
        {
            currentPoint++;

            if (currentPoint >= routePoints.Count)
            {
                guideParticles.Stop();
                gameObject.SetActive(false);
            }
        }
    }
    public void StartRoute(List<Transform> points)
    {
        if (points == null || points.Count < 2)
            return;

        routePoints.Clear();
        routePoints.AddRange(points);

        currentPoint = 1;

        transform.position = routePoints[0].position;

        guideParticles.Play();
        gameObject.SetActive(true);
    }
}