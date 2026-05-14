using UnityEngine;
using UnityEngine.InputSystem;

public class LightZones : MonoBehaviour
{
    [SerializeField] private Light light;

    private SanitySystem player;
    private bool playerInside = false;
    private bool wasInLight = false;

    private void Update()
    {
        if (playerInside && player != null)
        {
            bool nowInLight = light.enabled;

            // Solo actuar si cambió el estado
            if (nowInLight != wasInLight)
            {
                if (nowInLight)
                    player.ComeInLight();
                else
                    player.OutOfLight();

                wasInLight = nowInLight;
            }
        }

        //if (Keyboard.current.fKey.wasPressedThisFrame)
        //{
        //    light.enabled = !light.enabled; // toggle
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.GetComponent<SanitySystem>();

            wasInLight = light.enabled;

            if (light.enabled)
                player?.ComeInLight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (light.enabled)
                player?.OutOfLight();

            playerInside = false;
            player = null;
        }
    }

}