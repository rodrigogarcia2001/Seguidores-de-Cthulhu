using UnityEngine;

public class FuseBox : MonoBehaviour
{
    [Header("References to Assign")]
    public GameObject targetDoor;          // Drag the door/fence that blocks the path here
    public Renderer[] slotRenderers;      // Your 3 slot objects (Ranuras)
    public Material activeLightMaterial;   // Your emissive green/red material

    [Header("Puzzle Settings")]
    public int requiredFuses = 3;
    private int collectedFusesCount = 0;
    private bool isPuzzleCompleted = false;
    private bool isPlayerNearby = false;

    // This function is called by the ItemFuse script automatically
    public void RegisterCollectedFuse()
    {
        collectedFusesCount++;
        Debug.Log("Fuses collected: " + collectedFusesCount + " / " + requiredFuses);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPuzzleCompleted)
        {
            isPlayerNearby = true;
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
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isPuzzleCompleted)
        {
            if (collectedFusesCount >= requiredFuses)
            {
                CompletePuzzle();
            }
            else
            {
                Debug.Log("You need " + (requiredFuses - collectedFusesCount) + " more fuses.");
            }
        }
    }

    private void CompletePuzzle()
    {
        isPuzzleCompleted = true;

        // 1. Turn on slot visual indicators
        foreach (Renderer slot in slotRenderers)
        {
            if (slot != null && activeLightMaterial != null)
            {
                slot.material = activeLightMaterial;
            }
        }

        // 2. Open the door by disabling it
        if (targetDoor != null)
        {
            targetDoor.SetActive(false);
            Debug.Log("Puzzle Complete! Door opened.");
        }
        else
        {
            Debug.LogError("Target Door is missing in the FuseBox Inspector!");
        }
    }
}
