using UnityEngine;
public class ActivateLightTrigger : MonoBehaviour
{
    [SerializeField] private GameObject directionalLight;
    [SerializeField] private GameObject backgroundAmbient;
    [SerializeField] private AudioSource exteriorAudio;
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

            if (backgroundAmbient != null)
            {
                backgroundAmbient.SetActive(false);
            }

            if (exteriorAudio != null)
            {
                exteriorAudio.Play();
            }
        }
    }
}