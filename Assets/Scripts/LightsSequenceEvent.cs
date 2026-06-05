using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsSequenceEvent : MonoBehaviour
{
    [SerializeField] private List<BrokenSpotlight> spotlights;
    [SerializeField] private float delayBetweenLights = 1.5f;

    public void StartSequence()
    {
        StartCoroutine(BreakSequence());
    }

    private IEnumerator BreakSequence()
    {
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