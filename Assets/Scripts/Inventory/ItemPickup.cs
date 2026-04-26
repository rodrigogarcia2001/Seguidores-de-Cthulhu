using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public InventorySystem inventory;

    public bool playerInRange = false;

    private void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if(inventory.AddItem(item))
            {
                //Destroy(gameObject);
            }
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
