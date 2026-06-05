using UnityEngine;
using System.Collections.Generic;

public class EnemyRouteTrigger : MonoBehaviour
{
    [SerializeField] private EnemigoPatrullaIA enemy;

    [SerializeField] private List<Transform> routePoints;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            enemy.StartRoute(routePoints);
        }
    }
}