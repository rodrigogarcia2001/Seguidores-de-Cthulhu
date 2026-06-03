using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ControladorMenu : MonoBehaviour
{
    public static ControladorMenu instancia;

    [Header("Paneles (UI)")]
    public GameObject panelPausa;
    public GameObject panelPerder;
    public GameObject panelGanar;

    [Header("Botones Menú Principal")]
    public GameObject botonIniciar;
    public GameObject botonSalir;
    public GameObject fondoAzul;

    [Header("Sonido del Compañero")]
    public AudioController audioController;
    public AudioClip musicaParaMenu;

    private bool estaPausado = false;

    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    void Start()
    {
        AudioListener.volume = 1f;

        if (audioController == null)
        {
            audioController = Object.FindFirstObjectByType<AudioController>();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // En el menú principal, aseguramos que el cursor se vea
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (audioController != null && musicaParaMenu != null)
            {
                audioController.PlaySound(musicaParaMenu, true);
            }
        }
        else
        {
            // --- CAMBIO AQUÍ: Bloqueamos el cursor al iniciar cualquier nivel ---
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (botonIniciar) botonIniciar.SetActive(false);
            if (botonSalir) botonSalir.SetActive(false);
            if (fondoAzul) fondoAzul.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            if ((panelPerder != null && panelPerder.activeSelf) || (panelGanar != null && panelGanar.activeSelf))
                return;

            if (estaPausado) Reanudar();
            else Pausar();
        }

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            ReiniciarNivel();
        }
    }

    // --- NUEVAS FUNCIONES PARA CONTROLES ---

    public void AbrirControles()
    {
        if (panelControles != null)
        {
            panelControles.SetActive(true);
        }

        // CASO 1: Si lo abres desde el Menú de Pausa en medio del juego
        if (panelPausa != null && panelPausa.activeSelf)
        {
            panelPausa.SetActive(false);
        }
        // CASO 2: Si lo abres desde el Menú Principal
        else
        {
            if (fondoAzul != null) fondoAzul.SetActive(false);
            if (botonIniciar != null) botonIniciar.SetActive(false);
            if (botonSalir != null) botonSalir.SetActive(false);
            if (botonControles != null) botonControles.SetActive(false);
        }
    }

    public void CerrarControles()
    {
        if (panelControles != null)
        {
            panelControles.SetActive(false);
        }

        // Al cerrar, verificamos en qué escena estamos para saber qué reabrir
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // Si estamos jugando, volvemos a mostrar el panel de pausa
            if (panelPausa != null) panelPausa.SetActive(true);
        }
        else
        {
            // Si estamos en el menú principal, volvemos a mostrar los botones de inicio
            if (fondoAzul != null) fondoAzul.SetActive(true);
            if (botonIniciar != null) botonIniciar.SetActive(true);
            if (botonSalir != null) botonSalir.SetActive(true);
            if (botonControles != null) botonControles.SetActive(true);
        }
    }

    // ----------------------------------------

    public void EmpezarJuego()
    {
        Debug.Log("Iniciando nueva partida...");
        CheckpointManager.ResetearCheckpoints();

        if (audioController != null) audioController.FadeOut(1.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }

    public void Pausar()
    {
        if (panelPausa) panelPausa.SetActive(true);

        AudioListener.volume = 1f;

        if (audioController != null)
        {
            audioController.gameObject.SetActive(true);
            AudioSource fuente = audioController.GetComponent<AudioSource>();
            if (fuente != null)
            {
                fuente.enabled = true;
                fuente.ignoreListenerPause = true;
                fuente.volume = 1f;
                fuente.PlayOneShot(musicaParaMenu);
            }
        }

        Time.timeScale = 0f;
        estaPausado = true;

        // Liberamos el cursor al pausar
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Reanudar()
    {
        if (panelPausa) panelPausa.SetActive(false);
        Time.timeScale = 1f;
        estaPausado = false;

        if (audioController != null) audioController.FadeOut(0.5f);

        // --- CAMBIO AQUÍ: Ocultamos el cursor al volver al juego ---
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

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

    public void ReintentarDesdeCheckpoint()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReiniciarJuegoCompleto()
    {
        Time.timeScale = 1f;
        CheckpointManager.ResetearCheckpoints();
        SceneManager.LoadScene(1);
    }

    private void HabilitarRaton()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}