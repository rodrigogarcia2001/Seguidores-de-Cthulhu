using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FuseBox : MonoBehaviour
{
    [Header("References to Assign")]
    public GameObject targetDoor;
    public Renderer[] slotRenderers;
    public Material activeLightMaterial;
    public GameObject healingZone;        // <-- NUEVO: Arrastra aquí el objeto que cura al jugador

    [Header("Optional Event")]
public bool startLightsSequence = false;
public LightsSequenceEvent lightsSequenceEvent;

    [Header("Audio Settings")]
    public AudioClip insertSound;
    public AudioClip errorSound;
    public AudioClip completeSound;

    [Header("Puzzle Settings")]
    public int requiredFuses = 3;

    private int fusesInHand = 0;
    private int fusesPlaced = 0;

    private bool isPuzzleCompleted = false;
    private bool isPlayerNearby = false;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // OPCIONAL: Nos aseguramos de que la zona empiece apagada por código por si acaso
        if (healingZone != null)
        {
            healingZone.SetActive(false);
        }
    }

    public void PickUpFuse()
    {
        fusesInHand++;
        Debug.Log("Recogiste un fusible. Tienes " + fusesInHand + " en la mano.");
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
            if (fusesInHand > 0)
            {
                PlaceFuse();
            }
            else
            {
                if (errorSound != null)
                {
                    audioSource.PlayOneShot(errorSound);
                }
                Debug.Log("No tienes fusibles en la mano para colocar.");
            }
        }
    }

    private void PlaceFuse()
    {
        fusesInHand--;

        if (insertSound != null)
        {
            audioSource.PlayOneShot(insertSound);
        }

        if (fusesPlaced < slotRenderers.Length && slotRenderers[fusesPlaced] != null)
        {
            slotRenderers[fusesPlaced].material = activeLightMaterial;
        }

        fusesPlaced++;

        if (fusesPlaced >= requiredFuses)
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

        // ¡¡AQUÍ ACTIVAMOS LA CURACIÓN!!
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
