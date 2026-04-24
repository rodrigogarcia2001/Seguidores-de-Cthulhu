using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    // Esto permite que otros scripts lo encuentren fácilmente (Singleton)
    public static ControladorMenu instancia;

    [Header("Paneles (UI)")]
    public GameObject panelPausa;
    public GameObject panelPerder;
    public GameObject panelGanar;

    [Header("Botones Menú Principal")]
    public GameObject botonIniciar;
    public GameObject botonSalir;

    private bool estaPausado = false;

    void Awake()
    {
        // Configuramos la instancia al iniciar
        if (instancia == null) instancia = this;
    }

    void Update()
    {
        // Solo permitimos usar la P si NO están activos los paneles de Ganar o Perder
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Verificamos que los paneles existan antes de preguntar si están activos
            if ((panelPerder != null && panelPerder.activeSelf) || (panelGanar != null && panelGanar.activeSelf))
                return;

            if (estaPausado) Reanudar();
            else Pausar();
        }
    }

    // --- NAVEGACIÓN BÁSICA ---
    public void EmpezarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("EscenaJuego"); // Cambiar al nombre real de la escena
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
        if (panelPausa) panelPausa.SetActive(true);
        if (botonIniciar) botonIniciar.SetActive(false);
        if (botonSalir) botonSalir.SetActive(false);

        Time.timeScale = 0f;
        estaPausado = true;
        HabilitarRaton(); // Útil para poder hacer clic en la pausa
    }

    public void Reanudar()
    {
        if (panelPausa) panelPausa.SetActive(false);
        if (botonIniciar) botonIniciar.SetActive(true);
        if (botonSalir) botonSalir.SetActive(true);

        Time.timeScale = 1f;
        estaPausado = false;
    }

    // --- ESTADOS FINALES (GANAR/PERDER) ---
    public void ActivarPantallaPerder()
    {
        if (panelPerder) panelPerder.SetActive(true);
        Time.timeScale = 0f;
        HabilitarRaton();
    }

    public void ActivarPantallaGanar()
    {
        if (panelGanar) panelGanar.SetActive(true);
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