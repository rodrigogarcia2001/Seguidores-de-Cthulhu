using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventorySystem inventory;
    public Transform slotsParent;
    public InventorySlotUI[] slots;

    private void Start()
    {
        slots = slotsParent.GetComponentsInChildren<InventorySlotUI>();
        inventory.onInventoryChanged += UpdateUI;
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].SetItem(inventory.items[i]);
            }
            else
                slots[i].ClearSlot();
        }
    }
}
