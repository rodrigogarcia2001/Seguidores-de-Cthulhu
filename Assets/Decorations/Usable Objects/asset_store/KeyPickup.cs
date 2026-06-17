using UnityEngine;

public class KeyPuzzle : MonoBehaviour
{
    public static bool hasCorrectKey = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            hasCorrectKey = true;
            Destroy(gameObject);
        }
    }
}