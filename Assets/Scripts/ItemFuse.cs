using UnityEngine;

public class ItemFuse : MonoBehaviour
{
    private bool isPlayerNearby;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = false;
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            FuseCounter.Instance.AddFuse();
            GetComponent<PickupSound>()?.Play();
            Destroy(gameObject);
        }
    }
}