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

        system.OnSanityChanged += UpdateBar;

        UpdateBar(system.SanityCurrent);
    }

    private void UpdateBar(float value)
    {
        bar.value = value;
    }

    private void OnDestroy()
    {
        if (system != null)
        {
            system.OnSanityChanged -= UpdateBar;
        }
    }
}
