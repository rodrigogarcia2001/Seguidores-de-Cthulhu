using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsSequenceEvent : MonoBehaviour
{
    [SerializeField] private List<BrokenSpotlight> spotlights;
    [SerializeField] private float delayBetweenLights = 1.5f;
    [Header("Trigger Activation")]
    [SerializeField] private GameObject triggerToActivate;
    public void StartSequence()
    {
        StartCoroutine(BreakSequence());
    }

    private IEnumerator BreakSequence()
    {
        if (triggerToActivate != null)
        {
            triggerToActivate.SetActive(true);
        }

        foreach (BrokenSpotlight spotlight in spotlights)
        {
            if (spotlight != null)
            {
                spotlight.DestroySpotlight();
            }

            yield return new WaitForSeconds(delayBetweenLights);
        }
    }
}