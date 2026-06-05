using UnityEngine;
using System.Collections;
public class TripleKnobMiniGame : MonoBehaviour
{
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

    [Header("Panel")]
    public ElectricalPanel electricalPanel;

    [Header("Events")]
public GameObject triggerEnemy;
    
    [Header("Light System")]
    public LightSystemManager lightSystemManager;

    [Header("Controls")]
    public bool increase;
    public bool decrease;

    private float value1 = 0f;
    private float value2 = 0f;
    private float value3 = 0f;

    private float correctTime = 0f;
private bool completed = false;
    [Header("Audio")]
public AudioSource audioSource;
public AudioSource humSource;
public AudioClip backgroundHum;

public AudioClip successSound;

    void Start()
    {
        PlaceRandomZone(bar1, zone1);
        PlaceRandomZone(bar2, zone2);
        PlaceRandomZone(bar3, zone3);

        while (Mathf.Abs(zone1.anchoredPosition.x) < 150)
        {
            PlaceRandomZone(bar1, zone1);
        }

        while (Mathf.Abs(zone2.anchoredPosition.x) < 150)
        {
            PlaceRandomZone(bar2, zone2);
        }

        while (Mathf.Abs(zone3.anchoredPosition.x) < 150)
        {
            PlaceRandomZone(bar3, zone3);
        }

    }

    void Update()
    {
        float halfBar = bar1.rect.width / 2f;

        if (increase)
        {
            value1 += 800f * Time.deltaTime;
            value2 += 200f * Time.deltaTime;
            value3 += 400f * Time.deltaTime;
        }

        if (decrease)
        {
            value1 -= 800f * Time.deltaTime;
            value2 -= 200f * Time.deltaTime;
            value3 -= 400f * Time.deltaTime;
        }

        value1 = Mathf.Clamp(value1, -halfBar, halfBar);
        value2 = Mathf.Clamp(value2, -halfBar, halfBar);
        value3 = Mathf.Clamp(value3, -halfBar, halfBar);

        indicator1.anchoredPosition =
            new Vector2(value1, indicator1.anchoredPosition.y);

        indicator2.anchoredPosition =
            new Vector2(value2, indicator2.anchoredPosition.y);

        indicator3.anchoredPosition =
            new Vector2(value3, indicator3.anchoredPosition.y);

        bool correct1 = IsInZone(indicator1, zone1);
        bool correct2 = IsInZone(indicator2, zone2);
        bool correct3 = IsInZone(indicator3, zone3);

        Debug.Log(
            "1=" + correct1 +
            " 2=" + correct2 +
            " 3=" + correct3
        );

        if (correct1 && correct2 && correct3)
        {
            correctTime += Time.deltaTime;

            if (correctTime >= 1f && !completed)
            {
                completed = true;
                Complete();
            }
        }
        else
        {
            correctTime = 0f;
        }
    }

    void PlaceRandomZone(RectTransform bar, RectTransform zone)
    {
        float center;

        if (Random.value < 0.5f)
        {
            // Left side
            center = Random.Range(0.1f, 0.35f);
        }
        else
        {
            // Right side
            center = Random.Range(0.65f, 0.9f);
        }

        float positionX =
            (center - 0.5f) * bar.rect.width;

        zone.anchoredPosition =
            new Vector2(positionX, 0);
    }

    bool IsInZone(RectTransform indicator, RectTransform zone)
    {
        float min =
            zone.anchoredPosition.x - zone.rect.width / 2f;

        float max =
            zone.anchoredPosition.x + zone.rect.width / 2f;

        float indicatorPosition =
            indicator.anchoredPosition.x;

        return indicatorPosition >= min &&
               indicatorPosition <= max;
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

    if (lightSystemManager != null)
    {
        lightSystemManager.RepairAllLights();
    }

    if (triggerEnemy != null)
    {
        triggerEnemy.SetActive(true);
    }

    yield return new WaitForSeconds(1f);

    electricalPanel.CloseMiniGame();
}

void OnEnable()
{
    humSource.clip = backgroundHum;
    humSource.loop = true;
    humSource.Play();
}
}