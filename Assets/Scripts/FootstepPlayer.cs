using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private AudioSource footstepSource;

    private void Update()
    {
        Vector3 horizontalVelocity =
            new Vector3(
                controller.velocity.x,
                0f,
                controller.velocity.z
            );

        bool isMoving =
            horizontalVelocity.magnitude > 0.1f;

        if (isMoving)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.Play();
            }
        }
        else
        {
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }
    }
}