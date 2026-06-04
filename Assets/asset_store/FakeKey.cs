using UnityEngine;

public class FakeKey : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(gameObject);
        }
    }
}