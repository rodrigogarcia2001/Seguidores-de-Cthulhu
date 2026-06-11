using UnityEngine;

public class LightSystemManager : MonoBehaviour
{
    public void RepairAllLights()
    {
        BrokenSpotlight[] spotlights = FindObjectsOfType<BrokenSpotlight>();

        foreach (BrokenSpotlight spotlight in spotlights)
        {
            spotlight.RepairSpotlight();
        }

        TriggerSpotlight[] triggers = FindObjectsOfType<TriggerSpotlight>();

        foreach (TriggerSpotlight trigger in triggers)
        {
            trigger.gameObject.SetActive(false);
        }

    }
}