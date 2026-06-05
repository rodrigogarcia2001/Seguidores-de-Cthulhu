using UnityEngine;

public class EnemyRouteTrigger : MonoBehaviour
{
    [SerializeField] private EnemigoPatrullaIA enemy;

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            enemy.StartRoute(startPoint, endPoint);
        }
    }
}