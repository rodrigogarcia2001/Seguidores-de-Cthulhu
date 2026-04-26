using UnityEngine;
using System.Collections;

public class MatchPerformance : MonoBehaviour
{
    public SistemaCordura sistemaCordura;

    public void Use()
    {
        if (sistemaCordura == null)
        {
            Debug.LogError("SistemaCordura no asignado");
            return;
        }

        sistemaCordura.StartCoroutine(Effect());
    }

    IEnumerator Effect()
    {
        sistemaCordura.EntrarEnLuz();
        Debug.Log("USADO FOSFORO");

        yield return new WaitForSeconds(2f);

        sistemaCordura.SalirDeLuz();
        Destroy(gameObject);
    }
}
