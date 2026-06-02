using UnityEngine;

public class FuseBox : MonoBehaviour
{
    [Header("References to Assign")]
    public GameObject targetDoor;
    public Renderer[] slotRenderers;       // Arrastrar aquí Ranura_1, Ranura_2 y Ranura_3
    public Material activeLightMaterial;   // Tu material verde emisivo

    [Header("Puzzle Settings")]
    public int requiredFuses = 3;

    private int fusesInHand = 0; // Fusibles que el jugador lleva encima
    private int fusesPlaced = 0; // Fusibles que ya se colocaron en el panel

    private bool isPuzzleCompleted = false;
    private bool isPlayerNearby = false;

    // Esta función la llama el fusible del piso cuando lo agarramos
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
        // Si el jugador está cerca, presiona E, el puzzle NO está completo y TIENE al menos 1 fusible
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isPuzzleCompleted)
        {
            if (fusesInHand > 0)
            {
                PlaceFuse();
            }
            else
            {
                Debug.Log("No tienes fusibles en la mano para colocar.");
            }
        }
    }

    private void PlaceFuse()
    {
        fusesInHand--; // Descontamos uno del inventario del jugador

        // Encendemos solo la ranura correspondiente (0, 1 o 2)
        if (fusesPlaced < slotRenderers.Length && slotRenderers[fusesPlaced] != null)
        {
            slotRenderers[fusesPlaced].material = activeLightMaterial;
        }

        fusesPlaced++; // Lo sumamos a la caja
        Debug.Log("Fusible colocado. Total en caja: " + fusesPlaced + " / " + requiredFuses);

        // Si ya colocamos los necesarios, completamos el puzzle
        if (fusesPlaced >= requiredFuses)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        isPuzzleCompleted = true;

        if (targetDoor != null)
        {
            Animator doorAnimator = targetDoor.GetComponent<Animator>();
            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("Open");
                Debug.Log("¡Puzzle completado! Puerta abriéndose.");
            }
        }
    }
}
