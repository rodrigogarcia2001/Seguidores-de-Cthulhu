using UnityEngine;
using UnityEngine.InputSystem;
public class CheckpointManager : MonoBehaviour
{
    // La variable estática sobrevive al recargar la escena
    public static Vector3 posicionGuardada = Vector3.zero;
    public static bool hayCheckpoint = false;

    private void Start()
    {
        if (hayCheckpoint && posicionGuardada != Vector3.zero)
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");

            if (jugador != null)
            {
                CharacterController cc = jugador.GetComponent<CharacterController>();

                if (cc != null)
                {
                    cc.enabled = false; // Desactivamos la física del controlador

                    jugador.transform.position = posicionGuardada;
                    cc.enabled = true; // Reactivamos la física
                    Debug.Log("Teletransportando jugador al checkpoint: " + posicionGuardada);
                }
                else
                {
                    jugador.transform.position = posicionGuardada;
                }
            }
            else
            {
                Debug.LogWarning("CheckpointManager: No se encontró ningún objeto con el Tag 'Player'.");
            }
        }
    }

    public static void GuardarPunto(Vector3 nuevaPosicion)
    {
        posicionGuardada = nuevaPosicion;
        hayCheckpoint = true;
        Debug.Log("¡Checkpoint guardado!");
    }

    public static void ResetearCheckpoints()
    {
        posicionGuardada = Vector3.zero;
        hayCheckpoint = false;
        Debug.Log("Sistema de Checkpoints reiniciado para una nueva partida.");
    }
}