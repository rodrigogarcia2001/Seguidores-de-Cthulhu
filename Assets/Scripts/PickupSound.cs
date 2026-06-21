using UnityEngine;

public class PickupSound : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;

    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    public void Play()
{
    Debug.Log("Play llamado");

    if (pickupSound == null)
    {
        Debug.LogError("pickupSound es NULL");
        return;
    }

    AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, 1f);
}
}