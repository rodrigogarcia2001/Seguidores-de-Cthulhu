using UnityEngine;

public class ActivateLightTrigger : MonoBehaviour
{
    [SerializeField] private GameObject directionalLight;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            if (directionalLight != null)
            {
                directionalLight.SetActive(true);
            }
        }
    }
}