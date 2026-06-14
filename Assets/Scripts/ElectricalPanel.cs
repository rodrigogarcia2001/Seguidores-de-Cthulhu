using UnityEngine;
using StarterAssets;

public class ElectricalPanel : MonoBehaviour
{
    [Header("References")]
    public GameObject miniGameUI;

    public FirstPersonController playerController;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip panelOpenSound;

    private bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            OpenMiniGame();
        }
    }

    void OpenMiniGame()
    {
        audioSource.PlayOneShot(panelOpenSound);
        miniGameUI.SetActive(true);

        playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMiniGame()
    {
        miniGameUI.SetActive(false);

        playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    public void DisablePanel()
    {
        playerNearby = false;
        GetComponent<BoxCollider>().enabled = false;
    }
}