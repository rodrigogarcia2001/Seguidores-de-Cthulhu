using UnityEngine;
using System.Collections;

public class MatchPerformance : MonoBehaviour, IUsable
{
    public float duracion = 6f;

    private SistemaCordura sistemaCordura;
    private bool usado = false;

    public void Use(GameObject player)
    {
        if (usado) return;

        sistemaCordura = player.GetComponent<SistemaCordura>();

        if (sistemaCordura == null)
        {
            return;
        }

        usado = true;

        StartCoroutine(Fosforo());
    }

    private IEnumerator Fosforo()
    {
        Debug.Log("CORRUTINA INICIADA");

        sistemaCordura.EntrarEnLuz();

        yield return new WaitForSeconds(duracion);

        sistemaCordura.SalirDeLuz();

        Debug.Log("CORRUTINA TERMINADA");
        Destroy(gameObject);
    }
}