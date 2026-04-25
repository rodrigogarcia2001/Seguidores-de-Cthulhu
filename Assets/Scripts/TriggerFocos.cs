using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFocos : MonoBehaviour
{
    [Header("Focos a afectar")]
    [SerializeField] private List<FocoRoto> focos;

    [Header("Configuración")]
    [SerializeField] private int cantidadARomper = 2;
    [SerializeField] private float delayEntreFocos = 0.2f;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        if (other.CompareTag("Player"))
        {
            activado = true;
            StartCoroutine(RomperFocos());
        }
    }

    private IEnumerator RomperFocos()
    {
        // mezclamos la lista para que sea aleatorio
        List<FocoRoto> copia = new List<FocoRoto>(focos);

        for (int i = 0; i < copia.Count; i++)
        {
            FocoRoto temp = copia[i];
            int randomIndex = Random.Range(i, copia.Count);
            copia[i] = copia[randomIndex];
            copia[randomIndex] = temp;
        }

        int cantidad = Mathf.Min(cantidadARomper, copia.Count);

        for (int i = 0; i < cantidad; i++)
        {
            if (copia[i] != null)
            {
                copia[i].RomperFoco();
            }

            yield return new WaitForSeconds(delayEntreFocos);
        }
    }
}