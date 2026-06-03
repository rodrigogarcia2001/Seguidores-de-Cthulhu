using UnityEngine;
using UnityEngine.UI;
public class UI_Sanity : MonoBehaviour
{
    public Slider bar;
    public SanitySystem system;
    public Image fill;
    void Start()
    {
        bar.maxValue = system.SanityMax;
    }

    void Update()
    {
        bar.value = system.SanityCurrent;

        float percent = system.SanityCurrent / system.SanityMax;

        //fill.color = Color.Lerp(Color.red, Color.green, percent);
    }
}
