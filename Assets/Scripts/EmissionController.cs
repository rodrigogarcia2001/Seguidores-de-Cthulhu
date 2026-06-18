using UnityEngine;
using System.Collections.Generic;

public class EmissionController : MonoBehaviour
{
    [SerializeField] private bool startEnabled = true;

    private List<Material> materials = new();
    private List<Color> originalEmissions = new();

    private void Awake()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasProperty("_EmissionColor"))
                {
                    materials.Add(mat);
                    originalEmissions.Add(mat.GetColor("_EmissionColor"));
                }
            }
        }

        if (startEnabled)
            TurnOnEmission();
        else
            TurnOffEmission();
    }

    public void TurnOffEmission()
    {
        foreach (Material mat in materials)
        {
            mat.SetColor("_EmissionColor", Color.black);
        }
    }

    public void TurnOnEmission()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetColor("_EmissionColor", originalEmissions[i]);
        }
    }
}