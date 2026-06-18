using System.Collections.Generic;
using UnityEngine;

public class LightSystemManager : MonoBehaviour
{
    [SerializeField] private List<BrokenSpotlight> spotlights;

    public void RepairLights()
    {
        foreach (BrokenSpotlight spotlight in spotlights)
        {
            spotlight.RepairSpotlight();
        }
    }
}