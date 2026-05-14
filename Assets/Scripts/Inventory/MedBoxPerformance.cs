using UnityEngine;
using System.Collections;

public class Medbox : MonoBehaviour, IUsable
{
    [SerializeField] private float increase = 5f;
    [SerializeField] private float duration = 3f;

    private Light[] originalLights;
    private float[] originalRanges;

    public void Use(GameObject player)
    {
        StartCoroutine(ExpandLights());
        StartCoroutine(DestroyLater());
    }

    private IEnumerator ExpandLights()
    {
        originalLights = FindObjectsOfType<Light>();
        originalRanges = new float[originalLights.Length];

        // guardar y aumentar
        for (int i = 0; i < originalLights.Length; i++)
        {
            originalRanges[i] = originalLights[i].range;
            originalLights[i].range += increase;
        }

        yield return new WaitForSeconds(duration);

        // restaurar
        for (int i = 0; i < originalLights.Length; i++)
        {
            if (originalLights[i] != null)
                originalLights[i].range = originalRanges[i];
        }
    }

    private IEnumerator DestroyLater()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}