using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class KeyDoor : MonoBehaviour
{
    [Header("References to Assign")]
    public GameObject targetDoor;
    public Renderer[] slotRenderers;
    public Material activeLightMaterial;
    public GameObject healingZone;

    [Header("Optional Event")]
    public bool startLightsSequence = false;
    public LightsSequenceEvent lightsSequenceEvent;

    [Header("Audio Settings")]
    public AudioClip insertSound;
    public AudioClip errorSound;
    public AudioClip completeSound;

    [Header("Puzzle Settings")]
    public int requiredKeys = 3;
    private int keysPlaced = 0;
    private bool isPuzzleCompleted = false;
    private bool isPlayerNearby = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (healingZone != null)
        {
            healingZone.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPuzzleCompleted)
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isPuzzleCompleted)
        {
            if (KeyCounter.Instance.CurrentKeys > 0)
            {
                PlaceKey();
            }
            else
            {
                if (errorSound != null)
                {
                    audioSource.PlayOneShot(errorSound);
                }
                Debug.Log("No tienes llaves para colocar.");
            }
        }
    }

    private void PlaceKey()
    {
        KeyCounter.Instance.RemoveKey();

        if (insertSound != null)
        {
            audioSource.PlayOneShot(insertSound);
        }

        if (keysPlaced < slotRenderers.Length && slotRenderers[keysPlaced] != null)
        {
            slotRenderers[keysPlaced].material = activeLightMaterial;
        }

        keysPlaced++;

        if (keysPlaced >= requiredKeys)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        isPuzzleCompleted = true;

        if (completeSound != null)
        {
            audioSource.PlayOneShot(completeSound);
        }

        if (healingZone != null)
        {
            healingZone.SetActive(true);
            Debug.Log("¡Zona de curación encendida!");
        }

        if (targetDoor != null)
        {
            Animator doorAnimator = targetDoor.GetComponent<Animator>();
            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("Open");
                Debug.Log("¡Puzzle completado! Puerta abriéndose.");
            }
        }

        if (startLightsSequence && lightsSequenceEvent != null)
        {
            lightsSequenceEvent.StartSequence();
        }
    }
}
