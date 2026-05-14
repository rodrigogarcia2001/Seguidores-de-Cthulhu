using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    public InventorySystem inventory;
    public SanitySystem sanitySistem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseItem(0);
        }
    }

    void UseItem(int index)
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

        // crear objeto real en escena
        GameObject obj = Instantiate(item.prefab);

        foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        IUsable usable = obj.GetComponent<IUsable>();

        if (usable == null)
        {
            Debug.LogError("El prefab no implementa IUsable");;
            return;
        }

        // usarlo con el player
        usable.Use(gameObject);

        inventory.RemoveItem(index);
    }
}