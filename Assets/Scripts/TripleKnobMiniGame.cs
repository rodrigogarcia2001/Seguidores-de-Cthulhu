using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TripleKnobMiniGame : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onPuzzleCompleted;
    public UnityEvent onPuzzleFinished;
    
    [Header("Bars")]
    public RectTransform bar1;
    public RectTransform bar2;
    public RectTransform bar3;

    [Header("Indicators")]
    public RectTransform indicator1;
    public RectTransform indicator2;
    public RectTransform indicator3;

    [Header("Target Zones")]
    public RectTransform zone1;
    public RectTransform zone2;
    public RectTransform zone3;
    float stun1, stun2, stun3;
    const float stunDuration = 0.5f;
    float time1, time2, time3;
    bool locked1, locked2, locked3;

    private ElectricalPanel currentPanel;

    [Header("Controls")]
    public bool increase;
    public bool decrease;

    private float value1 = 0f;
    private float value2 = 0f;
    private float value3 = 0f;

    private bool completed = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource humSource;
    public AudioClip backgroundHum;
    public AudioClip successSound;

    void Start()
    {
        GenerateZonesBalanced();
    }

    void Update()
    {
        float halfBar = bar1.rect.width / 2f;

        // Movimiento
        float dir = 0f;

        if (increase) dir = 1f;
        else if (decrease) dir = -1f;

        // aplicar stun como tiempo (SEPARADO del movimiento)
        stun1 -= Time.deltaTime;
        stun2 -= Time.deltaTime;
        stun3 -= Time.deltaTime;

        // movimiento SOLO si no está en stun
        if (stun1 <= 0f)
            value1 += 800f * dir * Time.deltaTime;

        if (stun2 <= 0f)
            value2 += 200f * dir * Time.deltaTime;

        if (stun3 <= 0f)
            value3 += 400f * dir * Time.deltaTime;

        value1 = Mathf.Clamp(value1, -halfBar, halfBar);
        value2 = Mathf.Clamp(value2, -halfBar, halfBar);
        value3 = Mathf.Clamp(value3, -halfBar, halfBar);

        indicator1.anchoredPosition = new Vector2(value1, indicator1.anchoredPosition.y);
        indicator2.anchoredPosition = new Vector2(value2, indicator2.anchoredPosition.y);
        indicator3.anchoredPosition = new Vector2(value3, indicator3.anchoredPosition.y);

        TryStun(ref stun1, indicator1, zone1);
        TryStun(ref stun2, indicator2, zone2);
        TryStun(ref stun3, indicator3, zone3);

        // Check zonas
        bool correct1 = IsInZone(indicator1, zone1);
        bool correct2 = IsInZone(indicator2, zone2);
        bool correct3 = IsInZone(indicator3, zone3);

        HandleIndicator(ref time1, ref locked1, correct1);
        HandleIndicator(ref time2, ref locked2, correct2);
        HandleIndicator(ref time3, ref locked3, correct3);

        if (correct1 && correct2 && correct3 && !completed)
        {
            completed = true;
            Complete();
        }
    }

    void GenerateZonesBalanced()
    {
        float halfWidth = bar1.rect.width / 2f;

        SetZone(zone1, halfWidth);
        SetZone(zone2, halfWidth);
        SetZone(zone3, halfWidth);
    }

    void SetZone(RectTransform zone, float halfWidth)
    {
        float positionX;

        int tries = 0;

        do
        {
            // genera en toda la barra
            positionX = Random.Range(-halfWidth * 0.9f, halfWidth * 0.9f);
            tries++;

            // evita centro REAL (zona muerta)
        }
        while (Mathf.Abs(positionX) < 150f && tries < 20);

        zone.anchoredPosition = new Vector2(positionX, 0);
    }

    bool IsInZone(RectTransform indicator, RectTransform zone)
    {
        float min = zone.anchoredPosition.x - zone.rect.width / 2f;
        float max = zone.anchoredPosition.x + zone.rect.width / 2f;

        float pos = indicator.anchoredPosition.x;

        return pos >= min && pos <= max;
    }

    void HandleIndicator(ref float timer, ref bool locked, bool isCorrect)
    {
        if (locked) return;

        if (isCorrect)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                locked = true;
            }
        }
        else
        {
            timer = Mathf.Max(0f, timer - Time.deltaTime * 2f);
        }
    }

    void Complete()
    {
        StartCoroutine(CompleteRoutine());
    }

    IEnumerator CompleteRoutine()
    {
        Debug.Log("Triple Panel Repaired");

        humSource.Stop();
        audioSource.PlayOneShot(successSound);
        currentPanel?.PuzzleCompleted();

        onPuzzleCompleted?.Invoke();

        yield return new WaitForSeconds(1f);

        onPuzzleFinished?.Invoke();
    }

    void OnEnable()
    {
        value1 = value2 = value3 = 0f;

        indicator1.anchoredPosition = new Vector2(0f, indicator1.anchoredPosition.y);
        indicator2.anchoredPosition = new Vector2(0f, indicator2.anchoredPosition.y);
        indicator3.anchoredPosition = new Vector2(0f, indicator3.anchoredPosition.y);

        GenerateZonesBalanced();

        humSource.clip = backgroundHum;
        humSource.loop = true;
        humSource.Play();

        // reset estado
        time1 = time2 = time3 = 0f;
        locked1 = locked2 = locked3 = false;
        completed = false;
    }

    public void SetCurrentPanel(ElectricalPanel panel)
    {
        currentPanel = panel;
    }

    void TryStun(ref float stun, RectTransform indicator, RectTransform zone)
    {
        if (stun > 0f) return;

        float distanceToCenter = Mathf.Abs(indicator.anchoredPosition.x - zone.anchoredPosition.x);

        if (distanceToCenter < 2f)
        {
            stun = stunDuration;
        }
    }
}