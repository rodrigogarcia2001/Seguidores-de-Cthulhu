using System.Collections;
using UnityEngine;

public class EndingCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private MonoBehaviour playerDie;
    [SerializeField] private MonoBehaviour SanityE;
    [SerializeField] private GameObject bar;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float duration = 3f;
    [SerializeField] private float startDelay = 2f;
    [Header("Camera")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2f, 0);
    [SerializeField] private Vector3 cameraRotation = new Vector3(-80f, 0, 0);

    [Header("Player")]
    [SerializeField] private float playerDrop = 0.5f;

    public void StartEnding()
    {
        StartCoroutine(EndingSequence());
    }

    IEnumerator EndingSequence()
    {
        yield return new WaitForSeconds(startDelay);
        if (playerDie != null)
    {
    playerDie.enabled = false;
    }
    if (SanityE != null)
    {
    SanityE.enabled = false;
    }
    if (bar != null)
    {
    bar.SetActive(false);
    }


    if (characterController != null)
    {
        characterController.enabled = false;
    }
    

        Vector3 camStartPos = transform.position;
        Quaternion camStartRot = transform.rotation;

        Vector3 playerStartPos = player.position;

        Vector3 camTargetPos = camStartPos + cameraOffset;
        Quaternion camTargetRot = Quaternion.Euler(cameraRotation);

        Vector3 playerTargetPos =
            playerStartPos + Vector3.down * playerDrop;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            transform.position =
                Vector3.Lerp(camStartPos, camTargetPos, t);

            transform.rotation =
                Quaternion.Slerp(camStartRot, camTargetRot, t);

            player.position =
                Vector3.Lerp(playerStartPos, playerTargetPos, t);

            yield return null;
        }
    }
}