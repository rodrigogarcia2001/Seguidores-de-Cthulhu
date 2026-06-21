using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public GameObject door;
    private static bool doorDestroyed = false;

    void Start()
    {
        if (doorDestroyed && door != null)
        {
            Destroy(door);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if (KeyPuzzle.hasCorrectKey && !doorDestroyed)
            {
                Destroy(door);
                doorDestroyed = true;
            }
        }
    }
}