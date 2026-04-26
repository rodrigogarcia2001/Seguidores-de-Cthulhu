using UnityEngine;
using System.Collections;

public class Medbox : MonoBehaviour, IUsable
{
    [SerializeField] private float aumento = 5f;
    [SerializeField] private float duracion = 3f;

    private Light[] lucesOriginales;
    private float[] rangosOriginales;

    public void Use(GameObject player)
    {
        StartCoroutine(ExpandirLuces());
        StartCoroutine(DestruirLuego());
    }

    private IEnumerator ExpandirLuces()
    {
        lucesOriginales = FindObjectsOfType<Light>();
        rangosOriginales = new float[lucesOriginales.Length];

        // guardar y aumentar
        for (int i = 0; i < lucesOriginales.Length; i++)
        {
            rangosOriginales[i] = lucesOriginales[i].range;
            lucesOriginales[i].range += aumento;
        }

        yield return new WaitForSeconds(duracion);

        // restaurar
        for (int i = 0; i < lucesOriginales.Length; i++)
        {
            if (lucesOriginales[i] != null)
                lucesOriginales[i].range = rangosOriginales[i];
        }
    }

    private IEnumerator DestruirLuego()
    {
        yield return new WaitForSeconds(duracion);
        Destroy(gameObject);
    }
}