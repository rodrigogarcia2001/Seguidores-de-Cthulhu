using System.Collections;
using UnityEngine;
using System;
public class SanitySystem : MonoBehaviour
{
    [Header("Sanity")]
    [SerializeField] private float sanityMax = 500f;
    [SerializeField] private float sanityCurrent;
    public event Action<float> OnSanityChanged;
    public float SanityMax => sanityMax;
    public float SanityCurrent => sanityCurrent;
    private float lastSanity;
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
        lastSanity = sanityCurrent;
        NotifySanityChanged();
    }

    void Update()
    {
        // si esta muerto, no hacer nada mas
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
            sanityCurrent += recoverPerSecond * Time.deltaTime;
        }

        sanityCurrent = Mathf.Clamp(sanityCurrent, 0, sanityMax);
        if (sanityCurrent != lastSanity)
        {
            lastSanity = sanityCurrent;
            NotifySanityChanged();
        }

        // detectar muerte
        if (sanityCurrent <= 0f)
        {
            sanityCurrent = 0f;
            isDie = true;
            StartCoroutine(DieRoutine());
        }
    }

    private void NotifySanityChanged()
    {
        OnSanityChanged?.Invoke(sanityCurrent);
    }

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