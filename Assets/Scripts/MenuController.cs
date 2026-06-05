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
    public GameObject panelControles; // <-- NUEVO: Referencia al panel de controles
    private GameObject panelAnterior;

    [Header("Botones Menú Principal")]
    public GameObject botonIniciar;
    public GameObject botonSalir;
    public GameObject botonControles; // <-- NUEVO: (Opcional) Referencia al botón si necesitas ocultarlo
    public GameObject fondoAzul;

    [Header("Sonido del Compañero")]
    public AudioController audioController;
    public AudioClip musicaParaMenu;

    private bool estaPausado = false;

    //public CheckpointManager chekpointManager;

    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    void Start()
    {
        AudioListener.volume = 1f;

        // Asegurarnos de que el panel de controles empiece apagado
        if (panelControles != null) panelControles.SetActive(false);

        if (audioController == null)
        {
            audioController = Object.FindFirstObjectByType<AudioController>();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // Menú principal
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (audioController != null && musicaParaMenu != null)
            {
                audioController.PlaySound(musicaParaMenu, true);
            }
        }
        else
        {
            // Niveles de juego
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (botonIniciar) botonIniciar.SetActive(false);
            if (botonSalir) botonSalir.SetActive(false);
            if (botonControles) botonControles.SetActive(false); // Ocultar también este botón
            if (fondoAzul) fondoAzul.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            if ((panelPerder != null && panelPerder.activeSelf) || (panelGanar != null && panelGanar.activeSelf))
                return;

            // Si el panel de controles está abierto y apretamos pausa, lo cerramos
            if (panelControles != null && panelControles.activeSelf)
            {
                CerrarControles();
                return;
            }

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
            // Ocultamos el pausa al abrir controles
            if (panelPausa != null) panelPausa.SetActive(false); 
        }
    }

    public void CerrarControles()
    {
        if (panelControles != null)
        {
            panelControles.SetActive(false);
            // Volvemos a mostrar el pausa al cerrar controles
            if (panelPausa != null) panelPausa.SetActive(true);
        }
    }

    // ----------------------------------------

    public void EmpezarJuego()
    {
        Debug.Log("Iniciando nueva partida...");
        CheckPointManager.ResetCheckpoints();

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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Reanudar()
    {
        if (panelPausa) panelPausa.SetActive(false);
        // También cerramos el panel de controles por si estaba abierto en pausa
        if (panelControles) panelControles.SetActive(false);

        Time.timeScale = 1f;
        estaPausado = false;

        if (audioController != null) audioController.FadeOut(0.5f);

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
        CheckPointManager.ResetCheckpoints();
        SceneManager.LoadScene(1);
    }

    private void HabilitarRaton()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}