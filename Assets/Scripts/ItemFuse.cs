using UnityEngine;

public class ItemFuse : MonoBehaviour
{
    [Header("Settings")]
    public string fuseName = "Standard Fuse";

    // Reference to the main FuseBox in the room
    [SerializeField] private FuseBox targetFuseBox;
    private bool isPlayerNearby = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Near " + fuseName + ". Press 'E' to pick up.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (targetFuseBox != null)
            {
                targetFuseBox.RegisterCollectedFuse(); // Alerts the box
                Destroy(gameObject); // Disappears from the floor
            }
            else
            {
                Debug.LogError("Missing FuseBox reference on " + gameObject.name);
            }
        }
    }
}