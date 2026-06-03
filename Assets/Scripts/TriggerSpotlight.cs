using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpotlight : MonoBehaviour
{
    [Header("Spotlights to be affect")]
    [SerializeField] private List<BrokenSpotlight> spotlights;

    [Header("Configuration")]
    [SerializeField] private int amountToBroken = 2;
    [SerializeField] private float delayBetweenSpotlights = 0.2f;

    private bool Enable = false;

    private void OnTriggerEnter(Collider other)
    {
        if (Enable) return;

        if (other.CompareTag("Player"))
        {
            Enable = true;
            StartCoroutine(DestroySpotlights());
        }
    }

    private IEnumerator DestroySpotlights()
    {
        // mezclamos la lista para que sea aleatorio
        List<BrokenSpotlight> copy = new List<BrokenSpotlight>(spotlights);

        for (int i = 0; i < copy.Count; i++)
        {
            BrokenSpotlight temp = copy[i];
            int randomIndex = Random.Range(i, copy.Count);
            copy[i] = copy[randomIndex];
            copy[randomIndex] = temp;
        }

        int amount = Mathf.Min(amountToBroken, copy.Count);

        for (int i = 0; i < amount; i++)
        {
            if (copy[i] != null)
            {
                copy[i].DestroySpotlight();
            }

            yield return new WaitForSeconds(delayBetweenSpotlights);
        }
    }
}