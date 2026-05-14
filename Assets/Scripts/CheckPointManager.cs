using UnityEngine;
using UnityEngine.InputSystem;
public class CheckpointManager : MonoBehaviour
{
    // La variable estática sobrevive al recargar la escena
    public static Vector3 savePosition = Vector3.zero;
    public static bool haveChekpoint = false;

    private void Start()
    {
        if (haveChekpoint && savePosition != Vector3.zero)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                CharacterController cc = player.GetComponent<CharacterController>();

                if (cc != null)
                {
                    cc.enabled = false; // Desactivamos la física del controlador

                    player.transform.position = savePosition;
                    cc.enabled = true; // Reactivamos la física
                    Debug.Log("Teletransportando player al checkpoint: " + savePosition);
                }
                else
                {
                    player.transform.position = savePosition;
                }
            }
            else
            {
                Debug.LogWarning("CheckpointManager: No se encontró ningún objeto con el Tag 'Player'.");
            }
        }
    }

    public static void SavePoint(Vector3 newPosition)
    {
        savePosition = newPosition;
        haveChekpoint = true;
        Debug.Log("¡Checkpoint guardado!");
    }

    public static void ResetChekpoints()
    {
        savePosition = Vector3.zero;
        haveChekpoint = false;
        Debug.Log("Sistema de Checkpoints reiniciado para una nueva partida.");
    }
}