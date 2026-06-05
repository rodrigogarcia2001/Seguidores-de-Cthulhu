using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
    public EndingCamera endingCamera;
    public AudioSource audioToStop;
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            endingCamera.StartEnding();
        }
        if (audioToStop != null)
            {
                audioToStop.Stop();
            }

    }
}