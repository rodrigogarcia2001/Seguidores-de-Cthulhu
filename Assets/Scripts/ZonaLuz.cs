using UnityEngine;
using UnityEngine.InputSystem;

public class ZonaLuz : MonoBehaviour
{
    [SerializeField] private Light luz;

    private SistemaCordura jugador;
    private bool jugadorDentro = false;
    private bool estabaEnLuz = false;

    private void Update()
    {
        if (jugadorDentro && jugador != null)
        {
            bool ahoraEnLuz = luz.enabled;

            // Solo actuar si cambió el estado
            if (ahoraEnLuz != estabaEnLuz)
            {
                if (ahoraEnLuz)
                    jugador.EntrarEnLuz();
                else
                    jugador.SalirDeLuz();

                estabaEnLuz = ahoraEnLuz;
            }
        }

        //if (Keyboard.current.fKey.wasPressedThisFrame)
        //{
        //    luz.enabled = !luz.enabled; // toggle
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            jugador = other.GetComponent<SistemaCordura>();

            estabaEnLuz = luz.enabled;

            if (luz.enabled)
                jugador?.EntrarEnLuz();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (luz.enabled)
                jugador?.SalirDeLuz();

            jugadorDentro = false;
            jugador = null;
        }
    }

}