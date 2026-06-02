using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    [Header("Panels (UI)")]
    public GameObject panelPause;
    public GameObject panelLose;
    public GameObject panelWin;

    [Header("Button Main Menu")]
    public GameObject buttonStart;
    public GameObject buttonExit;
    public GameObject bottomBlue;

    [Header("Sound of Teammate")]
    public AudioController audioController;
    public AudioClip musicForMenu;

    private bool isPause = false;

    void Awake()
    {
        if (instance == null) instance = this;
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

            if (audioController != null && musicForMenu != null)
            {
                audioController.PlaySound(musicForMenu, true);
            }
        }
        else
        {
            // --- CAMBIO AQUÍ: Bloqueamos el cursor al iniciar cualquier nivel ---
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (buttonStart) buttonStart.SetActive(false);
            if (buttonExit) buttonExit.SetActive(false);
            if (bottomBlue) bottomBlue.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            if ((panelLose != null && panelLose.activeSelf) || (panelWin != null && panelWin.activeSelf))
                return;

            if (isPause) Resume();
            else Pause();
        }

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            RestartLevel();
        }
    }

    public void StartGame()
    {
        Debug.Log("Iniciando nueva partida...");
        CheckpointManager.ResetChekpoints();

        if (audioController != null) audioController.FadeOut(1.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Pause()
    {
        if (panelPause) panelPause.SetActive(true);

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
                fuente.PlayOneShot(musicForMenu);
            }
        }

        Time.timeScale = 0f;
        isPause = true;

        // Liberamos el cursor al pausar
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        if (panelPause) panelPause.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;

        if (audioController != null) audioController.FadeOut(0.5f);

        // --- CAMBIO AQUÍ: Ocultamos el cursor al volver al juego ---
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ActiveScreenLose()
    {
        if (panelLose) panelLose.SetActive(true);
        Time.timeScale = 0f;
        EnableMouse();
    }

    public void ActiveScreenWin()
    {
        if (panelWin) panelWin.SetActive(true);
        Time.timeScale = 0f;
        EnableMouse();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RetryFromChekpoint()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartFullGame()
    {
        Time.timeScale = 1f;
        CheckpointManager.ResetChekpoints();
        SceneManager.LoadScene(1);
    }

    private void EnableMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}