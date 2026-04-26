using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    public InventorySystem inventory;
    public SistemaCordura sistemaCordura; // 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UsarItem(0);
        }
    }

    void UsarItem(int index)
    {
        if (index >= inventory.items.Count)
        {
            Debug.Log("No hay item");
            return;
        }

        Item item = inventory.items[index];

        if (item.prefab == null)
        {
            Debug.LogError("Item sin prefab");
            return;
        }

        GameObject obj = Instantiate(item.prefab);

        MatchPerformance match = obj.GetComponent<MatchPerformance>();

        if (match == null)
        {
            Debug.LogError("El prefab no tiene MatchPerformance");
            return;
        }
        match.sistemaCordura = sistemaCordura;

        match.Use();

        inventory.RemoveItem(index);
    }
}