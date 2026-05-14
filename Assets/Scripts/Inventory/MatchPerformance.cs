using UnityEngine;
using System.Collections;

public class MatchPerformance : MonoBehaviour, IUsable
{
    public float duration = 6f;

    private SanitySystem sanitySystem;
    private bool used = false;

    public void Use(GameObject player)
    {
        if (used) return;

        sanitySystem = player.GetComponent<SanitySystem>();

        if (sanitySystem == null)
        {
            return;
        }

        used = true;

        StartCoroutine(Match());
    }

    private IEnumerator Match()
    {
        Debug.Log("CORRUTINA INICIADA");

        sanitySystem.ComeInLight();

        yield return new WaitForSeconds(duration);

        sanitySystem.OutOfLight();

        Debug.Log("CORRUTINA TERMINADA");
        Destroy(gameObject);
    }
}