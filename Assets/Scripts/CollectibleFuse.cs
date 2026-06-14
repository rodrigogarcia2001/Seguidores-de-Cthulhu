using UnityEngine;

public class CollectibleFuse : MonoBehaviour
{
    private bool playerInside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            FuseCounter.Instance.AddFuse();
            Destroy(gameObject);
        }
    }
}