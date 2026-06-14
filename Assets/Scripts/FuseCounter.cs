using System;
using UnityEngine;

public class FuseCounter : MonoBehaviour
{
    public static FuseCounter Instance;

    public int CurrentFuses { get; private set; }
    public int TotalFuses = 3;

    public event Action<int, int> OnFusesChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("FuseCounter inicializado");
    }

    public void AddFuse()
    {
        CurrentFuses++;

        Debug.Log($"FUSIBLES: {CurrentFuses}");

        OnFusesChanged?.Invoke(CurrentFuses, TotalFuses);
    }

    public void RemoveFuse()
    {
        if (CurrentFuses <= 0)
            return;

        CurrentFuses--;

        OnFusesChanged?.Invoke(CurrentFuses, TotalFuses);
    }
}