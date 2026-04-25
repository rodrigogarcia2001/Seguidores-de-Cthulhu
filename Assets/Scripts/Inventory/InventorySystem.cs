using UnityEngine;
using System.Collections.Generic;
using System;

public class InventorySystem : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxSlots = 5;

    public event Action onInventoryChanged; // Uso Action porque no tiene parametros
    public bool AddItem(Item item)
    {
        if(items.Count >= maxSlots)
        {
            Debug.Log("Inventary Full");
            return false;
        }

        items.Add(item);
        onInventoryChanged?.Invoke(); 
        return true;
    }
}
