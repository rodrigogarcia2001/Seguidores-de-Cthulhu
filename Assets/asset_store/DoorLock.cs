using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public GameObject door;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if(KeyPuzzle.hasCorrectKey)
            {
                Destroy(door);
            }
        }
    }
}