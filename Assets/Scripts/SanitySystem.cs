using System.Collections;
using UnityEngine;

public class SanitySystem : MonoBehaviour
{
    [Header("Sanity")]
    [SerializeField] private float sanityMax = 500f;
    [SerializeField] private float sanityCurrent;

    public float SanityMax => sanityMax;
    public float SanityCurrent => sanityCurrent;

    [Header("Obscure")]
    [SerializeField] private float timeBeforeLose = 3f;
    [SerializeField] private float losePerSecond = 5f;
    [SerializeField] private float recoverPerSecond = 10f;

    [Header("Die")]
    [SerializeField] private GameObject barUI;
    [SerializeField] private PlayerDie playerDie;

    private float timeIntoObscure = 0f;
    private bool isDie = false;

    // fuentes de luz
    private int sourceOfLight = 0;

    void Start()
    {
        sanityCurrent = sanityMax;
    }

    void Update()
    {
        // si está muerto, no hacer nada más
        if (isDie) return;

        bool enLuz = sourceOfLight > 0;

        if (!enLuz)
        {
            timeIntoObscure += Time.deltaTime;

            if (timeIntoObscure >= timeBeforeLose)
            {
                sanityCurrent -= losePerSecond * Time.deltaTime;
            }
        }
        else
        {
            timeIntoObscure = 0f;
            sanityCurrent += recoverPerSecond* Time.deltaTime;
        }

        sanityCurrent = Mathf.Clamp(sanityCurrent, 0, sanityMax);

        // detectar muerte
        if (sanityCurrent <= 0f)
        {
            sanityCurrent = 0f;
            isDie = true;
            StartCoroutine(DieRoutine());
        }
    }

    // estas funciones las llaman las lights
    public void ComeInLight()
    {
        if (isDie) return;
        sourceOfLight++;
    }

    public void OutOfLight()
    {
        if (isDie) return;
        sourceOfLight = Mathf.Max(0, sourceOfLight - 1);
    }

    // rutina de muerte
    private IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(1f);

        if (barUI != null)
            barUI.SetActive(false);

        if (playerDie != null)
            playerDie.Morir();
    }
}