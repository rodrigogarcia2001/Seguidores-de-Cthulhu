using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EndingCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private AudioSource footstepAudio;
    [SerializeField] private AudioSource breathingAudio;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private MonoBehaviour playerDie;
    [SerializeField] private MonoBehaviour sanityE;
    [SerializeField] private GameObject bar;
    [SerializeField] private CharacterController characterController;

    [Header("Post Processing")]
    [SerializeField] private Volume volume;
    [SerializeField] private CanvasGroup blackFade;

    [Header("UI")]
    [SerializeField] private GameObject finImage;

    [Header("Scene")]
    [SerializeField] private string mainMenuScene = "MainMenu";

    [SerializeField] private float duration = 3f;
    [SerializeField] private float startDelay = 2f;

    [Header("Camera")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2f, 0);
    [SerializeField] private Vector3 cameraRotation = new Vector3(-80f, 0, 0);

    [Header("Player")]
    [SerializeField] private float playerDrop = 0.5f;

    private Vignette vignette;

    private void Start()
    {
        if (volume != null)
        {
            volume.profile.TryGet(out vignette);
        }

        if (blackFade != null)
        {
            blackFade.alpha = 0f;
        }

        if (finImage != null)
        {
            finImage.SetActive(false);
        }
    }

    public void StartEnding()
    {

        if (breathingAudio != null)
        breathingAudio.gameObject.SetActive(false);
        StartCoroutine(EndingSequence());
    }

    private IEnumerator EndingSequence()
    {
        yield return new WaitForSeconds(startDelay);

        if (footstepAudio != null)
        footstepAudio.gameObject.SetActive(false);

        if (playerDie != null)
            playerDie.enabled = false;

        if (sanityE != null)
            sanityE.enabled = false;

        if (bar != null)
            bar.SetActive(false);

        if (characterController != null)
            characterController.enabled = false;
        
        if (playerAnimator != null)
            playerAnimator.speed = 0f;

        Vector3 camStartPos = transform.position;
        Quaternion camStartRot = transform.rotation;

        Vector3 playerStartPos = player.position;

        Vector3 camTargetPos = camStartPos + cameraOffset;
        Quaternion camTargetRot = Quaternion.Euler(cameraRotation);

        Vector3 playerTargetPos = playerStartPos + Vector3.down * playerDrop;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            transform.position = Vector3.Lerp(camStartPos, camTargetPos, t);

            transform.rotation = Quaternion.Slerp(camStartRot, camTargetRot, t);

            player.position = Vector3.Lerp(playerStartPos, playerTargetPos, t);

            yield return null;
        }

        // Espera mirando el cielo
        yield return new WaitForSeconds(3f);

        float closeTime = 0f;
        float closeDuration = 3f;

        float vignetteStart = vignette != null ? vignette.intensity.value : 0f;

        float smoothStart = vignette != null ? vignette.smoothness.value : 0f;

        while (closeTime < closeDuration)
        {
            closeTime += Time.deltaTime;

            float p = closeTime / closeDuration;

            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(vignetteStart, 1f, p);

                vignette.smoothness.value = Mathf.Lerp(smoothStart, 1f, p);
            }

            if (blackFade != null)
            {
                blackFade.alpha = Mathf.Lerp(0f, 1f, p);
            }

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        if (finImage != null)
        {
            finImage.SetActive(true);
        }

        yield return new WaitUntil(() => Input.anyKeyDown);

        SceneManager.LoadScene(mainMenuScene);
    }
}