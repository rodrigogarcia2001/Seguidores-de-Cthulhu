using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckPointManager.SavePoint(spawnPoint.position);
        }
    }
}
