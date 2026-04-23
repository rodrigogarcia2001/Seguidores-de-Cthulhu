using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    [Header("Paneles (UI)")]
    public GameObject panelPausa;
    public GameObject panelPerder;
    public GameObject panelGanar;

    [Header("Botones Menú Principal")]
    public GameObject botonIniciar;
    public GameObject botonSalir;

    private bool estaPausado = false;

    void Update()
    {
        // Solo permitimos la pausa si los paneles de ganar/perder están apagados
        if (Input.GetKeyDown(KeyCode.P) && !panelPerder.activeSelf && !panelGanar.activeSelf)
        {
            if (estaPausado) Reanudar();
            else Pausar();
        }
    }

    // --- NAVEGACIÓN BÁSICA ---
    public void EmpezarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("EscenaJuego");
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // --- SISTEMA DE PAUSA ---
    public void Pausar()
    {
        panelPausa.SetActive(true);
        if (botonIniciar) botonIniciar.SetActive(false);
        if (botonSalir) botonSalir.SetActive(false);

        Time.timeScale = 0f;
        estaPausado = true;
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false);
        if (botonIniciar) botonIniciar.SetActive(true);
        if (botonSalir) botonSalir.SetActive(true);

        Time.timeScale = 1f;
        estaPausado = false;
    }

    // --- ESTADOS FINALES (GANAR/PERDER) ---
    public void ActivarPantallaPerder()
    {
        panelPerder.SetActive(true);
        Time.timeScale = 0f;
        HabilitarRaton();
    }

    public void ActivarPantallaGanar()
    {
        panelGanar.SetActive(true);
        Time.timeScale = 0f;
        HabilitarRaton();
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HabilitarRaton()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}