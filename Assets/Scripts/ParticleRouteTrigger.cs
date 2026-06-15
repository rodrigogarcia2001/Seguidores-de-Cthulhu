using UnityEngine;
using System.Collections.Generic;

public class ParticleRouteTrigger : MonoBehaviour
{
    [SerializeField] private ParticlePathGuide guide;
    [SerializeField] private List<Transform> routePoints;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            if (guide != null)
            {
                guide.StartRoute(routePoints);
            }
        }
    }
}