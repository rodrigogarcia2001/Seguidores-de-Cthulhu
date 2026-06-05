using UnityEngine;

public class ItemFuse : MonoBehaviour
{
    [Header("Settings")]
    public string fuseName = "Standard Fuse";
    [SerializeField] private FuseBox targetFuseBox;
    private bool isPlayerNearby = false;

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

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (targetFuseBox != null)
            {
                targetFuseBox.PickUpFuse(); // <-- Esta es la línea que cambió
                GetComponent<PickupSound>()?.Play();
                Destroy(gameObject);
            }
        }
    }
}