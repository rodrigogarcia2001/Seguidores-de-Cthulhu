using UnityEngine;

public class PickupSound : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;

    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    public void Play()
    {
        if (pickupSound == null)
            return;

        AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);
    }
}