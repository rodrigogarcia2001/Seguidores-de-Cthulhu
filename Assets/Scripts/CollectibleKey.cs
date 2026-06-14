using UnityEngine;

public class CollectibleKey : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("LLAVE RECOGIDA");

            KeyCounter.Instance.AddKey();

            Destroy(gameObject);
        }
    }
}