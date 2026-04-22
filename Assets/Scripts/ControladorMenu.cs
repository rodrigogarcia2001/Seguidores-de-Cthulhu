using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    // Arrastramos los objetos desde la jerarquía a estas variables
    [Header("Paneles y Botones")]
    public GameObject panelTutorial;
    public GameObject botonIniciar;
    public GameObject botonSalir;

    public void EmpezarJuego()
    {
        // Asegúrate de que la escena de tu juego se llame exactamente así
        // o cámbialo por el nombre que le hayas puesto.
        SceneManager.LoadScene("EscenaJuego");
    }

    public void AbrirTutorial()
    {
        panelTutorial.SetActive(true);  // Muestra el tutorial
        botonIniciar.SetActive(false);  // Esconde el botón iniciar
        botonSalir.SetActive(false);    // Esconde el botón salir
    }

    public void CerrarTutorial()
    {
        panelTutorial.SetActive(false); // Esconde el tutorial
        botonIniciar.SetActive(true);   // Muestra el botón iniciar
        botonSalir.SetActive(true);     // Muestra el botón salir
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
