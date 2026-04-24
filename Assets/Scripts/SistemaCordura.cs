using System.Collections;
using UnityEngine;

public class SistemaCordura : MonoBehaviour
{
    [Header("Cordura")]
    [SerializeField] private float corduraMax = 500f;
    [SerializeField] private float corduraActual;

    public float CorduraMax => corduraMax;
    public float CorduraActual => corduraActual;

    [Header("Oscuridad")]
    [SerializeField] private float tiempoAntesDePerder = 3f;
    [SerializeField] private float perdidaPorSegundo = 5f;
    [SerializeField] private float recuperacionPorSegundo = 10f;

    [Header("Muerte")]
    [SerializeField] private GameObject barraUI;
    [SerializeField] private MuerteJugador muerteJugador;

    private float tiempoEnOscuridad = 0f;
    private bool estaMuerto = false;

    // fuentes de luz
    private int fuentesDeLuz = 0;

    void Start()
    {
        corduraActual = corduraMax;
    }

    void Update()
    {
        // si está muerto, no hacer nada más
        if (estaMuerto) return;

        bool enLuz = fuentesDeLuz > 0;

        if (!enLuz)
        {
            tiempoEnOscuridad += Time.deltaTime;

            if (tiempoEnOscuridad >= tiempoAntesDePerder)
            {
                corduraActual -= perdidaPorSegundo * Time.deltaTime;
            }
        }
        else
        {
            tiempoEnOscuridad = 0f;
            corduraActual += recuperacionPorSegundo * Time.deltaTime;
        }

        corduraActual = Mathf.Clamp(corduraActual, 0, corduraMax);

        // detectar muerte
        if (corduraActual <= 0f)
        {
            corduraActual = 0f;
            estaMuerto = true;
            StartCoroutine(RutinaMuerte());
        }
    }

    // estas funciones las llaman las luces
    public void EntrarEnLuz()
    {
        if (estaMuerto) return;
        fuentesDeLuz++;
    }

    public void SalirDeLuz()
    {
        if (estaMuerto) return;
        fuentesDeLuz = Mathf.Max(0, fuentesDeLuz - 1);
    }

    // rutina de muerte
    private IEnumerator RutinaMuerte()
    {
        yield return new WaitForSeconds(1f);

        if (barraUI != null)
            barraUI.SetActive(false);

        if (muerteJugador != null)
            muerteJugador.Morir();
    }
}