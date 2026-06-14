using UnityEngine;
using UnityEngine.InputSystem;

public class LightZones : MonoBehaviour
{
    [SerializeField] private Light zoneLight;
    private SanitySystem player;
    private bool playerInside = false;
    private bool wasInLight = false;

    private void Update()
    {
        if (playerInside && player != null)
        {
            bool nowInLight = zoneLight.enabled;

            if (nowInLight != wasInLight)
            {
                if (nowInLight)
                    player.ComeInLight();
                else
                    player.OutOfLight();

                wasInLight = nowInLight;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.GetComponent<SanitySystem>();

            wasInLight = zoneLight.enabled;

            if (zoneLight.enabled)
                player?.ComeInLight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (zoneLight.enabled)
                player?.OutOfLight();

            playerInside = false;
            player = null;
        }
    }
}