using System;
using UnityEngine;
public class KeyCounter : MonoBehaviour
{
    public static KeyCounter Instance;
    public int CurrentKeys { get; private set; }
    public int TotalKeys = 3;
    public event Action<int, int> OnKeysChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void RemoveKey()
    {
        if (CurrentKeys <= 0)
            return;

        CurrentKeys--;

        OnKeysChanged?.Invoke(CurrentKeys, TotalKeys);
    }

    public void AddKey()
    {
        CurrentKeys++;

        Debug.Log($"CONTADOR: {CurrentKeys}/{TotalKeys}");

        OnKeysChanged?.Invoke(CurrentKeys, TotalKeys);
    }
}