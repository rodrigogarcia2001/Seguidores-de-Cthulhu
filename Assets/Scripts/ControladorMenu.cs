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
    public GameObject fondoAzul; // variable para apagar el fondo

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
        // 1. RECUPERAR VOLUMEN no funciona
        AudioListener.volume = 1f;

        // 2. BUSCAR AUDIO: Si el hueco está vacío, lo busca solo en la escena
        if (audioController == null)
        {
            audioController = Object.FindFirstObjectByType<AudioController>();
        }

        // 3. REPRODUCIR MÚSICA
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (audioController != null && musicaParaMenu != null)
            {
                audioController.PlaySound(musicaParaMenu, true);
            }
        }

        // 4. LIMPIEZA DE INTERFAZ
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (botonIniciar) botonIniciar.SetActive(false);
            if (botonSalir) botonSalir.SetActive(false);
            if (fondoAzul) fondoAzul.SetActive(false);
        }
    }

    void Update()
    {
        //forma de detectar la tecla P sin errores
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
    // En ControladorMenu.cs
    public void EmpezarJuego()
    {
        Debug.Log("Iniciando nueva partida...");
        
        CheckpointManager.ResetearCheckpoints(); // Reiniciamos el sistema de checkpoints para la nueva partida

        if (audioController != null) audioController.FadeOut(1.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1); // Carga el nivel 1 desde cero
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
            // 1. Prender el objeto a la fuerza, no funciona
            audioController.gameObject.SetActive(true);

            AudioSource fuente = audioController.GetComponent<AudioSource>();
            if (fuente != null)
            {
                // 2. ACTIVAR EL COMPONENTE 
                fuente.enabled = true;

                // 3. Ignorar la pausa del tiempo
                fuente.ignoreListenerPause = true;
                fuente.volume = 1f;

                // 4. Reproducir
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
        Time.timeScale = 1f;
        estaPausado = false;

        // Detenemos la música del menú al volver a jugar
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
        CheckpointManager.ResetearCheckpoints();
        SceneManager.LoadScene(1);
    }

    private void HabilitarRaton()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}