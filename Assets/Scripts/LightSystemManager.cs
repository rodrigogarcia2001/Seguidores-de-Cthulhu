using UnityEngine;

public class LightSystemManager : MonoBehaviour
{
    public void RepairAllLights()
    {
        BrokenSpotlight[] spotlights = FindObjectsByType<BrokenSpotlight>(FindObjectsSortMode.None);

        foreach (BrokenSpotlight spotlight in spotlights)
        {
            spotlight.RepairSpotlight();
        }

        TriggerSpotlight[] triggers = FindObjectsByType<TriggerSpotlight>(FindObjectsSortMode.None);

        foreach (TriggerSpotlight trigger in triggers)
        {
            trigger.gameObject.SetActive(false);
        }

    }
}